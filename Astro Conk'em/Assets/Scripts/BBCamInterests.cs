using UnityEngine;
using System.Collections;

public class BBCamInterests
{
    protected BillBoardCam m_cam;
    public BBCamInterests(BillBoardCam _cam) { m_cam = _cam; }
    public virtual void interestUpdate() { }
    public virtual bool interested() { return false; }//COOULD implement a sort of interest weight instead of flat out yes/no. get this done if everything is working

    public void lerpLookAt(Vector3 _target, float _speedMultiplier=5.0f)
    {
        m_cam.gameObject.transform.LookAt(Vector3.Lerp(m_cam.gameObject.transform.position,
                                                        _target, Time.deltaTime * _speedMultiplier));
    }
}

public class CloseEnemy : BBCamInterests
{
    private EnemyManager m_enemyManager;
    private GameObject m_player;
    private int m_enemyIndex;
    private const float m_minDistForInterest = 1.0f;
  

    public CloseEnemy(BillBoardCam _cam) : base(_cam)
    {
        m_enemyManager = GameManager.instance.enemyManager;
        m_player = GameObject.Find("PLAYER");
        m_enemyIndex = 0;
    }

    public override void interestUpdate()
    {
        Vector3 targetPos = m_enemyManager.getEnemy(m_enemyIndex).gameObject.transform.position;
        lerpLookAt(targetPos);
    }

    public override bool interested()
    {
        float greatestRelativeDist=0;
        m_enemyIndex = 0;

        for (int i = 0; i < m_enemyManager.getNumActiveEnemies(); ++i)
        {
            float relativeDist = (m_player.transform.position - m_enemyManager.getEnemy(i).gameObject.transform.position).sqrMagnitude;
            if (relativeDist <= greatestRelativeDist)
            {
                greatestRelativeDist = relativeDist;
                m_enemyIndex = i;
            }
        }
        if (m_enemyIndex <= m_minDistForInterest)
        {
            return true;
        }
        return false;
    }
}

public class BunchOfEnemies : BBCamInterests
{
    public BunchOfEnemies(BillBoardCam _cam) : base(_cam)
    {
        //have a hitbox that counts ins/outs 
    }
    public override void interestUpdate()
    {
        //watch the box
    }

    public override bool interested()
    {
        //interested if ins >= threshold
        return false;
    }
}

public class BallHit : BBCamInterests
{
    //eh, idk about this one yet :p
    public BallHit(BillBoardCam _cam) : base(_cam)
    {

    }
    public override void interestUpdate()
    {

    }

    public override bool interested()
    {

        return false;
    }
}

 public class Player : BBCamInterests
{
    private PlayerScript m_player;
    private const int m_minCombo = 3;
    public Player(BillBoardCam _cam) : base(_cam)
    {
        m_player = GameObject.Find("PLAYER").GetComponent<PlayerScript>();
    }
    public override void interestUpdate()
    {

    }

    public override bool interested()
    {
        return GameManager.instance.scoreManager.combo >= m_minCombo;
    }
}
public class WideAngleField : BBCamInterests
{
    private Vector3 m_target;
    public WideAngleField(BillBoardCam _cam) : base(_cam)
    {
        m_target = GameObject.Find("WideAngleTarget").transform.position;
    }
    public override void interestUpdate()
    {
        //lerp to m_target
    }

    public override bool interested()
    {
        //always interested in looking centre field if nothing else is interesting!
        return true;
    }
}

