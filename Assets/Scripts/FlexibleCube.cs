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

    private Vector3 targetScale; // ��ǰĿ������ֵ
    private Vector3 direction; // ��ǰ���ŷ���
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

            // ��鵱ǰ����ֵ������������
            if (transform.localScale.x >= maxScale.x)
            {
                // �����������ֵ����ת����
                transform.localScale = maxScale; // ȷ�����������ֵ
                direction = -direction; // �������ŷ���Ϊ��С
            }
            else if (transform.localScale.x <= minScale.x)
            {
                // ������С����ֵ����ת����
                transform.localScale = minScale; // ȷ����������Сֵ
                direction = -direction; // �������ŷ���Ϊ����
            }
        }
    }
}
