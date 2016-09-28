using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public  Rigidbody m_rigidBody;  //public to provide an easy-to-reach reference, please don't go reassigning this! This is a jam, jam is messy.
    [SerializeField]
    private bool m_isActive = false;    
    private MeshRenderer m_renderer;
    private SphereCollider m_collider;
    private float m_timer =0.0f;
    [SerializeField]
    private float m_despawnTime = 5.0f; //low for now for testing

	// Use this for initialization
	void Start ()
    {
        //Cache component references
        m_rigidBody = GetComponent<Rigidbody>();
        m_renderer = GetComponent<MeshRenderer>();
        m_collider = GetComponent<SphereCollider>();

        //Set component initial state 
        m_rigidBody.isKinematic = true;
        m_renderer.enabled = false;
        m_collider.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_isActive)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= m_despawnTime)
            {
                m_timer = 0;
                setActive(false);
            }
        }
	}

    public bool isActive() { return m_isActive; }

    public void setActive(bool _active)
    {
        //If there has been a change...
        if (m_isActive != _active)
        {
            m_rigidBody.isKinematic = !m_rigidBody.isKinematic;
            m_renderer.enabled = !m_renderer.enabled;
            m_collider.enabled = !m_collider.enabled;
        }

        m_isActive = _active;
    }
}
