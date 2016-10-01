using UnityEngine;
using System.Collections;

public class SoundBank : MonoBehaviour {

    public static SoundBank sndBnk;

    public AudioClip menuClick, critMetronome;

    public AudioClip batHitBall, batCritBall;
    public AudioClip hitFloorWithBat;
    public AudioClip[] ballHits;

    public AudioClip[] slugVoices;

    public AudioClip[] playerSwingVoices, playerHurtVoices, playerDeathSounds;

    // Use this for initialization
    void Start () {
        sndBnk = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public AudioClip GetHitSound()
    {
        AudioClip returnThis;

        returnThis = ballHits[Random.Range(0, ballHits.Length)];

        return returnThis;
    }
}
