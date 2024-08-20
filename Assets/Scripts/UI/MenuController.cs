using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton; // ��ʼ��Ϸ��ť
    public CanvasGroup canvasGroup; // ���ڿ��Ʋ˵������ CanvasGroup
    public float fadeDuration = 1f; // ������������ʱ��
    // Start is called before the first frame update
    void Start()
    {
        // ȷ����ť�ǿ��õģ�����ӵ���¼�
        startButton.onClick.AddListener(OnStartButtonClicked);

        // ȷ�� CanvasGroup �ڿ�ʼʱ�ǿɽ�����
        canvasGroup.alpha = 1f; // ��ʼ��ȫ�ɼ�
    }

    void OnStartButtonClicked()
    {
        // ��ʼ������������Ϸ����
        StartCoroutine(FadeOutAndLoadScene()); 
    }

    IEnumerator FadeOutAndLoadScene()
    {
        // ����Ч��
        float startAlpha = canvasGroup.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime);
            yield return null; // �ȴ���һ֡
        }

        canvasGroup.alpha = 0; // ȷ��������ȫ͸��

        // �����³���
        SceneManager.LoadScene(1);
    }
}
