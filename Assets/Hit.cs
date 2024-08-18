using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit: MonoBehaviour
{
    public bool isRed;
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
    public bool RayForGame()//从物体发射射线
    {
        RaycastHit hit;
        Vector3 V3 =( Camera.main.transform.position-transform.position).normalized;
        if (Physics.Raycast(transform.position, V3, out hit, 20, LayerMask.GetMask("Water")))
        {
            
            Debug.Log(hit.transform.name + hit.point);//打印射线碰撞到的物体
            Debug.DrawLine(transform.position, hit.point, Color.red, 1);//打印射线（射线由脚本所在物体到达射线碰撞到的物体）
            return true;
        }
        else
            return false;
    }
    public void Detect()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))//锟斤拷幕锟斤拷锟斤拷去寻锟斤拷锟斤拷撞锟藉（锟斤拷锟斤拷
        {
            if (isRed)
                Player.instance.isRed++;
            else if (RayForGame())
                Player.instance.isRed+=0;
            else
                Player.instance.isRed--;
            Debug.Log(cube.name + "检测到了");
        }
    }
}
