using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlexibleCube : MonoBehaviour
{
    public bool isMove = true;
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 1f); // ��С����ֵ
    public Vector3 maxScale = new Vector3(2f, 0.5f, 1f); // �������ֵ
    public float scaleSpeed = 1f; // �����ٶ�

    private Vector3 direction; // ��ǰ���ŷ���
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = minScale;
        direction = Vector3.Normalize(maxScale - minScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {   
            transform.localScale += direction * scaleSpeed * Time.deltaTime;

            if (transform.localScale.x >= maxScale.x)
            {
                transform.localScale = maxScale; 
                direction = -direction; 
            }
            else if (transform.localScale.x <= minScale.x)
            {
                transform.localScale = minScale; 
                direction = -direction; 
            }
        }
    }
}
