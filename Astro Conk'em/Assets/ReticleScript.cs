using UnityEngine;
using System.Collections;

public class ReticleScript : MonoBehaviour
{
    public UnityEngine.UI.Image[] myImages;

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

        myImages[0].color = Color.Lerp(myImages[0].color, targetColour, 10 * Time.deltaTime);


        for(int i=1; i<myImages.Length; i++)
        {
            myImages[i].color = myImages[0].color;
        }
    }

    public void Fire()
    {
        transform.localScale = Vector3.one * 2;
    }
}
