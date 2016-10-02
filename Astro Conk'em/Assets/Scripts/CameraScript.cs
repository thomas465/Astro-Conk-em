using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public static CameraScript cameraSingleton;

	[SerializeField]
	private float maxFOV = 75.0f;
	private float targetFOV;
    
	private Transform watchingThis;
    private Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        cameraSingleton = this;
        targetFOV = cam.fov;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the player is in meleeMode
		if (PlayerScript.playerSingleton.meleeMode && cam.fieldOfView < maxFOV)
		{
			cam.fieldOfView = FOVout (cam.fieldOfView);
		}
		else if (!PlayerScript.playerSingleton.meleeMode && cam.fieldOfView > targetFOV)
		{
			cam.fieldOfView = FOVin (cam.fieldOfView);
		}
        //cam.fov = Mathf.Lerp(cam.fov, targetFOV, 3 * Time.deltaTime);
	}

	//go up to max FOV
	private float FOVout (float _currentFOV)
	{
		float FOV = _currentFOV;

		FOV += ((maxFOV - targetFOV) / 25);

		if (FOV > maxFOV)
		{
			FOV = maxFOV;
		}

		return FOV;
	}

	//go down to resting FOV
	private float FOVin (float _currentFOV)
	{
		float FOV = _currentFOV;

		FOV -= ((maxFOV - targetFOV) / 50);

		if (FOV > maxFOV)
		{
			FOV = maxFOV;
		}

		return FOV;
	}

    public void WatchThis(Transform target)
    {

    }

    public void HitBall()
    {
        //cam.fov = targetFOV * 0.f;
    }
}
