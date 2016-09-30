using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour
{
	public float minDifficultyLevel = 0f;
	public float weight = 10f;
	public bool isUnderGround = false;

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(transform.position, transform.forward * 0.5f);
	}
}
