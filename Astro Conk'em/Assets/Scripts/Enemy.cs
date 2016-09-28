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
	//############################## MAKE A PUBLIC REFERENCE TO THE PLAYER

	//the difficulty modifier for the speed
	[SerializeField] private float diffMod;
	[SerializeField] private float speed = 1.0f;

	//distance from the player that detonation will happen
	[SerializeField] private float explodeDist = 0.1f;

	//the target location to move to (the player)
	[SerializeField] private Vector3 target;

	/// <summary>
	/// Called when an enemy is spawned or respawned
	/// if the target is not known, get the target from the player pos
	/// </summary>
	public void Init()
	{
		//if no target has been aquired, aquire a target
		if (target.x == float.NaN)
		{
			//get pos of player
			//########################################################### POS OF PLAYER
		}
	}

	//will call Init
    private void Start()
    {
		target = new Vector3(float.NaN, float.NaN, float.NaN);
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
		transform.position += _dir * speed;
	}

	/// <summary>
	/// Detonate and damage the player.
	/// </summary>
	private void Detonate()
	{
		
	}
}
