using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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
    float g = -9.8f;               //重力
    Vector3 playerrun;           //控制玩家运动的向量

    //地面检测
    public Transform groundCheckTransform;//检测的点
    public float sphereRadius;//检测半径
    public float jumpHeight;
    public bool isGrounded;
    public bool isJumping;
    public float slopeForceRayLength = 0.2f;
    public bool isSlope;
    public float slopeForce = 6.0f;

    void Start()
    {
        Objs = GameObject.FindGameObjectsWithTag("Ball");
        instance = this;
        player = GetComponent<CharacterController>();//获取人物的角色控制器组件 
    }

    void Update()
    {
        //斜坡检测可视化
        Debug.DrawRay(transform.position + player.height / 2 * Vector3.down, Vector3.down * player.height / 2 * slopeForceRayLength, Color.blue);

        //地面检测
        isSlope = OnSlope();
        
        //拍照
        TakePhoto();

        //鼠标设置
        MouseSetting();

        //控制玩家运动
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        //移动向量
        if (player.isGrounded)
        {
            playerrun = new Vector3(_horizontal, 0, _vertical) * speed;
        }
        else
        {
            playerrun = new Vector3(_horizontal * speed, playerrun.y, _vertical * speed) ;
        }

        bool jump = Input.GetButtonDown("Jump");
        if (isSlope || player.isGrounded)
        {
            // 在着地时阻止垂直速度无限下降
            if (playerrun.y < 0.0f)
            {
                playerrun.y = -2f;
            }

            //跳跃
            if (jump)
            {
                playerrun.y = Mathf.Sqrt(jumpHeight * -2f * g);
            }
        }

        //重力
        playerrun.y += g * Time.deltaTime;

        //移动
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime));

        //使用鼠标来控制相机的视角的旋转
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

        //相机的限制
        cameraRestraint();
    }

    //控制斜坡
    public void SetSlope()
    {
        //如果处于斜坡
        if (isSlope && !isJumping)
        {
            //向下增加力
            playerrun.y = player.height / 2 * slopeForceRayLength;
            player.Move(Vector3.down * player.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    //快门声音
    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
    }

    //检测是否在地面
    private bool CheckIsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheckTransform.position, sphereRadius);//检测范围内碰撞的物体
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Plane"))
            {
                Debug.Log("检测到了地面");
                return true;
            }
        }
        return false;
    }

    //拍照
    private void TakePhoto()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRed = 0;
            PlaySound();
            foreach (var item in Objs)
            {
                item.GetComponent<Hit>().Detect();
            }
        }
    }

    //鼠标
    private void MouseSetting()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标到视图中心
        Cursor.visible = false;//隐藏鼠标
    }

    //相机限制
    private void cameraRestraint()
    {
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

    ////地面检测调试
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    //地面检测可视化
    //    Gizmos.DrawWireSphere(groundCheckTransform.position, sphereRadius);
    //}

    //是否在斜面
    public bool OnSlope()
    {
        RaycastHit hit;

        // 向下打出射线（检测是否在斜坡上）
        if (Physics.Raycast(transform.position + player.height / 2 * Vector3.down, Vector3.down, out hit, player.height / 2 * slopeForceRayLength))
        {
            // 如果接触到的点的法线，不在(0,1,0)的方向上，那么人物就在斜坡上
            if (hit.normal != Vector3.up)
                return true;
        }
        return false;
    }
}