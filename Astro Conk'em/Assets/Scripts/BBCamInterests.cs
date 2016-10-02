using UnityEngine;
using System.Collections;

public class BBCamInterests
{
    protected float m_weight = 0.0f;
    protected BillBoardCam m_cam;

    public BBCamInterests(BillBoardCam _cam) { m_cam = _cam; }
    //update the cam based on this interest
    public virtual void interestUpdate() { }
    //0-1 modifier for how good this particular thing is right now
    public virtual float recalcWeight() { return 0; }
    //current cached interest weight
    public float currentWeight() { return m_weight; }

    //probably guna be lerping lookAt a lot so this helps out for the simplest case.
    public void lerpLookAt(Vector3 _target, float _speedMultiplier=5.0f)
    {
        m_cam.gameObject.transform.LookAt(Vector3.Lerp(m_cam.gameObject.transform.position,
                                                        _target, Time.deltaTime * _speedMultiplier));
    }
    //need a lerp FOV/zoom too
}

public class CloseEnemy : BBCamInterests
{
    private EnemyManager m_enemyManager;
    private GameObject m_player;
    private Enemy m_enemy;
    private const float m_minDistForInterest = 1.0f;
  

    public CloseEnemy(BillBoardCam _cam) : base(_cam)
    {
        m_enemyManager = GameManager.instance.enemyManager;
        m_player = GameObject.Find("PLAYER");
    }

    public override void interestUpdate()
    {
        lerpLookAt(m_enemy.gameObject.transform.position);
    }

    public override float recalcWeight()
    {
        //@@actually would be better to just have a hitbox to check
        //@@and the closer it is to the player the higher the weight.

        m_weight = 0.0f;
        float smallestRelDist = float.MaxValue;
        for (int i = 0; i < m_enemyManager.getNumActiveEnemies(); ++i)
        {
            m_enemy = m_enemyManager.getEnemy(i);
            float relativeDist = (m_player.transform.position - m_enemy.gameObject.transform.position).sqrMagnitude;
            if (relativeDist <= smallestRelDist)
            {
                smallestRelDist = relativeDist;
            }
        }
        if (smallestRelDist <= m_minDistForInterest)
        {
            m_weight = 1.0f;
        }
        return m_weight;
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
    public override void interestUpdate()
    {

    }

    public override float recalcWeight()
    {

        return m_weight;
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

    public override float recalcWeight()
    {
        //GameManager.instance.scoreManager.combo >= m_minCombo;
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
    public override void interestUpdate()
    {
        //lerp to m_target
    }

    public override float recalcWeight()
    {
        //always interested in looking centre field if nothing else is interesting!
        return m_weight;
    }
}

