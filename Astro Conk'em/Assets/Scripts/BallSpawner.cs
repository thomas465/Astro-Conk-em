using UnityEngine;
using System.Collections;

public class BallSpawner : MonoBehaviour
{
   
    public GameObject m_ballPrefab;


    [SerializeField]
    private const int m_maxBalls = 2;
    private BallScript[] m_balls;

    //Yo ozone please get it so that this references the ball which the player will hit next time he swings :)
    public static BallScript currentBall;
   
  
    public Vector3 m_spawnTarget;
    private Vector3 m_spawnStart;
    private Vector3 m_spawnForce;
    public float m_lerpChange;
    private float m_currentLerp = 0;
    private bool spawning = false;

    private int nextBall = 0;



	void Start ()
    {
        //Init ball array
        m_balls = new BallScript[m_maxBalls];

        //Instantiate new balls from the m_ballPrefab prefab, cache reference to Ball script
        for (int i = 0; i < m_maxBalls; ++i)
        {
            m_balls[i] = Instantiate(m_ballPrefab).GetComponent<BallScript>();
            m_balls[i].gameObject.transform.position = new Vector3(0, -100, 0);
            m_balls[i].GetComponent<Rigidbody>().isKinematic = true;
        }

        currentBall = m_balls[0];
        m_spawnStart = transform.position;
        m_spawnTarget = GameObject.Find("BallSpawnPos").transform.position;
    }

	void Update ()
    {
  
        //Just for testing; activate the spawner with spacebar
        if (Input.GetKeyDown(KeyCode.Space) && currentBall.getState() != BallScript.BALL_STATE.SPAWNING)
        {
            spawnBall();
        }

        if (currentBall.getState() == BallScript.BALL_STATE.SPAWNING)
        {
            currentBall.gameObject.transform.position = Vector3.Lerp(m_spawnStart,m_spawnTarget, m_currentLerp);
            
            if (m_currentLerp >= 1)
            {
                currentBall.GetComponent<Rigidbody>().isKinematic = false;
                currentBall.readyForPlayerHit();
            }
            else
            {
                m_currentLerp += m_lerpChange* Time.deltaTime;
            }
        }
	}

    private void readyUpCurrentBall(int index)
    {
        currentBall = m_balls[index];
        currentBall.spwaningBall();
        m_currentLerp = 0;
    }
   

    public void spawnBall()
    {
        BallScript.BALL_STATE ballState =currentBall.getState();
        //don't spawn a ball if we have one already!
        if (ballState == BallScript.BALL_STATE.HAS_BEEN_HIT || ballState == BallScript.BALL_STATE.NOT_IN_USE)
        {

            ballState = m_balls[nextBall].getState();
            //don't spawn a ball if there are none left (this will effectively never happen as all of them would have to be spawning, but leave it here for now)
            if (ballState == BallScript.BALL_STATE.NOT_IN_USE || ballState == BallScript.BALL_STATE.HAS_BEEN_HIT)
            {
                readyUpCurrentBall(nextBall);
                nextBall = nextBall >= m_maxBalls - 1 ? 0 : nextBall + 1;
                //have balls that are in HAS_BEEN_HIT state play an explosion on the frame they're reused!
                spawning = true;
            }

        }
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject == currentBall)
        {
            //currentBallIsReady =true;
            //Vector3 displacement = transform.position - currentBall.transform.position;
            //float pullMag = 0.5f;
            //currentBall.rb.AddForce(displacement.normalized * pullMag);
        }
    }
}
