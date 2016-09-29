using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

    public ParticleSystem standardHit, critHit, critFire;
    public ParticleSystem standardDamage;

    public TrailRenderer trail, critTrail;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        BallSpawner.currentBall = this;
	}
	
	// Update is called once per frame
	void Update () {

        if(rb.velocity.magnitude>0.1f)
            transform.rotation = Quaternion.LookRotation(rb.velocity);

        //Positioning of crit trail
        Vector3 critTrailOffset = transform.forward * rb.velocity.magnitude / 8;
        critTrail.transform.position = transform.position + critTrailOffset;
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
        bool isCrit = PowerbarScript.powerbarSingleton.isCrit;

        //All crits have the same speed
        if(isCrit)
        {
            power = 1;
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
                ScreenShake.screenShake.shake(0.4f);
                critHit.Play();
                critFire.Play();
            }
            else
            {
                standardHit.Play();
            }
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        rb.useGravity = true;

        trail.enabled = false;
        critTrail.enabled = false;

        ResetParticles();
    }
}
