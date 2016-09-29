using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
   
    public GameObject m_ballPrefab;

    [SerializeField]
    private Vector3 m_baseSpawnForce = new Vector3( 0,200, 0);
    [SerializeField]
    private const int m_maxBalls = 20;
    private BallScript[] m_balls;

    //Yo ozone please get it so that this references the ball which the player will hit next time he swings :)
    public static BallScript currentBall;

	void Start ()
    {
        //Init ball array
        m_balls = new BallScript[m_maxBalls];

        //Instantiate new balls from the m_ballPrefab prefab, cache reference to Ball script
        for (int i = 0; i < m_maxBalls; ++i)
        {
            m_balls[i] = Instantiate(m_ballPrefab).GetComponent<BallScript>();
        }


    }

	void Update ()
    {
        //Just for testing; activate the spawner with spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawnBall();
        }
	}

    //spawn a ball, returns false if it wasn't possible (too many balls in play)
    public bool spawnBall()
    {
        //Find a free ball...
        for (int i = 0; i < m_maxBalls; ++i)
        {
           // if (m_balls[i].isActive() == false)
            {
                //m_balls[i].setActive(true);
               // m_balls[i].gameObject.transform.position = gameObject.transform.position;
                //m_balls[i].m_rigidBody.AddForce(m_baseSpawnForce);
                return true;//early out
            }
        }

        return false;
    }
  
}
