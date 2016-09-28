using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    public Image reticle;
    public Animator anim;

    public Transform cam;

    float prevStickMagnitude = 0;

    Vector3 swingAngle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Aiming();
    }

    void Aiming()
    {
        Vector3 curStickDir = new Vector3(-Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical"), 0);
        float stickMagnitude = curStickDir.magnitude;

        if (stickMagnitude > 1)
        {
            curStickDir = curStickDir.normalized;
            stickMagnitude = 1;
        }

        Vector3 reticleTargetPos = Vector3.zero + curStickDir * 300;
        reticle.transform.localPosition = Vector3.Lerp(reticle.transform.localPosition, reticleTargetPos, 15 * Time.deltaTime);

        cam.rotation = Quaternion.Lerp(cam.rotation, Quaternion.LookRotation((cam.transform.position + transform.forward + curStickDir / 5) - cam.transform.position), 15 * Time.deltaTime);


        if (stickMagnitude == 0 && prevStickMagnitude > 0.25f)
            Swing(curStickDir);


        prevStickMagnitude = stickMagnitude;
    }

    void Swing(Vector3 newSwingAngle)
    {
        swingAngle = newSwingAngle;
        Debug.Log("SWING");
    }
}
