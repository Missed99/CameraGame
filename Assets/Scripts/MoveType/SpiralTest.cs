using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralTest : MonoBehaviour
{
    public float distance;//半径
    public float spiralSpeed;//旋转速度
    public float angle;
    //public float riseSpeed;//上升速度

    public float StartTime;

    private bool isStart = false;

    public Transform center;
    private Transform surround;

    private float value = 1f;

    private List<Vector3> pos = new List<Vector3>();//为绘制图而收集的点的集合

    void Awake()
    {
        //center = transform;//挂载脚本的物体为中心
        //surround = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;//围绕中心旋转的物体
        //center.name = "Center";
        //surround.name = "Surround";

        surround = transform;
        distance = (surround.position - center.position).magnitude;
        //surround.position = Vector3.Normalize(Vector3.forward) * distance + center.position;//指定旋转物体的位置


        StartTime = Random.Range(1, 3);

        
    }

    private void Start()
    {
        //开始旋转
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

        ////圆周运动
        Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);//计算出旋转10度所需要的四元数
        Vector3 dir = Vector3.Normalize(surround.position - center.position);//指向旋转物体的向量
        surround.position = rotate * dir * distance + center.position;//旋转之后的位置

        pos.Add(surround.position);

    }



        void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //在Scene视图中绘制螺纹线
        for (int i = 0; i < pos.Count - 1; i++)
        {
            Vector3 from = pos[i];
            Vector3 to = pos[i + 1];
            Gizmos.DrawLine(from, to);
        }
    }
}
