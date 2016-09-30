using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public PlayerHealth playerHealth;

    Animator anim;

    //float to restart the game
    float restart;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
   
	}
	
	// Update is called once per frame
	void Update () {
        if(playerHealth.m_currentPlayerHealth <= 0)
        {
            anim.SetTrigger("GameOver");

            restart += Time.deltaTime;
            if (restart >= 5)
            {
                //Application.LoadLevel(Application.loadedLevel);
                //pherhaps a dodgey way of reloading the scene?
            }
        }

	}
}
