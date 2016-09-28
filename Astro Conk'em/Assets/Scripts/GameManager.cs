using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_difficulty = 0; //start this as whatever is appropriate

    private EnemyManager m_enemyManager;
    private ScoreManager m_scoreManager;
    private PlayerScript m_player;

    // Use this for initialization
    void Start ()
    {
        m_enemyManager = new EnemyManager();
        m_scoreManager = new ScoreManager();
    }
	
	// Update is called once per frame
	void Update ()
    {
       //honestly not convinced that we need a game manager!
       //maybe a persitant score object so that the game over level can display the score is probably about as far as we need
       //because when the player dies we can just Application.loadLevel(Levels.GameOver), on the splash screen and game over we just have a simple
       //scrip that loads Levels.Game after space is pressed...
       //On top of that, all these managers can just be put on empty gameobjects and reference eachother directly.
    }

}
