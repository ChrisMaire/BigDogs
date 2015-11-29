using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Music : MonoBehaviour
{
    public static AudioSource source;
    public AudioClip Menu;
    public AudioClip Game;

    void Awake()
    {
        source = GetComponent<AudioSource>();

    }

    public void PlayTrack_Menu()
    {
        if (source.isPlaying)
            source.Stop();
        source.clip = Menu;
        source.Play();
    }

    public void PlayTrack_Game()
    {
        if (source.isPlaying)
            source.Stop();
        source.clip = Game;
        source.Play();
    }
}