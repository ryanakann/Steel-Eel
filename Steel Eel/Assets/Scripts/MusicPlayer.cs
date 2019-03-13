using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;

    public AudioSource[] music;
    AudioSource current_track;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        music = GetComponents<AudioSource>();
        current_track = music[0];
        current_track.volume = 1;
    }

    public void Play(music m)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchTracks(music[(int)m]));
    }

    private void Update()
    {
        foreach (AudioSource a in music)
        {
            if (a != current_track && a.isPlaying)
            {
                a.volume -= 0.01f;
                if (a.volume < 0.01f)
                {
                    a.volume = 0;
                    a.Stop();
                }
            }
        }
    }

    IEnumerator SwitchTracks(AudioSource track)
    {
        current_track = track;
        if (!track.isPlaying)
        {
            track.volume = 0;
            track.Play();
        }
        while (track.volume < 1)
        {
            track.volume += 0.01f;
            yield return null;
        }
    }
}

public enum music { menu, game, win, lose, end }
