/// <summary>
/// Created by Sam Endean 28/09/2016
/// 
/// The enemy baseclass governs the standard behaviour for an enemy
/// Enemies will start or be activated through the EnemyManager
/// They will then move towards the player and explode
/// </summary>

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	//reference to the player
	private PlayerScript player;

	//the difficulty modifier for the speed
	[SerializeField]
	private float diffMod;
	[SerializeField]
	private float speed = 1.0f;

	//distance from the player that detonation will happen
	[SerializeField]
	private float explodeDist = 1.0f;

	//they spawn rotated randomly by this much in each direction in degrees
	[SerializeField]
	private float randRotRange = 30f;

	//when they are this far from the player, they will be moving directly towards them
	[SerializeField]
	private float distToMoveDirect = 15f;

    //This is so that they stop slightly in front of the player rather than inside him
    Vector3 offset;

    private Vector3 initialFacing;
	private float initialDist;

    //Particles
    public GameObject standardHitParticles, critHitParticles, burstParticles;

    Collider myCollider;

    bool m_isDead = false;
    //need this for cam AI
    public bool isDead()
    {
        return m_isDead;
    }

    float deathTime = 0;
    Vector3 originalScale;

    bool isBlowingUp = false;
    float explosionDelayCounter = 0, explosionDelayLength = 1;

    Animator anim;
    AudioSource myAudio;

    /// <summary>
    /// Called when an enemy is spawned or respawned
    /// </summary>
    public void Init()
	{
		//aquire a target
		player = GameManager.instance.player;

		//Face the player
		initialFacing = GetVectorToPlayer();

		//Rotate by up to randRotRange
		initialFacing = Quaternion.Euler(0f, Random.Range(-randRotRange, randRotRange), 0f) * initialFacing;

		//Set the initial distance to our current distance to player
		initialDist = GetDistToPlayer();

        m_isDead = false;
        deathTime = 0;

        if(anim)
            anim.ResetTrigger("Detonate");

        if(myCollider)
        {
            myCollider.enabled = true;
        }

        if (originalScale.x > 0)
        {
            transform.localScale = originalScale;
        }

        isBlowingUp = false;
	}

	//will call Init
	private void Start()
	{
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAudio = GetComponent<AudioSource>();

        originalScale = transform.localScale;

        offset = Vector3.forward * 1.5f;

		Init();
	}

    /// <summary>
    /// The enemies will move towards the target and check their proximity
    /// if within range of the player
    /// </summary>
    private void Update()
    {
        //Debug.Log(GameManager.instance.GetCurrentState());
        if (GameManager.instance.GetCurrentState() == (int)GameManager.STATE.game)
        {


            if (!m_isDead)
            {
                //If we are far away from the target position
                if (GetDistToPlayer() > explodeDist)
                {
                    DoMovement();
                    anim.ResetTrigger("Detonate");
                }
                else
                {
                    anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), 0, 3 * Time.deltaTime));
                    Detonate();
                }
            }
            else
            {
                deathTime -= Time.deltaTime;
                anim.ResetTrigger("Detonate");

                if (deathTime < 2)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.25f, 0, 1.25f), 3 * Time.deltaTime);
                    transform.position -= Vector3.up * Time.deltaTime * 1;
                }

                if (deathTime <= 0)
                {
                    //m_isDead = false;
                    GameManager.instance.enemyManager.OnEnemyKilled(this);
                }
            }

        }
    }

    void LateUpdate()
    {
        anim.SetBool("Alive", !m_isDead);
    }

	/// <summary>
	/// Call this when the slug gets hit, currently instakills but we can expand to have hp if needed
	/// </summary>
	public void TakeDamage(bool isCrit, Vector3 hitDirection)
	{
        if (m_isDead)
        {

        }
        else
        {
            myAudio.Stop();
            myAudio.PlayOneShot(SoundBank.sndBnk.GetHitSound());

            if (isCrit)
            {
                GameObject hit = Instantiate(critHitParticles, transform.position, Quaternion.LookRotation(GetVectorToPlayer())) as GameObject;
                hit.GetComponent<HitParticleScript>().myDir = -hitDirection;

                Destroy(hit, 3);
            }
            else
            {
                GameObject hit = Instantiate(standardHitParticles, transform.position, Quaternion.LookRotation(GetVectorToPlayer())) as GameObject;
                hit.GetComponent<HitParticleScript>().myDir = -hitDirection;

                Destroy(hit, 3);
            }

            ScreenShake.g_instance.shake(0.28f, 0.1f);
            m_isDead = true;

            myCollider.enabled = false;

            deathTime = 4;

            ScoreManager.scoreSingleton.AddScore(100);
        }
    }

	/// <summary>
	/// Handles basic enemy AI, doesnt just move straight towards player
	/// </summary>
	private void DoMovement()
	{
		//Current distance to player
		float dist = GetDistToPlayer();

		//Steer towards the player
		Vector3 forward = Vector3.Slerp(initialFacing, GetVectorToPlayer(), Mathf.InverseLerp(initialDist, distToMoveDirect, dist));

		//Set the rotation
		transform.rotation = Quaternion.LookRotation(forward);

		//Move forward
		Move(forward.normalized);

        anim.SetFloat("Speed", speed);
	}

	private void Move(Vector3 _dir)
	{
		float s = speed * Time.deltaTime + GameManager.instance.GetDifficultyLevel() * diffMod;
		transform.position += _dir * s;
	}

	private Vector3 GetVectorToPlayer()
	{
		Vector3 dir = (player.transform.position + offset) - transform.position;
		dir.Scale(new Vector3(1f, 0f, 1f)); //Remove everything in y component
		return dir;
	}

	private float GetDistToPlayer()
	{
		return Vector3.Distance
		(
			new Vector3(transform.position.x, 0f, transform.position.z),
			new Vector3(player.transform.position.x, 0f, player.transform.position.z) + offset
		);
	}

    /// <summary>
    /// Detonate and damage the player.
    /// </summary>
    private void Detonate()
    {
        if (!isBlowingUp)
        {
            if (explosionDelayCounter < explosionDelayLength)
            {
                explosionDelayCounter += Time.deltaTime;
            }
            else
            {

                {
                    explosionDelayCounter = 0;
                    myAudio.PlayOneShot(SoundBank.sndBnk.slugInflationSound);
                    anim.SetTrigger("Detonate");
                    isBlowingUp = true;
                    //Debug.Break();
                }

            }
        }
    }

    private void Explode()
    {
        ScreenShake.g_instance.shake(0.5f);
        GameManager.instance.player.TakeHit(35);

        GameObject hit = Instantiate(burstParticles, transform.position, Quaternion.LookRotation(GetVectorToPlayer())) as GameObject;
        Destroy(hit, 3);

        GameManager.instance.enemyManager.OnEnemyKilled(this);
        transform.position -= Vector3.up * 100;
        //isBlowingUp = false;
        m_isDead = true;
    }
}
