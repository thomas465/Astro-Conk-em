using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    public int combo;
    public int score;
    Text text;

    void Start () {
        text = GetComponent<Text>();
        
	}
	void Update () {
        text.text = "Score " + score;
        /*
        score value to be assigned in the enemy manager for different scoring enemies? 
        ScoreManager.score += score;
        code to use if so ^
        */

        //NOTE:    the collision will be between the ball and the enemy, so it might be easier for the enemies to 
        //         update the scores instead of the enemy manager? kinda dirty but it will work and this is a jam
	}
}
