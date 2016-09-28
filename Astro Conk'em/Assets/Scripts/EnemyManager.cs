using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{

	[System.Serializable]
	public class SpawnPoint
	{
		public Transform transform;
		public float minDifficultyLevel;
		public float weight;
		public bool isUnderGround;
	}


	//List of all active enemies
	private List<Enemy> enemyList = new List<Enemy>();

	//List of enemies in the 'pseudo object pool'
	//These are enemies that have been spawned and then killed
	private List<Enemy> disabledEnemyList = new List<Enemy>();

	//The initial difficulty level
	public float initialDifficulty = 20f;

	//The current difficulty 'level'
	//Starts at initialDifficulty
	//Every frame exponentially increases, with the expo as (difficultyExpOverTime * Time.deltaTime)
	private float curDifficulty;

	//The exponent to increase curDifficulty every frame, per delta time
	//Essentially the 'ramp up' of difficulty
	public float difficultyExpoOverTime = 0.01f;

	//Spawns an enemy when curDifficulty ticks over this value
	//Increases by 1 every time an enemy spawns
	private float spawnThreshold;

	public GameObject enemyPrefab;

	public List<SpawnPoint> spawnPoints;
	private SpawnPoint lastSpawnPoint;

	void Awake()
	{
		//Set the current difficulty to the initial difficulty
		curDifficulty = initialDifficulty;

		//Set the spawn threshold to the initial difficulty
		spawnThreshold = initialDifficulty;
	}

	void Update()
	{
		//Difficulty before this frame
		float preDifficulty = curDifficulty;

		//Increase the difficulty exponentially
		curDifficulty *= difficultyExpoOverTime * Time.deltaTime + 1;

		//If the difficulty before this frame was less than the threshold, and the difficulty after this frame is above the threshold
		if (preDifficulty <= spawnThreshold && curDifficulty > spawnThreshold)
		{
			//Spawn an enemy
			SpawnEnemy();
			//Debug.Log("Spawn");

			//Increase the threshold
			spawnThreshold += 1f;
		}
	}

	public void SpawnEnemy()
	{
		//Get either a new enemy or take a used one out of the pool
		Enemy enemy = GetNewEnemy();

		//Pick a random spawn point
		SpawnPoint sp = PickRandomSpawnPoint();

		//Set the enemy to be at the spawn point
		enemy.transform.position = sp.transform.position;
		enemy.transform.rotation = sp.transform.rotation;

		//TODO: Call some 'start' function on the enemy, with sp.isUnderGround, maybe other stuff
	}

	private SpawnPoint PickRandomSpawnPoint()
	{
		//Pick a random spawn point
		List<SpawnPoint> possibleSpawnPoints = new List<SpawnPoint>();
		float totalWeights = 0f;
		foreach (SpawnPoint sp in spawnPoints)
		{
			//Dont spawn at points that are too difficult yet
			if (curDifficulty < sp.minDifficultyLevel)
			{
				continue;
			}

			//Dont spawn at points that we just spawned from
			if (sp == lastSpawnPoint)
			{
				continue;
			}

			possibleSpawnPoints.Add(sp);
			totalWeights += sp.weight;
		}

		float randomWeight = Random.Range(0f, totalWeights);

		foreach (SpawnPoint sp in possibleSpawnPoints)
		{
			randomWeight -= sp.weight;
			if (randomWeight < 0f)
			{
				lastSpawnPoint = sp;
				return sp;
			}
		}

		Debug.LogError("randomWeight mis-calculation (Total = " + totalWeights + ", Chosen = " + randomWeight + ")");
		return possibleSpawnPoints[0];
	}

	private Enemy GetNewEnemy()
	{
		Enemy enemy;
		if (disabledEnemyList.Count == 0)
		{
			//No enemies in the 'dead' list, instantiate a new one
			GameObject GO = Instantiate(enemyPrefab);
			enemy = GO.GetComponent<Enemy>();
			enemyList.Add(enemy);
			return enemy;
		}

		enemy = disabledEnemyList[disabledEnemyList.Count - 1]; //Get the last enemy in the dead enemy list

		disabledEnemyList.RemoveAt(disabledEnemyList.Count - 1); //Remove them from the dead list

		enemyList.Add(enemy);

		enemy.gameObject.SetActive(true);

		return enemy;
	}

	public void OnEnemyKilled(Enemy enemy)
	{
		enemyList.Remove(enemy);
		disabledEnemyList.Add(enemy);

		enemy.gameObject.SetActive(false);
	}

}
