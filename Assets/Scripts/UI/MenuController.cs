using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button startButton; // 开始游戏按钮
    public CanvasGroup canvasGroup; // 用于控制菜单界面的 CanvasGroup
    public float fadeDuration = 1f; // 渐隐动画持续时间
    // Start is called before the first frame update
    void Start()
    {
        // 确保按钮是可用的，并添加点击事件
        startButton.onClick.AddListener(OnStartButtonClicked);

        // 确保 CanvasGroup 在开始时是可交互的
        canvasGroup.alpha = 1f; // 初始完全可见
    }

    void OnStartButtonClicked()
    {
        // 开始渐隐并加载游戏场景
        StartCoroutine(FadeOutAndLoadScene()); 
    }

    IEnumerator FadeOutAndLoadScene()
    {
        // 渐隐效果
        float startAlpha = canvasGroup.alpha;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime);
            yield return null; // 等待下一帧
        }

        canvasGroup.alpha = 0; // 确保最终完全透明

        // 加载新场景
        SceneManager.LoadScene(1);
    }
}
