using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScript : MonoBehaviour {

    public Text astro, conkem, prompt;

    bool active = true;

    Vector3 conkemOffscreenPos, astroOffscreenPos;
    Vector3 conkemOnscreenPos, astroOnscreenPos;
    Vector3 promptPos;


    public Camera myCam;
    public Transform gameCamPos;

    float panTime = 0;

    public static bool titlePanFinished = false;

    // Use this for initialization
    void Start() {
        GameManager.instance.registerState((int)GameManager.STATE.start, StartTitle, EndTitle);

        conkemOffscreenPos = conkem.transform.localPosition + Vector3.up * 300;
        astroOffscreenPos = astro.transform.localPosition + Vector3.up * 380;

        conkemOnscreenPos = conkem.transform.localPosition;
        astroOnscreenPos = astro.transform.localPosition;

        promptPos = prompt.transform.localPosition;
    }

    void StartTitle()
    {
        active = true;
        myCam.GetComponent<Animator>().Play("TitleAnimation");
        titlePanFinished = false;
    }

    void EndTitle()
    {
        float maxPanTime = 1.5f;

        active = false;
        PlayerScript.playerSingleton.GiveSwingDelay(maxPanTime);
        panTime = maxPanTime;
        myCam.GetComponent<Animator>().Stop();

        GameManager.globalSoundSource.PlayOneShot(SoundBank.sndBnk.menuClick);
        GameManager.instance.musicManager.StopSong();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (active)
        {
            conkem.transform.localScale = Vector3.one * (1 + (Mathf.Sin(Time.timeSinceLevelLoad * 3)) * 0.04f);
            prompt.transform.localPosition = promptPos + Vector3.up * (Mathf.Sin(Time.timeSinceLevelLoad*4)) * 10;

            if(Input.GetButtonDown("Fire1"))
            {
                //GameManager.g_GameManager.changeState((int)GameManager.STATE.game);
                EndTitle();
            }
        }
        else
        {
            conkem.transform.localPosition = Vector3.Lerp(conkem.transform.localPosition, conkemOffscreenPos, 4 * Time.deltaTime);
            astro.transform.localPosition = Vector3.Lerp(astro.transform.localPosition, astroOffscreenPos, 4 * Time.deltaTime);

            prompt.transform.localScale = Vector3.Lerp(prompt.transform.localScale, Vector3.zero, 15 * Time.deltaTime);

            if (panTime > 0)
            {
                myCam.transform.position = Vector3.Lerp(myCam.transform.position, gameCamPos.transform.position, 4 * Time.deltaTime);
                myCam.transform.rotation = Quaternion.Lerp(myCam.transform.rotation, gameCamPos.transform.rotation, 6f * Time.deltaTime);
                panTime -= Time.deltaTime;
            }
            else
            {
                //Debug.Break();
                if(!titlePanFinished)
                {
                    GameManager.instance.musicManager.PlaySong(1);
                    titlePanFinished = true;
                }
                
            }
        }
    }
}
