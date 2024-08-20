using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmAudioSource;

    public List<AudioClip> audioClipsList = new List<AudioClip>();//存放BGM的集合

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    //切换BGM
    public void ChangeBGMAudioClip(int tmp)
    {
        bgmAudioSource.clip = audioClipsList[tmp];//切换到序列号场景的BGM
        bgmAudioSource.Play();
    }


}
