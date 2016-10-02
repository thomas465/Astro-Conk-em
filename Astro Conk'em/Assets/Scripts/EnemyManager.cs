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
	private float spawnThreshold;

	public GameObject enemyPrefab;

	private EnemySpawnPoint[] spawnPoints;
	private EnemySpawnPoint lastSpawnPoint;


    //Haz these are the holes which the slugs burst through
    //SetActive() one of these objects to have it burst into existance and after that let slugs spawn from them
    //The order of the Entrance objects might not match up with the positions of your spawners you've already arranged so you will need to change the order of the entrances to match up
    public EnemyEntranceScript[] enemyEntrances;

	void Awake()
	{
		//Set the spawn threshold to the initial difficulty
		spawnThreshold = GameManager.instance.initialDifficulty;
	}

	void Start()
	{
		spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
	}

	void Update()
	{
        //Temporary quick way to make them not attack during the title screen
        if (TitleScript.titlePanFinished)
        {

            //If the difficulty before this frame was less than the threshold, and the difficulty after this frame is above the threshold
            if (GameManager.instance.curDifficulty > spawnThreshold)
            {
                //Spawn an enemy
                SpawnEnemy();
                //Debug.Log("Spawn");

                //Increase the threshold
                spawnThreshold += 1f;
            }
        }
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

	private EnemySpawnPoint PickRandomSpawnPoint()
	{
		//Pick a random spawn point
		List<EnemySpawnPoint> possibleSpawnPoints = new List<EnemySpawnPoint>();
		float totalWeights = 0f;
		foreach (EnemySpawnPoint sp in spawnPoints)
		{
			//Dont spawn at points that are too difficult yet
			if (GameManager.instance.curDifficulty < sp.minDifficultyLevel)
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
		enemyList.Remove(enemy);
		disabledEnemyList.Add(enemy);
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
