using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

    public static ScoreManager scoreSingleton;
    public static int score;
    
	[SerializeField]
	private Text comboDisp;

	[SerializeField]
	private int scoreBase = 25; //the scorebase will add itself to the scorebonus upon each consecutive hit
	private static int scorebonus; //the scorebonus will add itself to the score amount whilst in a combo
	private static int continualHits;
	private Text text;
	private bool alphaUp;

    void Awake ()
    {
        scoreSingleton = this;

        text = GetComponent<Text>();

        //set the initial score to zero
        score = 0;
		continualHits = 0;
	}

	void Update ()
    {
		//sorry, I know this is filth, but it is a workaround
		if (!alphaUp && comboDisp)
		{
			if (text.color.a != 1)
			{
				Color newCol = text.color;
				comboDisp.color = newCol;
			}

			if (comboDisp.color.a == 1)
			{
				alphaUp = false;
			}
		}
		text.text = "Score: " + score;
		if (comboDisp)
		{
			comboDisp.text = "Combo: " + continualHits;
		}

       // text.text = "Score: " + score;
        /*
        score value to be assigned in the enemy manager for different scoring enemies? 
        
        code to use if so ^
        */

        //NOTE:    the collision will be between the ball and the enemy, so it might be easier for the enemies to 
        //         update the scores instead of the enemy manager? kinda dirty but it will work and this is a jam
	}

	public void BallMissed()
	{
		continualHits = 0;
		scorebonus = 0;
	}

	public void BallHit()
	{
		continualHits++;
	}

	public int getComboNo ()
	{
		return continualHits;
	}

    public void AddScore(int amount)
    {
		if (continualHits > 1)
		{
			scorebonus += scoreBase;

			amount += scorebonus;
		}

        ScoreManager.score += amount;
    }
}
