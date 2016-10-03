using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScreenCoverScript : MonoBehaviour {

    UnityEngine.UI.Image myImg;
    public bool coverScreen = false;
    public float fadeDelay = 0.1f;

    // Use this for initialization
    void Start()
    {
        myImg = GetComponent<UnityEngine.UI.Image>();

        if(!coverScreen)
        {
            myImg.color = Color.white;
        }
    }

	
	// Update is called once per frame
	void Update () {
        Color targetColour = Color.white;

        if (!coverScreen)
            targetColour = Color.clear;

        if (fadeDelay <= 0)
        {
            myImg.color = Color.Lerp(myImg.color, targetColour, 2 * Time.deltaTime);
        }
        else
        {
            fadeDelay -= Time.deltaTime;
        }
	}
}
