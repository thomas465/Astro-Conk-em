﻿/// <summary>
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
	[SerializeField] private float diffMod;
	[SerializeField] private float speed = 1.0f;

	//distance from the player that detonation will happen
	[SerializeField] private float explodeDist = 1.0f;

	//the target location to move to (the player)
	[SerializeField] private Vector3 target;

	/// <summary>
	/// Called when an enemy is spawned or respawned
	/// if the target is not known, get the target from the player pos
	/// </summary>
	public void Init()
	{
		//aquire a target
		//get pos of player
		//target = player.transform.position;
	}

	//will call Init
    private void Start()
    {
		Init ();
    }
		
	/// <summary>
	/// The enemies will move towards the target and check their proximity
	/// if within range of the player
	/// </summary>
    private void Update()
    {
		if (Vector3.Distance (transform.position, target) > explodeDist)
		{
			Move ((target - transform.position).normalized);
		}
		else
		{
			Detonate ();
		}
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
