using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip selectSound; 
    public AudioClip clickSound; 
    private AudioSource audioSource; 
    // Start is called before the first frame update
    void Start()
    {
        // 获取 AudioSource 组件
        audioSource = gameObject.AddComponent<AudioSource>();

        // 获取 Button 组件并添加监听事件
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // 使用 EventTrigger 组件来检测选择事件
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((data) => { OnSelect(); });
        trigger.triggers.Add(entry);
    }

    // 选中时播放音效
    void OnSelect()
    {
        PlaySound(selectSound);
    }

    // 点击时播放音效
    void OnClick()
    {
        PlaySound(clickSound);
    }

    // 播放音效
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // 播放一次性音效
        }
    }
}
