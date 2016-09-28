using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

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
	
	}
}
