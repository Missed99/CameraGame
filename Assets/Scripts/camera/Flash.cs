using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Image flash;
    public float flashDuration = 0.2f; 
    // Start is called before the first frame update
    void Start()
    {
        flash.color = new Color(1, 1, 1, 0); // ȷ����ʼʱ��͸����
    }
    public void TriggerFlash()
    {
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        // ����ʾ
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / (flashDuration / 2)); // ǰ�벿�ֽ�������
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // ˲��͸��
        flash.color = new Color(1, 1, 1, 1);

        // ����ʧ
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / (flashDuration / 2))); // ��벿�ֽ�������
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // ȷ����ȫ͸��
        flash.color = new Color(1, 1, 1, 0);
    }
}
