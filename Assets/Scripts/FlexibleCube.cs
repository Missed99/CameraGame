using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleCube : MonoBehaviour
{
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 1f); // 最小缩放值
    public Vector3 maxScale = new Vector3(2f, 0.5f, 1f); // 最大缩放值
    public float scaleSpeed = 1f; // 伸缩速度

    private Vector3 targetScale; // 当前目标缩放值
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = minScale;
        targetScale = maxScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.localScale, maxScale) < 0.01f)
        {
            transform.localScale = maxScale;
            targetScale = minScale;
        }
        else if(Vector3.Distance(transform.localScale, minScale) < 0.01f)
        {
            transform.localScale = minScale;
            targetScale = maxScale;
        }
    }
}
