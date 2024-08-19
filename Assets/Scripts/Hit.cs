using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit: MonoBehaviour
{
    public bool isRed;
    public bool isFloat;
    GameObject cube;
    Collider objCollider;
    Camera cam;
    Plane[] planes;

    void Awake()
    {
        cube = this.gameObject;
        cam = Camera.main;
        
        objCollider = GetComponent<Collider>();
    }


    void Update()
    {



    }
    public bool RayForGame()//�����巢������
    {
        RaycastHit hit;
        Vector3 V3 =( Camera.main.transform.position-transform.position).normalized;
        if (Physics.Raycast(transform.position, V3, out hit, 20, LayerMask.GetMask("Water")))
        {
            
            Debug.Log(hit.transform.name + hit.point);//��ӡ������ײ��������
            Debug.DrawLine(transform.position, hit.point, Color.red, 1);//��ӡ���ߣ������ɽű��������嵽��������ײ�������壩
            return true;
        }
        else
            return false;
    }
    public void Detect()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))//��Ļ����ȥѰ����ײ�壨����
        {
            if (isRed)
                Player.instance.isRed++;
            else if (RayForGame())
                Player.instance.isRed+=0;
            else
                Player.instance.isRed--;
            Debug.Log(cube.name + "��⵽��");

            if (GetComponent<SphereMove>())//����ͽⶳ
                GetComponent<SphereMove>().isMove = !GetComponent<SphereMove>().isMove;
        }
        
    }
}
