using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	private static GameManager s_instance;

	/// <summary>
	/// Get the singleton-instance
	/// </summary>
	public static GameManager instance
	{
		get
		{
			return s_instance;
		}
	}


	//The initial difficulty level
	public float initialDifficulty = 20f;

	//The current difficulty 'level'
	//Starts at initialDifficulty
	//Every frame exponentially increases, with the expo as (difficultyExpOverTime * Time.deltaTime)
	public float curDifficulty;

	//The exponent to increase curDifficulty every frame, per delta time
	//Essentially the 'ramp up' of difficulty
	public float difficultyExpoOverTime = 0.01f;


	[SerializeField]
	private EnemyManager m_enemyManager;
	public EnemyManager enemyManager
	{
		get
		{
			return m_enemyManager;
		}
	}

	[SerializeField]
	private ScoreManager m_scoreManager;
	public ScoreManager scoreManager
	{
		get
		{
			return m_scoreManager;
		}
	}

	[SerializeField]
	private PlayerScript m_player;
	public PlayerScript player
	{
		get
		{
			return m_player;
		}
	}



	void Awake()
	{
		if(s_instance != null)
		{
			Debug.LogError("Multiple GameManagers, destroying this");
			Destroy(this);
			return;
		}

		s_instance = this;


		//Set the current difficulty to the initial difficulty
		curDifficulty = initialDifficulty;
	}

	void Start()
	{

	}


	void Update()
	{
		//Increase the difficulty exponentially
		curDifficulty *= difficultyExpoOverTime * Time.deltaTime + 1;
	}

}
