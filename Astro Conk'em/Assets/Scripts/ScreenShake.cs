using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake g_instance;

    //cap the magnitude of the shakes so it doesn't hurt the player's brain
    public float m_maxMagnitude = 0.65f;
    public float m_maxDuration = 0.5f;

    public float m_lerpValue = 25.0f;
    [SerializeField]
    private float m_duration;

    [SerializeField]
    private float m_currentMag;
    [SerializeField]
    private float m_currentTime =0;
    [SerializeField]
    private float m_currentLerpValue = 0;
    [SerializeField]
    private Vector3 m_startPos;
    [SerializeField]
    private Vector3 m_target;
    [SerializeField]
    private bool m_shaking = false;

    //reference to where the camera should rest - TMS
    public Transform restPos;

    // Use this for initialization
    void Start ()
    {
        g_instance = this;
        m_startPos = restPos.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ////For testing...
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    shake(0.08f, 0.1f);
        //}

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
                    m_target = restPos.position + (Random.insideUnitSphere * m_currentMag);
                    m_startPos = transform.position;
                    m_target.z = restPos.position.z;
                    //Reset lerpval
                    m_currentLerpValue = 0;
                }

                //Lerp to target
                gameObject.transform.position = Vector3.Lerp(m_startPos, m_target, m_currentLerpValue);
                //Update lerp
                m_currentLerpValue += m_lerpValue * Time.deltaTime;

            }
            else
            {
                //We're done shaking if the time is up
                m_shaking = false;
                m_currentLerpValue = 0;
                m_currentTime = 0;
                m_currentMag = 0;
                m_duration = 0;
            }

        }
        else
        {
            if (transform.position != restPos.position && TitleScript.titlePanFinished)
            {
                //Lerp to start
                //gameObject.transform.position = Vector3.Lerp(m_target, restPos.position, m_currentLerpValue);

                //This is TMS trying something out to help with the 'getting stuck' glitch - it works quite nicely about half the time but you can remove this if you like
                transform.position = Vector3.Lerp(transform.position, restPos.position, 10 * Time.deltaTime);
                //Update lerp
                m_currentLerpValue += m_lerpValue * Time.deltaTime;
            }
            else
            {   //this isn't really necessary but i'm including it for sanity checking this mysterious bug
                m_currentLerpValue = 0;
            }
        }
	}

    //screenshake is additive, so calling this multiple times'll ramp up no problemo
    //0.4f mag and 0.15f duration make small shake
    public void shake(float _magnitude = 1.0f, float _duration = 0.15f)
    {
        if (m_shaking)
        {
            m_duration += Mathf.Sqrt(_duration);
            m_currentMag += Mathf.Sqrt(_magnitude);
        }
        else
        {
            m_duration = _duration;
            m_currentMag = _magnitude;

            m_target = m_startPos;
            m_currentLerpValue = 0;
            m_currentTime = 0;

            m_shaking = true;
        }
        m_currentMag = m_currentMag > m_maxMagnitude ? m_maxMagnitude : m_currentMag;
        m_duration = m_duration > m_maxDuration ? m_maxDuration : m_duration;
    }
}
