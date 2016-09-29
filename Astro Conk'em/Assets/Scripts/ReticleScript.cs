using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{
    public UnityEngine.UI.Image[] myImages;

    //Most of the reticle funcitonality is actually handed in the PLAYER script; sorry it's a bit confusing


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 16 * Time.deltaTime);

        bool visible = transform.localPosition.y > -240;
        Color targetColour;

        if(visible)
        {
            targetColour = Color.white;
        }
        else
        {
            targetColour = Color.clear;
        }

        //Control visibility of the power bar
        PowerbarScript.powerbarSingleton.SetVisible(visible);

        myImages[0].color = Color.Lerp(myImages[0].color, targetColour, 10 * Time.deltaTime);


        for(int i=1; i<myImages.Length; i++)
        {
            myImages[i].color = myImages[0].color;
        }

        myImages[1].transform.Rotate(transform.forward * (10*Mathf.Sin(Time.timeSinceLevelLoad)));
        myImages[2].transform.localScale = Vector3.one * (1.1f + (Mathf.Sin(Time.timeSinceLevelLoad * 12)) * 0.4f);
    }

    public void Fire()
    {
        transform.localScale = Vector3.one * 2.5f;
    }
}
