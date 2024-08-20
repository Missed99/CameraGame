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
        // ��ȡ AudioSource ���
        audioSource = gameObject.AddComponent<AudioSource>();

        // ��ȡ Button �������Ӽ����¼�
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // ʹ�� EventTrigger ��������ѡ���¼�
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entry.callback.AddListener((data) => { OnSelect(); });
        trigger.triggers.Add(entry);
    }

    // ѡ��ʱ������Ч
    void OnSelect()
    {
        PlaySound(selectSound);
    }

    // ���ʱ������Ч
    void OnClick()
    {
        PlaySound(clickSound);
    }

    // ������Ч
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // ����һ������Ч
        }
    }
}
