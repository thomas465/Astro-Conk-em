using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{
	public static PlayerScript playerSingleton;

	private float hitShakeMultipler = 0.18f;
	public Image reticle;
	Animator anim;
	public Rigidbody ballTest;
	public Camera cam;

	public ReticleScript reticleScript;

	public bool aimAssist = false;

	public Transform ballSpawnPos;

	public float aimScale = 0.7f;

	//This is used to check if the player has "flicked" the stick
	float prevStickMagnitude = 0;
	Vector3 prevStickDir;

	//This is to allow the reticule to stay still as the player is swinging
	float swingDelay = 0;

	Vector3 swingAngle;
	Vector3 reticleRestPos;

	//Melee
	public bool meleeMode = false;
	
	AudioSource myAudio;

	// Use this for initialization
	void Start()
	{
		playerSingleton = this;
		anim = GetComponent<Animator>();


		reticleRestPos = new Vector3(0, -250, 0);
		reticle.transform.localPosition = reticleRestPos;

		myAudio = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		if(swingDelay <= 0 && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Busy"))
		{
			if(TitleScript.titlePanFinished)
			{
				Aiming();
			}
		}
		else
		{
			swingDelay -= Time.deltaTime;
		}

		anim.SetBool("MeleeMode", meleeMode);

        float swingSpeed = Mathf.Clamp(PowerbarScript.powerbarSingleton.GetCurrentPower(), 0.55f, 1.1f);
        anim.SetFloat("Power", swingSpeed);
	}
	
	public void GiveSwingDelay(float amount = 2)
	{
		swingDelay = amount;
	}

	void Aiming()
	{
		Vector3 curStickDir = new Vector3(-Input.GetAxisRaw("Horizontal"), -Input.GetAxisRaw("Vertical"), 0);

		if(meleeMode)
		{
			curStickDir.x = -curStickDir.x;

			if(curStickDir.y > 0.5f || (curStickDir.y==0 && prevStickMagnitude<0.1f))
				meleeMode = false;
		}
		else
		{
			if(curStickDir.y < -0.25f)
				meleeMode = true;
			else
				curStickDir.y = Mathf.Clamp(curStickDir.y, 0, 1);
		}

		//
		float stickMagnitude = curStickDir.magnitude;

		if(stickMagnitude > 1)
		{
			curStickDir = curStickDir.normalized;
			stickMagnitude = 1;
		}

		if(stickMagnitude < prevStickMagnitude - 0.2f && prevStickMagnitude > 0.1f)
		{
			if(!meleeMode)
				Swing(prevStickDir, reticle.transform.position);
			else
				MeleeSwing();
		}
		else
		{
			Vector3 reticleTargetPos = Vector3.zero + curStickDir * (400 * aimScale);

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

	void MeleeSwing()
	{
		meleeMode = true;
		anim.SetBool("MeleeMode", true);
		anim.SetTrigger("Swing");
		swingDelay = 0.5f;
		prevStickMagnitude = 0;
		//Debug.Break();
	}

    public void TakeHit(int damage)
    {
        PlayerHealth.healthSingleton.TakeDamage(damage);
        anim.SetTrigger("Hit");
        swingDelay = 0.35f;
    }

	void HitFloor()
	{
		myAudio.PlayOneShot(SoundBank.sndBnk.hitFloorWithBat);
		meleeMode = false;
		ScreenShake.g_instance.shake(0.1f, 0.06f);


		MeleeZoneMarker zone = GetComponentInChildren<MeleeZoneMarker>();
		Vector3 start = zone.transform.position;

		Debug.DrawRay(start, Vector3.down * 5f, Color.red, 3f);

		RaycastHit[] slugs = Physics.SphereCastAll(start, 0.5f, Vector3.down, 15f, LayerMask.GetMask("MeleeZone") + LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Collide);

        
		//Debug.Log(slugs.Length + ", " + LayerMask.GetMask("MeleeZone"));

		foreach(RaycastHit hit in slugs)
		{
			Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
			//if(enemy == null)
			//{
				//continue;
			//}

			//Debug.Log("Hit enemy");

            if(enemy)
			    enemy.TakeDamage(false, transform.forward);
		}

        //let the score manager know that the player did a melee attack and therefore loses their combo
        ScoreManager.scoreSingleton.BallMissed();
    }

	void Swing(Vector3 newSwingAngle, Vector3 reticlePos)
	{
		//ballTest.transform.position = ballSpawnPos.position;
		//ballTest.velocity = Vector3.zero;

		RaycastHit swingCastHit;
		Ray swingCastRay = cam.ScreenPointToRay(reticle.transform.position);

		Debug.DrawLine(swingCastRay.origin, swingCastRay.origin + swingCastRay.direction * 250, Color.magenta, 10);

		if(Physics.SphereCast(swingCastRay, 0.05f, out swingCastHit, 250)) //in the case of a hit
		{
            RaycastHit lockOn;

            if (Physics.SphereCast(swingCastRay, 20f, out lockOn, 250, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore))
            {
                //Auto-aim
                if (swingCastHit.collider.gameObject.tag == Tags.Enemy && aimAssist)
                {
                    Debug.Log("Aimed swing");
                    swingCastHit.point = swingCastHit.collider.transform.position + Vector3.up / 2;
                    reticleScript.LockOn();
                }
            }

			Debug.DrawLine(swingCastRay.origin, swingCastHit.point, Color.green, 10);
			reticle.transform.position = cam.WorldToScreenPoint(swingCastHit.point);
			swingAngle = (swingCastHit.point - BallSpawner.currentBall.transform.position).normalized;
		}
		else //in the case of a miss
		{
			//let the score manager know that a miss occured
			ScoreManager.scoreSingleton.BallMissed();

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

		if(BallSpawner.currentBall.getState() == BallScript.BALL_STATE.READY_FOR_PLAYER_HIT)
		{
			BallSpawner.currentBall.HitByPlayer(PowerbarScript.powerbarSingleton.GetCurrentPower(), swingAngle);
			CameraScript.cameraSingleton.HitBall();
			ScreenShake.g_instance.shake(PowerbarScript.powerbarSingleton.GetCurrentPower() * hitShakeMultipler, PowerbarScript.powerbarSingleton.GetCurrentPower() * 0.1f);
		}
	}

	public void Die()
	{
		Debug.Log("Player is dead!");
        anim.SetBool("Dead", true);
        swingDelay = 9999;
        TitleScript.titlePanFinished = false;
        PowerbarScript.powerbarSingleton.SetVisible(false);
	}
}
