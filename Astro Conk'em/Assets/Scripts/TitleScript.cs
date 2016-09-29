using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScript : MonoBehaviour {

    public Text astro, conkem, prompt;

	// Use this for initialization
	void Start () {
        GameManager.g_GameManager.registerState((int)GameManager.STATE.title, StartTitle, EndTitle);
	}

    void StartTitle()
    {

    }

    void EndTitle()
    {

    }
	
	// Update is called once per frame
	void Update () {
        conkem.transform.localScale = Vector3.one * (1 + (Mathf.Sin(Time.timeSinceLevelLoad*3))*0.1f);

        prompt.transform.Rotate(Vector3.up * Time.deltaTime);
	}
}
