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

    // Use this for initialization
    void Start()
    {
        powerbarSingleton = this;

        onScreenPos = transform.localPosition;
        offScreenPos = onScreenPos - Vector3.up * 200;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos;

        if(visible)
        {
            targetPos = onScreenPos;
        }
        else
        {
            targetPos = offScreenPos;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 8 * Time.deltaTime);


        if (cursorPauseTime <= 0)
        {
            //This is a lerped thing so that the cursor never does massive jumps
            currentBarSpeed = Mathf.Lerp(currentBarSpeed, targetBarSpeed, 2 * Time.deltaTime);
                
            currentPower = Mathf.Sin(Time.time * currentBarSpeed);

            cursor.transform.localPosition = new Vector3(currentPower * 450 / 2, 0, 0);
            cursor.transform.localScale = Vector3.Lerp(Vector3.one * 0.4f, Vector3.one * 1.45f, 1 - Mathf.Abs(currentPower));
        }
        else
        {
            cursor.transform.localScale = Vector3.Lerp(cursor.transform.localScale, Vector3.one*0.4f, Time.deltaTime * 15);
            cursorPauseTime -= Time.deltaTime;
        }

        testText.text = ""+GetCurrentPower();
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
        cursor.transform.localScale = cursor.transform.localScale * 1.5f;

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
