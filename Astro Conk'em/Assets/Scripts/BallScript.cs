﻿using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    public enum BALL_STATE
    {
        NOT_IN_USE,
        SPAWNING,
        READY_FOR_PLAYER_HIT,
        HAS_BEEN_HIT,
        HIT_SOMETHING
    }

    //--gravity fall off thing
    public float m_gravityTimerConstant;
    private float m_gravityTimer;
    private float m_gravityTimerMax;
    //--

    public ParticleSystem standardHit, critHit, critFire;
    public ParticleSystem standardDamage;

    public ParticleSystem spawnParticles;

    public TrailRenderer trail, critTrail;

    private Rigidbody rb;
    private BALL_STATE state;

    bool isDangerous = false;

    //float around position
    private float m_mag = 0.1f;
    private float m_currentLerpValue = 0.0f;
    private Transform m_spwnPosTransform;
    private Vector3 m_start;
    private Vector3 m_target;
    private float m_lerpValue = 3.0f;

    //float around rotation
    private float m_torqueModifier = 3.0f;

    AudioSource myAudio;

    public BALL_STATE getState()
    {
        return state;
    }

    // Use this for initialization
    void Awake()
    {
        m_spwnPosTransform = GameObject.Find("BallSpawnPos").transform;
        m_target = m_spwnPosTransform.position;
        state = BALL_STATE.NOT_IN_USE;
        rb = GetComponent<Rigidbody>();

        transform.rotation = Random.rotation;
        disableTrails();

        myAudio = GetComponent<AudioSource>();

        //--Gravity stuffs
        m_gravityTimerConstant = 0.8f;
        m_gravityTimer = 0.0f;
        m_gravityTimerMax = 0.0f;
    }
    public void disableTrails()
    {
        trail.enabled = false;
        critTrail.enabled = false;
    }
    public void enableTrails()
    {
        trail.enabled = true;
        critTrail.enabled = true;
    }

    public void spwaningBall()
    {
        ResetParticles();
        disableTrails();
        //rb.useGravity = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        state = BALL_STATE.SPAWNING;

        spawnParticles.Play();

        if (BallSpawner.hoverParticles)
            BallSpawner.hoverParticles.Play();
    }
    public void readyForPlayerHit()
    {
        state = BALL_STATE.READY_FOR_PLAYER_HIT;
        rb.AddTorque(Random.insideUnitSphere * m_torqueModifier, ForceMode.Force);
        m_start = transform.position;
        m_target = m_spwnPosTransform.position + (Random.insideUnitSphere * m_mag);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 0.1f && isDangerous)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }

        if (rb.velocity.magnitude < 2.0f)
        {
            isDangerous = false;
        }

        //@@GRAVITY STUFF
        if (m_gravityTimer >= m_gravityTimerMax)
        {
            rb.useGravity = true;
        }
        m_gravityTimer += Time.deltaTime;

        //Positioning of crit trail
        Vector3 critTrailOffset = transform.forward * rb.velocity.magnitude / 15;
        critTrail.transform.position = transform.position + critTrailOffset;


        if (state == BALL_STATE.READY_FOR_PLAYER_HIT)
        {
            floatAround();
        }

    }

    private void floatAround()
    {
        //If we are at our target position...
        if (gameObject.transform.position == m_target)
        {
            //Get new target
            m_target = m_spwnPosTransform.position + (Random.insideUnitSphere * m_mag);
            m_start = transform.position;
            //Reset lerpval
            m_currentLerpValue = 0;
        }
        //Lerp to target
        gameObject.transform.position = Vector3.Lerp(m_start, m_target, m_currentLerpValue);

        //Update lerpd
        m_currentLerpValue += m_lerpValue * Time.deltaTime;// * Mathf.Sin(Mathf.PI  * m_currentLerpValue) + sinYOffset;
    }
    public void ResetParticles()
    {
        trail.Clear();
        critTrail.Clear();

        critHit.Stop();

        critFire.Stop();
        critFire.Clear();
    }

    public void HitByPlayer(float power, Vector3 dir)
    {
        BallSpawner.hoverParticles.Stop();
        BallSpawner.hoverParticles.Clear();

        enableTrails();

        //@@GRAVITY STUFF
        m_gravityTimer = 0;
        m_gravityTimerMax = m_gravityTimerConstant * power;

        isDangerous = true;
        bool isCrit = PowerbarScript.powerbarSingleton.isCrit;

        //All crits have the same speed
        if (isCrit)
        {
            power = 1;
            myAudio.pitch = Random.Range(0.9f, 1.1f);
            myAudio.PlayOneShot(SoundBank.sndBnk.batCritBall);
        }
        else
        {
            myAudio.pitch = Random.Range(0.9f, 1.1f);
            myAudio.PlayOneShot(SoundBank.sndBnk.batHitBall, power + 0.15f);
        }

        float speed = 10;
        speed += power * 30;
        rb.velocity = dir * speed;

        ResetParticles();



        critTrail.enabled = isCrit;

        if (power < 0.15f)
        {
            trail.enabled = false;
            rb.useGravity = true;
        }
        else
        {
            trail.enabled = true;
            rb.useGravity = false;

            if (isCrit)
            {
                ScreenShake.g_instance.shake(0.2f, 0.12f);//powerbar being high already makes this shake bigger so this will be additive; doesn't need to be so large
                critHit.Play();
                critFire.Play();
            }
            else
            {
                standardHit.Play();
            }
        }


        state = BALL_STATE.HAS_BEEN_HIT;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        rb.useGravity = true;

        ResetParticles();
        disableTrails();

        if (state == BALL_STATE.HAS_BEEN_HIT && isDangerous)
        {
            Enemy enemy = collisionInfo.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(PowerbarScript.powerbarSingleton.isCrit, rb.velocity.normalized);
                //inform the scoremanager that the ball hit
                ScoreManager.scoreSingleton.BallHit();
            }
            else
            {
                //let the score manager know that a miss occured
                ScoreManager.scoreSingleton.BallMissed();
                isDangerous = false;//not dangerous if we hit a slug or the ground!
            }

            state = BALL_STATE.HIT_SOMETHING;

        }
    }
}
