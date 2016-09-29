using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerbarScript : MonoBehaviour
{

    public static PowerbarScript powerbarSingleton;
    public Image holder, cursor;

    public float currentPower;

    public float targetBarSpeed = 4;
    public float currentBarSpeed = 0;

    float cursorPauseTime = 0;

    Vector3 onScreenPos, offScreenPos;

    // Use this for initialization
    void Start()
    {
        powerbarSingleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (cursorPauseTime <= 0)
        {
            //This is a lerped thing so that the cursor never does massive jumps
            currentBarSpeed = Mathf.Lerp(currentBarSpeed, targetBarSpeed, 2 * Time.deltaTime);
                
            currentPower = Mathf.Sin(Time.time * currentBarSpeed);

            cursor.transform.localPosition = new Vector3(currentPower * 450 / 2, 0, 0);
            cursor.transform.localScale = Vector3.Lerp(Vector3.one * 0.6f, Vector3.one * 1.35f, 1 - Mathf.Abs(currentPower));
        }
        else
        {
            cursorPauseTime -= Time.deltaTime;
        }
    }

    public void IncreaseSpeed(float speedIncrease = 0.15f)
    {
        targetBarSpeed += speedIncrease;
    }

    public void ResetSpeed()
    {
        targetBarSpeed = 0;
    }

    public float GetCurrentPower()
    {
        return 1 - Mathf.Abs(currentPower);
    }

    public void LockInCurrentPower()
    {
        cursorPauseTime = 2;
    }
}
