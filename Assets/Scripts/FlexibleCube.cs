using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlexibleCube : MonoBehaviour
{
    public bool isMove = true;
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 1f); // 最小缩放值
    public Vector3 maxScale = new Vector3(2f, 0.5f, 1f); // 最大缩放值
    public float scaleSpeed = 1f; // 伸缩速度

    private Vector3 targetScale; // 当前目标缩放值
    private Vector3 direction; // 当前缩放方向
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = minScale;
        targetScale = maxScale;
        direction = Vector3.Normalize(maxScale - minScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            /*transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localScale, maxScale) < 0.1f)
            {
                transform.localScale = maxScale;
                targetScale = minScale;
            }
            else if (Vector3.Distance(transform.localScale, minScale) < 0.1f)
            {
                transform.localScale = minScale;
                targetScale = maxScale;
            }*/
            
            transform.localScale += direction * scaleSpeed * Time.deltaTime;

            // 检查当前缩放值，并调整方向
            if (transform.localScale.x >= maxScale.x)
            {
                // 到达最大缩放值，反转方向
                transform.localScale = maxScale; // 确保不超过最大值
                direction = -direction; // 设置缩放方向为减小
            }
            else if (transform.localScale.x <= minScale.x)
            {
                // 到达最小缩放值，反转方向
                transform.localScale = minScale; // 确保不低于最小值
                direction = -direction; // 设置缩放方向为增加
            }
        }
    }
}
