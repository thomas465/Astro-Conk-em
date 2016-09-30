using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript playerSingleton;

    public Image reticle;
    Animator anim;
    public Rigidbody ballTest;
    public Camera cam;

    public ReticleScript reticleScript;

    public bool aimAssist = false;

    public Transform ballSpawnPos;

    //This is used to check if the player has "flicked" the stick
    float prevStickMagnitude = 0;
    Vector3 prevStickDir;

    //This is to allow the reticule to stay still as the player is swinging
    float swingDelay = 0;

    Vector3 swingAngle;
    Vector3 reticleRestPos;

    // Use this for initialization
    void Start()
    {
        playerSingleton = this;
        anim = GetComponent<Animator>();


        reticleRestPos = new Vector3(0, -250, 0);
        reticle.transform.localPosition = reticleRestPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (swingDelay <= 0)
        {
            if (TitleScript.titlePanFinished)
            {
                Aiming();
            }
        }
        else
        {
            swingDelay -= Time.deltaTime;
        }
    }

    public void GiveSwingDelay(float amount = 2)
    {
        swingDelay = amount;
    }

    void Aiming()
    {
        Vector3 curStickDir = new Vector3(-Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical"), 0);

        curStickDir.y = Mathf.Clamp(curStickDir.y, 0, 1);

        float stickMagnitude = curStickDir.magnitude;

        if (stickMagnitude > 1)
        {
            curStickDir = curStickDir.normalized;
            stickMagnitude = 1;
        }

        if (stickMagnitude < prevStickMagnitude - 0.2f && prevStickMagnitude > 0.1f)
        {
            Swing(prevStickDir, reticle.transform.position);
        }
        else
        {
            Vector3 reticleTargetPos = Vector3.zero + curStickDir * 400;

            reticle.transform.localPosition = Vector3.Lerp(reticle.transform.localPosition, reticleTargetPos + reticleRestPos, 15 * Time.deltaTime);

            Vector3 playerRotation = Vector3.forward;
            playerRotation.x += curStickDir.x;
            playerRotation.Normalize();

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerRotation), 3 * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation((cam.transform.position + Vector3.forward + curStickDir / 5) - cam.transform.position), 15 * Time.deltaTime);
            prevStickMagnitude = stickMagnitude;
            prevStickDir = curStickDir;
        }
    }

    void Swing(Vector3 newSwingAngle, Vector3 reticlePos)
    {
        //ballTest.transform.position = ballSpawnPos.position;
        //ballTest.velocity = Vector3.zero;

        RaycastHit swingCastHit;
        Ray swingCastRay = cam.ScreenPointToRay(reticle.transform.position);

        Debug.DrawLine(swingCastRay.origin, swingCastRay.origin + swingCastRay.direction * 250, Color.magenta, 10);

        if (Physics.SphereCast(swingCastRay, 0.31f, out swingCastHit, 250))
        {
            Debug.Log("Aimed swing");

            //Auto-aim
            if (swingCastHit.collider.gameObject.layer == 8 && aimAssist)
            {
                swingCastHit.point = swingCastHit.collider.transform.position;
            }

            Debug.DrawLine(swingCastRay.origin, swingCastHit.point, Color.green, 10);
            reticle.transform.position = cam.WorldToScreenPoint(swingCastHit.point);
            swingAngle = (swingCastHit.point - BallSpawner.currentBall.transform.position).normalized;
        }
        else
        {
            Debug.Log("Random swing");
            swingAngle = newSwingAngle;
        }


        reticleScript.Fire();

        prevStickMagnitude = 0;
        swingDelay = 5;

        anim.SetTrigger("Swing");
        BallSpawner.currentBall.ResetParticles();

        PowerbarScript.powerbarSingleton.LockInCurrentPower();
        //Debug.Break();
    }

    void HitBall()
    {
        swingDelay = 0.25f;

        if (BallSpawner.currentBall.getState() == BallScript.BALL_STATE.READY_FOR_PLAYER_HIT)
        {
            BallSpawner.currentBall.HitByPlayer(PowerbarScript.powerbarSingleton.GetCurrentPower(), swingAngle);
            CameraScript.cameraSingleton.HitBall();
        }
    }

    public void Die()
    {
        Debug.Log("Player is dead!");
    }
}
