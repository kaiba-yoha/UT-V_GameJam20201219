using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEID
{
    PlayStart,
    PlayFinish,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<SoundManager>();
            return m_Instance;
        }
    }
    static SoundManager m_Instance;

    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] AudioClip[] naguruVoices;
    [SerializeField] AudioClip brakeBlock;
    [SerializeField] AudioClip playStartVoice;
    [SerializeField] AudioClip playFinishVoice;


    public void PlayBGM()
    {
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void PlaySE(SEID seID)
    {
        switch(seID) {
            case SEID.PlayStart:
                seAudioSource.PlayOneShot(playStartVoice);
                break;
            case SEID.PlayFinish:
                seAudioSource.PlayOneShot(playFinishVoice);
                break;
            default:
                break;
        }
    }

    public void PlayBrakeBlockSE()
    {
        int i = Random.Range(0,naguruVoices.Length);
        seAudioSource.PlayOneShot(naguruVoices[i]);

        seAudioSource.PlayOneShot(brakeBlock);
    }
}
