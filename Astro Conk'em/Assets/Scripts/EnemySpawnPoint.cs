using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour
{
	public float minDifficultyLevel = 0f;
	public float weight = 10f;
	public float randomRotationLeft = 45f;
	public float randomRotationRight = 45f;

	public void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, -randomRotationLeft, 0f) * transform.forward * 5f);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, randomRotationRight, 0f) * transform.forward * 5f);
	}
}
