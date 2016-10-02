using UnityEngine;
using System.Collections;
//TODO: change implementation to look for more interesting things every x-seconds
//instead of re-chosing interest when timer runs out 
public class BillBoardCam : MonoBehaviour
{

    //private Vector3 m_target;
    //private bool m_lookForNewTarget;
    [SerializeField]
    private INTEREST m_currentInterest;
    [SerializeField]
    private float m_weightUpdateDelta;
    [SerializeField]
    private float m_timeFromLastWeightUpdate;
    [SerializeField]
    private float m_interestChangeDelta;
    [SerializeField]
    private float m_timeFromLastInterestChange;
    [SerializeField]
    private float m_currentWeight;
    private BBCamInterests[] m_interests;

    public float[] debugWeights;
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
        debugWeights = new float[(int)INTEREST.LENGTH];
         //m_ballManager = GameObject.Find("BallSpawner").GetComponent<BallSpawner>();
         //m_player = GameObject.Find("PLAYER").GetComponent<PlayerScript>();

        m_interests = new BBCamInterests[(int)INTEREST.LENGTH];
        m_interests[(int)INTEREST.CLOSE_ENEMY] = new CloseEnemy(this);
        m_interests[(int)INTEREST.BUNCH_OF_ENEMIES] = new BunchOfEnemies(this);
        m_interests[(int)INTEREST.BALL_HIT] = new BallHit(this);
        m_interests[(int)INTEREST.PLAYER] = new Player(this);
        m_interests[(int)INTEREST.WIDE_ANGLE_FIELD] = new WideAngleField(this);
        m_interests[(int)INTEREST.SINLGE_ENEMY] = new SingleEnemy(this);
        m_currentInterest = INTEREST.WIDE_ANGLE_FIELD;

        m_weightUpdateDelta = 0.5f;
        m_timeFromLastWeightUpdate = 0.0f;

        m_interestChangeDelta = 5.0f;
        m_timeFromLastInterestChange =0.0f;
}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_timeFromLastWeightUpdate >= m_weightUpdateDelta)
        {
            updateInterestWeights();
            
            m_timeFromLastWeightUpdate = 0;
        }
        if (m_timeFromLastInterestChange >= m_interestChangeDelta)
        {
            checkForNewInterest();
            m_timeFromLastInterestChange = 0;
        }
        if (m_interests[(int)m_currentInterest].interestUpdate() == false)
        {
            checkForNewInterest();
        }
        m_timeFromLastWeightUpdate += Time.deltaTime;
        m_timeFromLastInterestChange += Time.deltaTime;
    }


    private float precedenceMultiplier(int _index)
    {
        return ((float)INTEREST.LENGTH - (float)_index) * 0.2f;
    }
    private void updateInterestWeights()
    {
        for (int i = 0; i < m_interests.Length; ++i)
        {
            m_interests[i].recalcWeight();
        }
    }
    private void checkForNewInterest()
    {
        //used to see whether the current interest is the same after this block of code
        INTEREST previousInterest = m_currentInterest;

        float highestWeight = 0;
        //For every interest...
        for (int i = 0; i < m_interests.Length; ++i)
        {
            //retrieve pre-calculated weight * precedence multiplier
            float weight = m_interests[i].currentWeight() * precedenceMultiplier(i);

            //--debug
            debugWeights[i] = weight;
         
            //if i is not the current interest
            //AND the weight is ge highest recorded weight
           // if ((INTEREST)i != m_currentInterest && weight >= highestWeight)
            if (weight >= highestWeight)
            {
                //this should be the current interest
                m_currentInterest = (INTEREST)i;
                //update highest weight val
                highestWeight = weight;
            }
        }

        //inialise the interest (probably should have called these 'behaviours' l u l.
        if (previousInterest != m_currentInterest)
        {
            m_interests[(int)previousInterest].interestEnd();
            m_interests[(int)m_currentInterest].interestInit();
        }
        
    }
}
