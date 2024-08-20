using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralTest : MonoBehaviour
{
    public float distance;//�뾶
    public float spiralSpeed;//��ת�ٶ�
    public float angle;
    //public float riseSpeed;//�����ٶ�

    public float StartTime;

    private bool isStart = false;

    public Transform center;
    private Transform surround;

    private float value = 1f;

    private List<Vector3> pos = new List<Vector3>();//Ϊ����ͼ���ռ��ĵ�ļ���

    void Awake()
    {
        //center = transform;//���ؽű�������Ϊ����
        //surround = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;//Χ��������ת������
        //center.name = "Center";
        //surround.name = "Surround";

        surround = transform;
        distance = (surround.position - center.position).magnitude;
        //surround.position = Vector3.Normalize(Vector3.forward) * distance + center.position;//ָ����ת�����λ��


        StartTime = Random.Range(1, 3);

        
    }

    private void Start()
    {
        //��ʼ��ת
        StartCoroutine(InitEnd());
    }

    IEnumerator InitEnd()
    {
        yield return new WaitForSeconds(StartTime);
        isStart = true;
    }

    void Update()
    {
        if (!isStart) return;

        //float tmp_distance = Mathf.Lerp(0, distance, value);

        ////Բ���˶�
        Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);//�������ת10������Ҫ����Ԫ��
        Vector3 dir = Vector3.Normalize(surround.position - center.position);//ָ����ת���������
        surround.position = rotate * dir * distance + center.position;//��ת֮���λ��

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
