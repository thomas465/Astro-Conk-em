using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerbarScript : MonoBehaviour
{

    public static PowerbarScript powerbarSingleton;
    public Image holder, cursor;
    public Text testText;

    public float currentPower;

    public float targetBarSpeed = 4;
    public float currentBarSpeed = 0;

    //This number dictates how hard it is get a critical hit
    float critThreshold = 0.85f;

    public bool isCrit = false;

    float cursorPauseTime = 0;

    Vector3 onScreenPos, offScreenPos;

    bool visible = false;


    Outline myOutline;
    Color outlineCritColour;

    AudioSource myAudio;

    // Use this for initialization
    void Start()
    {
        powerbarSingleton = this;

        onScreenPos = transform.localPosition;
        offScreenPos = onScreenPos - Vector3.up * 200;

        myOutline = GetComponent<Outline>();
        outlineCritColour = myOutline.effectColor;

        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos;

        if(visible)
        {
            targetPos = onScreenPos;
            myAudio.volume = Mathf.Lerp(myAudio.volume, 0.45f, 8 * Time.deltaTime);
        }
        else
        {
            targetPos = offScreenPos;
            myAudio.volume = Mathf.Lerp(myAudio.volume, 0, 8 * Time.deltaTime);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 8 * Time.deltaTime);

        if (cursorPauseTime <= 0)
        {
            //This is a lerped thing so that the cursor never does massive jumps
            currentBarSpeed = Mathf.Lerp(currentBarSpeed, targetBarSpeed, 2 * Time.deltaTime);
                
            currentPower = Mathf.Sin(Time.time * currentBarSpeed);

            cursor.transform.localPosition = new Vector3(currentPower * 600 / 2, 0, 0);
            cursor.transform.localScale = Vector3.Lerp(Vector3.one * 0.4f, Vector3.one * 1.45f, GetCurrentPower());

            if (GetCurrentPower() > critThreshold)
                transform.localScale = Vector3.one * 1.05f;
        }
        else
        {
            cursor.transform.localScale = Vector3.Lerp(cursor.transform.localScale, Vector3.one*1f, Time.deltaTime * 15);
            cursorPauseTime -= Time.deltaTime;
        }


        myOutline.effectColor = Color.Lerp(Color.clear, outlineCritColour, GetCurrentPower());
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 15);
        testText.text = ""+GetCurrentPower();

        myAudio.pitch = GetCurrentPower() + 0.6f;
        
    }

    public void IncreaseSpeed(float speedIncrease = 0.15f)
    {
        targetBarSpeed += speedIncrease;
    }

    public void ResetSpeed()
    {
        targetBarSpeed = 0;
    }

    public void SetVisible(bool newVis)
    {
        visible = newVis;

        if (!TitleScript.titlePanFinished)
            visible = false;
    }

    public float GetCurrentPower()
    {
        return 1 - Mathf.Abs(currentPower);
    }

    public void LockInCurrentPower()
    {
        cursorPauseTime = 1;
        cursor.transform.localScale = new Vector3(1, 2, 1);

        if(GetCurrentPower()>critThreshold)
        {
            //Crit!
            isCrit = true;
        }
        else
        {
            isCrit = false;
        }
    }
}
