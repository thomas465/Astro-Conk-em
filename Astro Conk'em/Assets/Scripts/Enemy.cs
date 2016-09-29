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
	public PlayerScript player;

	//the difficulty modifier for the speed
	[SerializeField]
	private float diffMod;
	[SerializeField]
	private float speed = 1.0f;

	//distance from the player that detonation will happen
	[SerializeField]
	private float explodeDist = 1.0f;

	[SerializeField]
	private float randRotRange = 30f;


	private Vector3 initialFacing;
	private float initialDist;

	/// <summary>
	/// Called when an enemy is spawned or respawned
	/// if the target is not known, get the target from the player pos
	/// </summary>
	public void Init()
	{
		//Face the player
		initialFacing = player.transform.position - transform.position;

		//Rotate by up to randRotRange
		initialFacing = Quaternion.Euler(0f, Random.Range(-randRotRange, randRotRange), 0f) * initialFacing;

		//Set the initial distance to our current distance to player
		initialDist = Vector3.Distance(transform.position, player.transform.position);
	}

	//will call Init
	private void Start()
	{
		//aquire a target
		player = GameManager.instance.player;

		Init();
	}

	/// <summary>
	/// The enemies will move towards the target and check their proximity
	/// if within range of the player
	/// </summary>
	private void Update()
	{
		//If we are far away from the target position
		if ((transform.position - player.transform.position).sqrMagnitude > explodeDist * explodeDist)
		{
			DoMovement();
		}
		else
		{
			Detonate();
		}
	}

	/// <summary>
	/// Handles basic enemy AI, doesnt just move straight towards player
	/// </summary>
	private void DoMovement()
	{
		//Current distance to player
		float dist = Vector3.Distance(transform.position, player.transform.position);

		//Steer towards the player
		Vector3 forward = Vector3.Slerp(initialFacing, player.transform.position - transform.position, Mathf.InverseLerp(initialDist, 0f, dist));

		//Set the rotation
		transform.rotation = Quaternion.LookRotation(forward);

		//Move forward
		Move(forward.normalized);
	}

	private void Move(Vector3 _dir)
	{
		transform.position += _dir * speed * Time.deltaTime;
	}

	/// <summary>
	/// Detonate and damage the player.
	/// </summary>
	private void Detonate()
	{
		//place detonation logic here (Explode, inform the player, inform the EnemyManager
	}
}
