using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMananger : MonoBehaviour
{
    public static SoundMananger instance;

    public AudioSource audioSource;
    public AudioClip audioClip_Die;
    public AudioClip audioClip_EngineDriving;
    public AudioClip audioClip_EngineIdle;
    public AudioClip audioClip_Explosion;
    public AudioClip audioClip_Fire;
    public AudioClip audioClip_GetBonus;
    public AudioClip audioClip_HeartDamage;
    public AudioClip audioClip_Hit;
    public AudioClip audioClip_Start;
    public AudioClip audioClip_FinalWar;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void BGMplayer()
    {
    }
    public void PlayAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
