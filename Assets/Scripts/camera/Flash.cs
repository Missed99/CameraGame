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
        flash.color = new Color(1, 1, 1, 0); // 确保开始时是透明的
    }
    public void TriggerFlash()
    {
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        // 逐渐显示
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / (flashDuration / 2)); // 前半部分渐渐增加
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 瞬间透明
        flash.color = new Color(1, 1, 1, 1);

        // 逐渐消失
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / (flashDuration / 2))); // 后半部分渐渐减少
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 确保完全透明
        flash.color = new Color(1, 1, 1, 0);
    }
}
