using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	//Game states
	public enum STATE
	{
		start,
		game,
		gameover
	};

	public struct TransitionFuncts
	{
		public TransitionFuncts(Fnct _init, Fnct _shutdown)
		{
			init = _init;
			shutdown = _shutdown;
		}
		public Fnct init;
		public Fnct shutdown;
	}

	public delegate void Fnct();


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


    [SerializeField]
    private float m_score = 0;


	private Dictionary<int, TransitionFuncts> m_states;

    private int m_currentState;


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

    [SerializeField]
    private MusicController m_musicController;
    public MusicController musicManager
    {
        get
        {
            return m_musicController;
        }
    }

    //This audiosource is used for menu sounds etc
    public static AudioSource globalSoundSource;

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
        globalSoundSource = GetComponent<AudioSource>();
	}

	void Start()
	{
 		//fixed the problem it just needed to be initialised 
        m_states = new Dictionary<int, TransitionFuncts>();
	}

    public int GetCurrentState()
    {
        return m_currentState;
    }

	void Update()
	{
		//Increase the difficulty exponentially
        if(TitleScript.titlePanFinished)
		    curDifficulty *= difficultyExpoOverTime * Time.deltaTime + 1;
	}


	public float GetDifficultyLevel()
	{
		return curDifficulty - initialDifficulty;
	}


	public void registerState(int _index, Fnct _init, Fnct _shutdown)
	{
		TransitionFuncts fncts = new TransitionFuncts(_init, _shutdown);
		m_states.Add(_index, fncts);
	}

	public void changeState(int index)
	{
		//call init funct
		//m_states[m_currentState].shutdown();
		m_currentState = index;
		//m_states[index].init();
        //Debug.Break();
	}

}
