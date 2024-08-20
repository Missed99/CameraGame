using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spiral : MonoBehaviour
{
    public float distance;//�뾶
    public float spiralSpeed;//��ת�ٶ�
    public float angle;
    public float riseSpeed;//�����ٶ�

    private bool isStart = false;

    public bool isPositive;
    public bool isReverse;

    private Transform center;
    private Transform surround;
    private float value = 1f;

    public float positvValue;
    public float reverseValue;

    private List<Vector3> pos = new List<Vector3>();//Ϊ����ͼ���ռ��ĵ�ļ���

    void Awake()
    {
        center = transform;//���ؽű�������Ϊ����
        surround = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;//Χ��������ת������
        center.name = "Center";
        surround.name = "Surround";

        surround.position = Vector3.Normalize(Vector3.forward) * distance + center.position;//ָ����ת�����λ��


        //��ʼ��ת
        StartCoroutine(InitEnd());
    }

    IEnumerator InitEnd()
    {
        yield return new WaitForSeconds(1);
        isStart = true;
    }

    void Update()
    {
        if (!isStart) return;

        if (isReverse)
        {
            if(value >= positvValue)
            {
                isReverse = false;
                isPositive = true;
            }

            value += spiralSpeed * Time.deltaTime * 0.001f;
            //�½�
            surround.Translate(Vector3.down * riseSpeed * Time.deltaTime);

            float tmp_distance = Mathf.Lerp(0, distance, value);

            //Բ���˶�
            Quaternion rotate = Quaternion.AngleAxis(-angle, Vector3.up);//�������ת10������Ҫ����Ԫ��
            Vector3 dir = Vector3.Normalize(surround.position - center.position);//ָ����ת���������
            surround.position = rotate * dir * tmp_distance + center.position;//��ת֮���λ��
        }
        
        if (isPositive)
        {
            if(value <= reverseValue)
            {
                isPositive = false;
                isReverse = true;
            }

            value -= spiralSpeed * Time.deltaTime * 0.001f;
            //����
            surround.Translate(Vector3.up * riseSpeed * Time.deltaTime);

            float tmp_distance = Mathf.Lerp(0, distance, value);

            //Բ���˶�
            Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);//�������ת10������Ҫ����Ԫ��
            Vector3 dir = Vector3.Normalize(surround.position - center.position);//ָ����ת���������
            surround.position = rotate * dir * tmp_distance + center.position;//��ת֮���λ��

        }



        //û��Y��仯�ģ����Ը�����ʼ��Y��ֵ
        //float tmp_distance = Mathf.Lerp(0, distance, value);

        ////Բ���˶�
        //Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);//�������ת10������Ҫ����Ԫ��
        //Vector3 dir = Vector3.Normalize(surround.position - center.position);//ָ����ת���������
        //surround.position = rotate * dir * tmp_distance + center.position;//��ת֮���λ��

        pos.Add(surround.position);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //��Scene��ͼ�л���������
        for (int i = 0; i < pos.Count - 1; i++)
        {
            Vector3 from = pos[i];
            Vector3 to = pos[i + 1];
            Gizmos.DrawLine(from, to);
        }
    }
}
