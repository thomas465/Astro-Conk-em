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

	private Vector3 initialFacing;
	private float initialDist;

    //Particles
    public GameObject standardHitParticles, critHitParticles;

    Collider myCollider;

    bool isDead = false;
    float deathTime = 0;
    Vector3 originalScale;

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

        isDead = false;
        deathTime = 0;

        if(myCollider)
        {
            myCollider.enabled = true;
        }

        if (originalScale.x > 0)
        {
            transform.localScale = originalScale;
        }
	}

	//will call Init
	private void Start()
	{
        anim = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAudio = GetComponent<AudioSource>();

        originalScale = transform.localScale;

		Init();
	}

	/// <summary>
	/// The enemies will move towards the target and check their proximity
	/// if within range of the player
	/// </summary>
	private void Update()
	{
        if (!isDead)
        {
            //If we are far away from the target position
            if (GetDistToPlayer() > explodeDist)
            {
                DoMovement();
            }
            else
            {
                Detonate();
            }
        }
        else
        {
            deathTime -= Time.deltaTime;

            if(deathTime<2)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.25f, 0, 1.25f), 3 * Time.deltaTime);
                transform.position -= Vector3.up * Time.deltaTime * 1;
            }

            if(deathTime<=0)
            {
                isDead = false;
                GameManager.instance.enemyManager.OnEnemyKilled(this);
            }
        }
	}

    void LateUpdate()
    {
        anim.SetBool("Alive", !isDead);
    }

	/// <summary>
	/// Call this when the slug gets hit, currently instakills but we can expand to have hp if needed
	/// </summary>
	public void TakeDamage(bool isCrit, Vector3 hitDirection)
	{
        if (isDead)
            return;

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

        ScreenShake.g_instance.shake(0.4f, 0.1f);
        isDead = true;

        myCollider.enabled = false;

        deathTime = 4;
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
	}

	private void Move(Vector3 _dir)
	{
		float s = speed * Time.deltaTime + GameManager.instance.GetDifficultyLevel() * diffMod;
		transform.position += _dir * s;
	}

	private Vector3 GetVectorToPlayer()
	{
		Vector3 dir = player.transform.position - transform.position;
		dir.Scale(new Vector3(1f, 0f, 1f)); //Remove everything in y component
		return dir;
	}

	private float GetDistToPlayer()
	{
		return Vector3.Distance
		(
			new Vector3(transform.position.x, 0f, transform.position.z),
			new Vector3(player.transform.position.x, 0f, player.transform.position.z)
		);
	}

	/// <summary>
	/// Detonate and damage the player.
	/// </summary>
	private void Detonate()
	{
		//place detonation logic here (Explode, inform the player, inform the EnemyManager
		
		
		//GameManager.instance.enemyManager.OnEnemyKilled(this);
	}
}
