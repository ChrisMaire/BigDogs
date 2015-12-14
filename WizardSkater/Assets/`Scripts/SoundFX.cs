using UnityEngine;
using System.Collections;

public class SoundFX : MonoBehaviour
{
    public static AudioSource source;

    public AudioClip LevelStart;
    public AudioClip Skate;
    public AudioClip Jump;
    public AudioClip Trick;
    public AudioClip Turbo;
    public AudioClip ObstacleHit;
    public AudioClip LevelComplete;

    public AudioClip uiMove;
    public AudioClip uiSubmit;
    public AudioClip uiCancel;

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
    public void OneShot_Skate()
    {
        source.PlayOneShot(Skate);
    }
    public void OneShot_Jump()
    {
        source.PlayOneShot(Jump);
    }
    public void OneShot_Trick()
    {
        source.PlayOneShot(Trick);
    }
    public void OneShot_Turbo()
    {
        source.PlayOneShot(Turbo);
    }
    public void OneShot_ObstacleHit()
    {
        source.PlayOneShot(ObstacleHit);
    }
    public void OneShot_LevelComplete()
    {
        source.PlayOneShot(LevelComplete);
    }

    public void uiOneShot_Move()
    {
        source.PlayOneShot(uiMove);
    }
    public void uiOneShot_Submit()
    {
        source.PlayOneShot(uiSubmit);
    }
    public void uiOneShot_Cancel()
    {
        source.PlayOneShot(uiCancel);
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
