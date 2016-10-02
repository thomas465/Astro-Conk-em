using UnityEngine;
using System.Collections;

public class EnemyEntranceScript : MonoBehaviour
{
    public ParticleSystem myAppearSystem;
    public AudioClip appearSnd;

    AudioSource audioSource;

    void Start()
    {
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();       
    }

    // Use this for initialization
    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        myAppearSystem.Play();
        audioSource.PlayOneShot(appearSnd);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 2 * Time.deltaTime);
    }
}
