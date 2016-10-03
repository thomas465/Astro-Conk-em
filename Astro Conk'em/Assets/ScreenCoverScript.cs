using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScreenCoverScript : MonoBehaviour
{

    public UnityEngine.UI.Image myImg;
    public bool coverScreen = false;
    public float fadeDelay = 0.1f;

    // Use this for initialization
    void Awake()
    {
        myImg = GetComponent<UnityEngine.UI.Image>();

        if (!coverScreen && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            myImg.color = Color.white;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Color targetColour = Color.white;

        if (!coverScreen)
            targetColour = Color.clear;

        if (fadeDelay <= 0)
        {
            myImg.color = Color.Lerp(myImg.color, targetColour, 2 * Time.deltaTime);

            if (coverScreen && myImg.color.a > 0.98f)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                else
                    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
        }
        else
        {
            fadeDelay -= Time.deltaTime;
        }
    }
}
