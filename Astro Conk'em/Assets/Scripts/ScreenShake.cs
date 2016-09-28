using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake screenShake;

    [SerializeField]
    public float m_lerpValue = 0.4f;
    public float m_duration;
    private float m_currentTime =0;
    public float m_initialMag = 1.0f;
    [SerializeField]
    private float m_currentMag;

    private float m_currentLerpValue = 0;
    private Vector3 m_startPos;
    private Vector3 m_target;
    private bool m_shaking = false;
    // Use this for initialization
    void Start ()
    {
        screenShake = this;
        m_startPos = gameObject.transform.position;

    }
	
	// Update is called once per frame
	void Update ()
    {
        //For testing...
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shake(1.5f);
        }

        //If we are shaking...
        if (m_shaking)
        {
            //If have not gone over alloted time
            if (m_currentTime < m_duration)
            {
                //Update timer
                m_currentTime += Time.deltaTime;

                //If we are at our target position...
                if (gameObject.transform.position == m_target)
                {
                    //Get new target
                    m_target = m_startPos + (Random.insideUnitSphere * m_currentMag);
                    m_target.z = m_startPos.z;
                    //Reset lerpval
                    m_currentLerpValue = 0;
                }

                //Lerp to target
                gameObject.transform.position = Vector3.Lerp(m_startPos, m_target, m_currentLerpValue);
                //Update lerp
                m_currentLerpValue += m_lerpValue;

            }
            else
            {
                //We're done shaking if the time is up
                m_shaking = false;
                m_currentLerpValue = 0;
                m_currentTime = 0;
            }

        }
        else
        {
            if (m_target != m_startPos)
            {
                //Lerp to start
                gameObject.transform.position = Vector3.Lerp(m_target, m_startPos, m_currentLerpValue);
                //Update lerp
                m_currentLerpValue += m_lerpValue;
            }

        }
	}

    void shake(float _magnitude = 1.0f)
    {
        if (m_shaking)
        {
            m_currentMag *= _magnitude;
        }
        else
        {
            m_shaking = true;
           
            m_currentMag = 1;
            m_target = m_startPos; //initate shake by setting these equal
            m_currentLerpValue = 0;
            m_currentMag = m_initialMag;
        }
        m_currentTime = 0;
    }
}
