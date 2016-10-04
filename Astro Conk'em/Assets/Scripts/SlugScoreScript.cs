using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlugScoreScript : MonoBehaviour {
    public static SlugScoreScript slugscoresingleton;

    public float speed;
    public Vector3 direction;
    public RectTransform RectTransform;
    //used to set the alpha
    private float fade = 1.0f;

    Text slugText;

    Vector3 targetScale;

	// Use this for initialization
	void Awake () {
        slugText = GetComponent<Text>();
        targetScale = transform.localScale;

        transform.localScale *= 2;
	}

    public void SetTextAmount(int amount)
    {
        slugText.text = "+" + amount;
    }
	
	// Update is called once per frame
	void Update () {

        //get the bonus amount from the other script
        //GetTextAmount();

        //move the text upwards gradually 

        if (transform.localScale.x < targetScale.x * 1.3f)
        {
            RectTransform.position -= Vector3.up * Time.deltaTime * speed;
            //finally gradually decrease the alpha
            slugText.color -= new Color(0, 0, 0, fade * Time.deltaTime);
        }



        //scaling juice
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 12 * Time.deltaTime);

        if (slugText.color.a <= 0.05f)
            Destroy(gameObject);
    }
}
