using UnityEngine;
using System.Collections;

public class SoundFX : MonoBehaviour
{
    public static AudioSource source;

    public AudioClip LevelStart;
    public AudioClip Jump;
    public AudioClip LevelComplete;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void StopAll()
    {
        source.Stop();
    }

    public void OneShot_LevelStart()
    {
        source.PlayOneShot(LevelStart);
    }
    public void OneShot_Jump()
    {
        source.PlayOneShot(Jump);
    }
    public void OneShot_WorldComplete()
    {
        source.PlayOneShot(LevelComplete);
    }

    //public float pitchStart = 0.85f;
    //public float pitchStep = 0.025f;
    //public float pitchCurrent;
    //public float pitchCap = 1.05f;

    //public void ResetPitch()
    //{
    //    pitchCurrent = pitchStart;
    //    source.pitch = pitchCurrent;
    //}

    //void ChangePitch()
    //{
    //    float modifier = -1f;
    //    int rando = Random.Range(0, 2);
    //    if (rando == 1)
    //        modifier = 1f;

    //    pitchCurrent += pitchStep * modifier;

    //    if (pitchCurrent < pitchCap && pitchCurrent > pitchStart)
    //    {
    //        source.pitch = pitchCurrent;
    //    }
    //}
}
