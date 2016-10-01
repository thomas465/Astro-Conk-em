using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    public AudioClip[] songList;
    int[] loopPoints;

    int curSongIndex = 0;

    AudioSource musicSource;

	// Use this for initialization
	void Start () {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        PlaySong(0);

        loopPoints = new int[songList.Length];
        loopPoints[0] = 325662;
        loopPoints[1] = 352800;
        loopPoints[2] = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(musicSource.timeSamples>musicSource.clip.samples)
        {
            musicSource.timeSamples = loopPoints[curSongIndex];
            musicSource.Play();
            Debug.Log("LOOPED");
        }
	}

    public void PlaySong(int index)
    {
        musicSource.clip = songList[index];
        musicSource.Play();

        curSongIndex = index;
    }

    public void StopSong()
    {
        musicSource.Stop();
    }
}
