using UnityEngine;
using System.Collections;

public class NameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void GetInput(string input)
    {
        Debug.Log("Name: " + input);
        LocalHighScoreManager.g_instance.addPlayerScore(input, ScoreManager.score);
        LeaderBoardVisualsScript.singleton.Activate();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
