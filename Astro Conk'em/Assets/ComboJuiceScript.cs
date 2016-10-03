using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboJuiceScript : MonoBehaviour {

    public Text myText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        myText.transform.localScale = Vector3.Lerp(myText.transform.localScale, Vector3.one * (1+(ScoreManager.scoreSingleton.getComboNo()*0.1f)), 8 * Time.deltaTime);
	}

    public void UpdateComboDisplay(int increase)
    {
        myText.transform.localScale *= 1.25f;

    }

    public void ResetCombo()
    {

    }
}
