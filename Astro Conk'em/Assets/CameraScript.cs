using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public static CameraScript cameraSingleton;

    Transform watchingThis;
    float targetFOV;
    Camera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        cameraSingleton = this;
        targetFOV = cam.fov;
	}
	
	// Update is called once per frame
	void Update () {
        //cam.fov = Mathf.Lerp(cam.fov, targetFOV, 3 * Time.deltaTime);
	}

    public void WatchThis(Transform target)
    {

    }

    public void HitBall()
    {
        //cam.fov = targetFOV * 0.f;
    }
}
