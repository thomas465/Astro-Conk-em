using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum STATE
    {
        start,
        game,
        gameover
    };

    [SerializeField]
    private float m_score = 0;

    public static GameManager g_GameManager;
    public delegate void Fnct();
    public struct TransitionFuncts
    {
        public TransitionFuncts(Fnct _init, Fnct _shutdown)
        {
            init = _init;
            shutdown = _shutdown;
        }
        public Fnct init;
        public Fnct shutdown;
    }Dictionary<int, TransitionFuncts> m_states;
    public int m_currentState;

    public void registerState(int _index, Fnct _init, Fnct _shutdown)
    {
        TransitionFuncts fncts = new TransitionFuncts(_init, _shutdown);
        m_states.Add(_index, fncts);
    }

    public void changeState(int index)
    {
        //call init funct
        m_states[m_currentState].shutdown();
        m_currentState = index;
        m_states[index].init();
    }
    // Use this for initialization
    void Start ()
    {
        g_GameManager = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
    
    }

}
