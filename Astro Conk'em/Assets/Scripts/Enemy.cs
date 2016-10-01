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
	}

	//will call Init
	private void Start()
	{
		Init();
	}

	/// <summary>
	/// The enemies will move towards the target and check their proximity
	/// if within range of the player
	/// </summary>
	private void Update()
	{
		//If we are far away from the target position
		if(GetDistToPlayer() > explodeDist)
		{
			DoMovement();
		}
		else
		{
			Detonate();
		}
	}

	/// <summary>
	/// Call this when the slug gets hit, currently instakills but we can expand to have hp if needed
	/// </summary>
	public void TakeDamage()
	{
		GameManager.instance.enemyManager.OnEnemyKilled(this);

		//Probably want this ot be a function on the scoreManager
		//GameManager.instance.scoreManager.score += 1;
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
