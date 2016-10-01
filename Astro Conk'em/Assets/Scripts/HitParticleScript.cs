using UnityEngine;
using System.Collections;

public class HitParticleScript : MonoBehaviour {

    Light myLight;

    public ParticleSystem directionalOne;
    public Vector3 myDir;

	// Use this for initialization
	void Start () {
        //Debug.Break();
        directionalOne.transform.rotation = Quaternion.LookRotation(myDir);
        myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        myLight.intensity -= Time.deltaTime * 20;
	}
}
