using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardSet : MonoBehaviour {

    public static ScoreboardSet scoreSetSingleton;
    public InputField enterName;

    // Use this for initialization
    void Start()
    {
        
    }

    public void playerLeaderboard()
    {
        //will the player get onto the leaderboard
        bool getNameForScoreboard = LocalHighScoreManager.g_instance.scoreIsHighestScore(ScoreManager.score);

        if(getNameForScoreboard)
        {
            //input name
            if (enterName.text != string.Empty)
            {
                enterName.text = string.Empty;
            }
            LocalHighScoreManager.g_instance.addPlayerScore(new LocalHighScoreManager.Pair(ScoreManager.score, enterName.text));
        }

        Scoreboard();

    }

    private void Scoreboard()
    {
        for(int i = 0; LocalHighScoreManager.g_instance.numberOfScores() < 2; i++)
        {
            //ScoreboardSet[i].enterName.text = LocalHighScoreManager.g_instance.getScore(i).name;
            //ScoreboardSet[i].score = LocalHighScoreManager.g_instance.getScore(i).score;
        }
    }
}
