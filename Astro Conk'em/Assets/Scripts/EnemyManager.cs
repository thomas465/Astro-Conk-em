using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
	
	//List of all active enemies
	private List<Enemy> enemyList = new List<Enemy>();

	//List of enemies in the 'pseudo object pool'
	//These are enemies that have been spawned and then killed
	private List<Enemy> disabledEnemyList = new List<Enemy>();

	//Spawns an enemy when curDifficulty ticks over this value
	//Increases by 1 every time an enemy spawns
	//Starts negative so we spawn a bunch immediately when the game starts
	private float spawnThreshold = -5f;

	public GameObject enemyPrefab;

	//How many enemies to spawn on the next wave
	private int numEnemysToSpawn;

	[SerializeField]
	private int waveSpawnCountMin = 2;

	[SerializeField]
	private int waveSpawnCountMax = 5;

	[SerializeField]
	private float waveSpawnTimeMult = 0.8f;

	//Chance to skip spawning a single slug and spawn multiple next time
	[SerializeField]
	private float chanceToSpawnWave = 0.2f;

	private EnemySpawnPoint[] spawnPoints;
	private EnemySpawnPoint lastSpawnPoint;
	
	void Start()
	{
		spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
	}

	void Update()
	{
        //Temporary quick way to make them not attack during the title screen
        if (TitleScript.titlePanFinished)
		{
			foreach(EnemySpawnPoint sp in spawnPoints)
			{
				EnemyEntranceScript[] es = sp.GetComponentsInChildren<EnemyEntranceScript>(true);
				if(es.Length == 0)
				{
					Debug.LogError("EnemySpawnPoint does not have Entrance script as child!");
					Destroy(sp.gameObject);
					continue;
				}
				EnemyEntranceScript entrance = es[0];

				if(entrance == null || entrance.isActiveAndEnabled)
				{
					continue;
				}

				if(sp.minDifficultyLevel < GameManager.instance.GetDifficultyLevel())
				{
					entrance.gameObject.SetActive(true);
				}
			}


			//If the difficulty before this frame was less than the threshold, and the difficulty after this frame is above the threshold
			if (GameManager.instance.GetDifficultyLevel() >= spawnThreshold)
            {

				if(Random.value < chanceToSpawnWave)
				{
					int waveSize = Random.Range(waveSpawnCountMin, waveSpawnCountMax);
					numEnemysToSpawn += waveSize;
					spawnThreshold += waveSize * waveSpawnTimeMult;
				}
				else
				{
					//Spawn an enemy
					SpawnWave();
					//Debug.Log("Spawn");
				}

				//Increase the threshold
				spawnThreshold += 1f;
            }


			if(enemyList.Count < 3)
			{
				//Most enemies are dead, spawn some more in immediately
				SpawnWave();
			}
        }
	}

	private void SpawnWave()
	{
		while(numEnemysToSpawn > 0)
		{
			SpawnEnemy();
			numEnemysToSpawn--;
		}

		//Next wave starts with atleast 1 enemy
		numEnemysToSpawn = 1;
	}

	public void SpawnEnemy()
	{
		//Get either a new enemy or take a used one out of the pool
		Enemy enemy = GetNewEnemy();

		//Pick a random spawn point
		EnemySpawnPoint sp = PickRandomSpawnPoint();

		//Set the enemy to be at the spawn point
		enemy.transform.position = sp.transform.position;
		enemy.transform.rotation = sp.transform.rotation;

		enemy.Init();
	}

    public void EnemyHasExploded(Enemy exploder)
    {
        float explosionDistance = 6;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (Vector3.Distance(enemyList[i].transform.position, exploder.transform.position) < explosionDistance)
            {
                if(!enemyList[i].isDead())
                {
                    ScoreManager.scoreSingleton.BallHit();
                    enemyList[i].GetExploded(true);
                }
            }
        }
    }

	private EnemySpawnPoint PickRandomSpawnPoint()
	{
		//Pick a random spawn point
		List<EnemySpawnPoint> possibleSpawnPoints = new List<EnemySpawnPoint>();
		float totalWeights = 0f;
		foreach (EnemySpawnPoint sp in spawnPoints)
		{
			//Dont spawn at points that are too difficult yet
			if (GameManager.instance.GetDifficultyLevel() < sp.minDifficultyLevel)
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

		foreach (EnemySpawnPoint sp in possibleSpawnPoints)
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
		//Sometimes this is called multiple times
		if(enemyList.Remove(enemy))
		{
			disabledEnemyList.Add(enemy);
		}
	}

    //Added some public functions for gettin' at enemies for
    //the billboard cam to calculate interesting POVs
    public int getNumActiveEnemies()
    {
        return enemyList.Count;
    }

    public Enemy getEnemy(int index)
    {
        return enemyList[index];
    }

    public Enemy[] copyActiveEnemies()
    {
        return enemyList.ToArray();
    }
}
