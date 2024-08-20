using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgmAudioSource;

    public List<AudioClip> audioClipsList = new List<AudioClip>();//���BGM�ļ���

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    //�л�BGM
    public void ChangeBGMAudioClip(int tmp)
    {
        bgmAudioSource.clip = audioClipsList[tmp];//�л������кų�����BGM
        bgmAudioSource.Play();
    }


}
