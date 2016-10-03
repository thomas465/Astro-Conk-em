using UnityEngine;
using System.Collections;

public class BBCamInterests
{
    protected float m_weight = 0.0f;
    protected BillBoardCam m_cam;
    protected Camera m_camRef;
    protected float m_defualtFov;
    public BBCamInterests(BillBoardCam _cam) {m_cam = _cam; m_camRef = m_cam.gameObject.GetComponent<Camera>(); m_defualtFov = m_camRef.fieldOfView; }
    //init the interest because we're about to focus on it!
    public virtual void interestInit() { }
    //end of interest
    public virtual void interestEnd() { }
    //update the cam based on this interest , return whether we're still interested
    public virtual bool interestUpdate() { return true; }

    //0-1 modifier for how good this particular thing is right now
    public virtual float recalcWeight() { return 0; }
    //current cached interest weight
    public float currentWeight() { return m_weight; }

    //probably guna be lerping lookAt a lot so this helps out for the simplest case.
    public void lerpLookAt(Vector3 _target, float _speedMultiplier=5.0f)
    {

        //implement

        //for now just do this
        m_cam.gameObject.transform.LookAt(_target);
    }
    public void lerpFov(float _newFov, float _speedMultiplier =3.0f)
    {
        m_camRef.fieldOfView = Mathf.Lerp(m_camRef.fieldOfView, _newFov, _speedMultiplier * Time.deltaTime);
    }

    //need a lerp FOV/zoom too
}

public class CloseEnemy : BBCamInterests
{
    private EnemyManager m_enemyManager;
    private GameObject m_player;
    private Enemy m_enemy;
    private const float m_minDistForInterest = 5.0f;//want to be interested aroun 6.0f but there's a slow update on recalc weights so give room
    private float fov = 8;

    public CloseEnemy(BillBoardCam _cam) : base(_cam)
    {
        m_enemyManager = GameManager.instance.enemyManager;
        m_player = GameObject.Find("PLAYER");
    }

    public override bool interestUpdate()
    {
        //fak it just look at player! (Was enemy but it is pooled so shit happens;
        lerpLookAt(m_player.gameObject.transform.position + new Vector3(0,-2.9f,0));
        lerpFov(fov);
        return true; //!m_enemy.isDead();
    }

    public override float recalcWeight()
    {
      
        m_weight = 0.0f;
        float smallestRelDist = float.MaxValue;
        for (int i = 0; i < m_enemyManager.getNumActiveEnemies(); ++i)
        {
            m_enemy = m_enemyManager.getEnemy(i);
            float relativeDist = (m_player.transform.position - m_enemy.gameObject.transform.position).magnitude;
            if (relativeDist <= smallestRelDist)
            {
                smallestRelDist = relativeDist;
            }
        }
        if (smallestRelDist <= m_minDistForInterest)
        {
            //normalise weight value
            m_weight = 1 - (smallestRelDist * 0.5f / m_minDistForInterest);
        }
        return m_weight;
    }
}

public class BunchOfEnemies : BBCamInterests
{
    //private GroupDetector[] m_groups;
    public BunchOfEnemies(BillBoardCam _cam) : base(_cam)
    {
        //have a hitbox that counts ins/outs 
       // m_group = GameObject.Find("EnemyGroupDetector").GetComponent<EnemyGroupDetector>(); 
    }
    public override bool interestUpdate()
    {
        //watch the box
        return true;
    }

    public override float recalcWeight()
    {
        //interested if ins >= threshold
        return 0.0f;
    }
}

public class BallHit : BBCamInterests
{
    //eh, idk about this one yet :p
    public BallHit(BillBoardCam _cam) : base(_cam)
    {

    }
    public override bool interestUpdate()
    {
        return true;
    }

    public override float recalcWeight()
    {

        return m_weight;
    }
}

 public class Player : BBCamInterests
{
    private PlayerScript m_player;
    private ScoreManager m_scores;
    private const int m_minCombo = 3;//inclusive
    private const int m_upperCombo = 15;
    public Player(BillBoardCam _cam) : base(_cam)
    {
        m_player = GameObject.Find("PLAYER").GetComponent<PlayerScript>();
        m_scores = GameManager.instance.scoreManager;
    }
    public override bool interestUpdate()
    {
        lerpLookAt(m_player.transform.position);
        lerpFov(30);//idk
        return true;
    }

    public override float recalcWeight()
    {
        if (!m_scores) return 0.0f;
        if (GameManager.instance.scoreManager.getComboNo() <= m_minCombo)
        {
            m_weight = 0.0f;
        }
        else
        {
            m_weight = (m_scores.getComboNo() - m_minCombo) / m_upperCombo;
            m_weight = m_weight >= 1.0f ? 1.0f : m_weight; 
        }
        return m_weight;
    }
}
public class WideAngleField : BBCamInterests
{
    private Vector3 m_target;
    public WideAngleField(BillBoardCam _cam) : base(_cam)
    {
        m_target = GameObject.Find("WideAngleTarget").transform.position;
    }
    public override bool interestUpdate()
    {
        //lerp to m_target
        lerpLookAt(m_target);
        lerpFov(m_defualtFov);//or maybe wider
        return true;
    }

    public override float recalcWeight()
    {
        //always interested in looking centre field if nothing else is interesting!
        m_weight = 1.0f;
        return m_weight;
    }
}

public class SingleEnemy : BBCamInterests
{
    private EnemyManager m_enemyManager;
    private Enemy m_enemy;
    private float m_durationWatching=0.0f;
    private float m_minWatchTime = 2.0f;
    private float m_maxWatchTime = 6.0f;
    private float m_watchTime=0.0f;
    private bool m_watching=false;
    public SingleEnemy(BillBoardCam _cam) : base(_cam)
    {
        m_enemyManager = GameManager.instance.enemyManager;
        m_watchTime = Random.Range(m_minWatchTime, m_maxWatchTime);
    }
    public override bool interestUpdate()
    {
        //lerp to m_target
        lerpLookAt(m_enemy.gameObject.transform.position + new Vector3(0,-2,0));
        lerpFov(m_defualtFov-30.0f);
        m_durationWatching += Time.deltaTime;
        return true;
    }
    public override void interestInit()
    {
        if (m_enemyManager.getNumActiveEnemies() > 0)
        {
            int inedx = Random.Range(0, m_enemyManager.getNumActiveEnemies() - 1);
            m_enemy = m_enemyManager.getEnemy(inedx);//0 i guess cus index isn't working
            m_watchTime = Random.Range(m_minWatchTime, m_maxWatchTime);
            m_durationWatching = 0;
            m_watching = true;
        }
    }
    public override void interestEnd()
    {
        m_watching = false;
    }
    public override float recalcWeight()
    {
        m_weight = m_enemyManager.getNumActiveEnemies() > 0 ? 1.0f :0.0f;
        if (m_watching) m_weight *= (1 - (m_durationWatching/m_watchTime));
        return m_weight;
    }
}
