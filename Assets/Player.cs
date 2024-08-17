using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
  @Author:Rekite
 */
public class Player : MonoBehaviour
{
    public int isRed;
    public GameObject[] Objs;
    public static Player instance;
    public AudioClip[] audioClips;
    CharacterController player;  //定义角色控制器组件
    public new Transform camera; //新建一个camera对象用于放入所要实现的第一人称相机
    public float speed = 2f;			 //角色移动速度
    float x, y;                  //相机旋转x，y轴控制
    float g = 10f;               //重力
    Vector3 playerrun;           //控制玩家运动的向量

    void Start()
    {
        Objs = GameObject.FindGameObjectsWithTag("Ball");
        instance = this;
        player = GetComponent<CharacterController>();//获取人物的角色控制器组件    
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            isRed = 0;
            PlaySound();
            foreach (var item in Objs)
            {
                item.GetComponent<Hit>().Detect();
            } 

        }



        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标到视图中心
        Cursor.visible = false;//隐藏鼠标

        //控制玩家运动
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        if (player.isGrounded)
        {
            playerrun = new Vector3(_horizontal, 0, _vertical);
        }
        playerrun.y -= g * Time.deltaTime;
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime * speed));

        //使用鼠标来控制相机的视角的旋转
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

        //让相机z轴保持不变，防止相机倾斜
        if (camera.localEulerAngles.z != 0)
        {
            float rotX = camera.localEulerAngles.x;
            float rotY = camera.localEulerAngles.y;
            camera.localEulerAngles = new Vector3(rotX, rotY, 0);
        }


        Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * 100f;
        if (Camera.main.fieldOfView > 50)
            Camera.main.fieldOfView = 50;
        if (Camera.main.fieldOfView < 30)
            Camera.main.fieldOfView = 30;
    }
    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
    }
}