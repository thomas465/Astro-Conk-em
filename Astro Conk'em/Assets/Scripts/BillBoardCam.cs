using UnityEngine;
using System.Collections;

public class BillBoardCam : MonoBehaviour
{
    //references to managers of objects of interest
    private PlayerScript m_player;
    
    private BallSpawner m_ballManager;

    private Vector3 m_target;
    private bool m_lookForNewTarget;
    private INTEREST m_currentInterest;
    private float m_maxInterestDuration;

    private float m_currentInterstDuration;

    private BBCamInterests[] m_interests;

    //billboard camera interests with precedence (0 is greatest)
    public enum INTEREST
    {
        CLOSE_ENEMY,
        BUNCH_OF_ENEMIES,
        BALL_HIT,
        PLAYER,
        SINLGE_ENEMY,
        WIDE_ANGLE_FIELD,

        //length of enum
        LENGTH
    }

	// Use this for initialization
	void Start ()
    {
       
        m_ballManager = GameObject.Find("BallSpawner").GetComponent<BallSpawner>();
        m_player = GameObject.Find("PLAYER").GetComponent<PlayerScript>();

        m_interests = new BBCamInterests[(int)INTEREST.LENGTH];
        m_interests[(int)INTEREST.CLOSE_ENEMY] = new CloseEnemy(this);
        m_interests[(int)INTEREST.BUNCH_OF_ENEMIES] = new CloseEnemy(this);
        m_interests[(int)INTEREST.BALL_HIT] = new CloseEnemy(this);
        m_interests[(int)INTEREST.PLAYER] = new CloseEnemy(this);
        m_interests[(int)INTEREST.SINLGE_ENEMY] = new CloseEnemy(this);
        m_interests[(int)INTEREST.WIDE_ANGLE_FIELD] = new CloseEnemy(this);


        m_currentInterest = INTEREST.WIDE_ANGLE_FIELD;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_currentInterstDuration >= m_maxInterestDuration)
        {
            findNewInterest();
            m_currentInterstDuration = 0;
        }


        m_interests[(int)m_currentInterest].interestUpdate();
        m_currentInterstDuration += Time.deltaTime;
    }

    private void findNewInterest()
    {
        //systematically look for most desirable interest
        //that isn't the current interest
        //NOTE: @@maybe have the chance for a random interest  
        //sometimes, to mix it up
        for (int i = 0; i < m_interests.Length; ++i)
        {
            if ((INTEREST)i != m_currentInterest && m_interests[i].interested())
            {
                m_currentInterest = (INTEREST)i;
                break;
            }
        }
    }

}
