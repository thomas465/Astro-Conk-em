using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    public AudioClip[] songList;
    int[] loopPoints;

    int curSongIndex = 0;
    AudioSource musicSource;

    bool fadeToSilence = false;
    float musicVolume = 0.9f;

	// Use this for initialization
	void Start () {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0;
        PlaySong(0);

        loopPoints = new int[songList.Length];
        loopPoints[0] = 325662;
        loopPoints[1] = 352800;
        loopPoints[2] = 0;
    }
	
	// Update is called once per frame
	void Update () {

        //Looping
        if (musicSource.clip)
        {
            if (musicSource.timeSamples > musicSource.clip.samples)
            {
                musicSource.timeSamples = loopPoints[curSongIndex];
                musicSource.Play();
                Debug.Log("LOOPED");
            }
        }

        //Fading
        if(fadeToSilence)
        {
            musicSource.volume -= Time.deltaTime;
        }
	}

    public void PlaySong(int index)
    {
        musicSource.clip = songList[index];
        musicSource.Play();

        curSongIndex = index;
        musicSource.volume = musicVolume;
        fadeToSilence = false;
    }

    public void StopSong()
    {
        fadeToSilence = true;
        //musicSource.Stop();
    }
}
