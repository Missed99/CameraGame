using DG.Tweening.Core.Easing;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int doorNum;
    public Vector3 initPos;//玩家起始位置
    public int redNum;
    public int yNum;
    public int pNum;
    public GameObject[] Objs;//Tag == Ball
    public GameObject[] Objs_Level3;//Tag == Level3_Ball
    public Slider slider;

    public static Player instance;
    public AudioClip[] audioClips;
    public AudioClip[] scaleAudioClips;
    CharacterController player;  //定义角色控制器组件
    public new Transform camera; //新建一个camera对象用于放入所要实现的第一人称相机
    public float speed = 2f;			 //角色移动速度
    public int fastSpeedX;
    float x, y;                  //相机旋转x，y轴控制
    float g = -9.8f;               //重力
    Vector3 playerrun;           //控制玩家运动的向量
    public Image flash;  
    public float flashDuration = 0.01f;

    //地面检测
    public Transform groundCheckTransform;//检测的点
    public float sphereRadius;//检测半径
    public float jumpHeight;
    public bool isGrounded;
    public bool isJumping;
    
    public float slopeForceRayLength = 0.2f;
    public bool isSlope;
    public float slopeForce = 6.0f;

    private void Awake()
    {
        instance = this;
        Objs = GameObject.FindGameObjectsWithTag("Ball");//找到所有的ball
        Objs_Level3 = GameObject.FindGameObjectsWithTag("Level3_Ball"); ;//找到第三关对应Tag的Ball
        player = GetComponent<CharacterController>();//获取人物的角色控制器组件 
        flash.color = new Color(1, 1, 1, 0); 
    }

    void Start()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate {
            PlaySound("Scale");
        }) ;
        //让所有球添加悬浮脚本
        BallAddFloatingScript(Objs);

        //让第三关对应Tag的球添加脚本
        BallAddJumpAndChangeScaleScript(Objs_Level3);
        initPos = transform.position;
    }

    void Update()
    {
        //斜坡检测可视化
        //Debug.DrawRay(transform.position + player.height / 2 * Vector3.down, Vector3.down * player.height / 2 * slopeForceRayLength, Color.blue);

        //地面检测
        isSlope = OnSlope();
        
        //拍照
        TakePhoto();

        //鼠标设置
        if (GamePause.isPause == false)
        {
            MouseSetting();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//玩家快速移动
            speed *= fastSpeedX;
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            speed /= fastSpeedX;
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //MouseSetting();

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

        Physics.autoSyncTransforms = true;//用来解决CharacterController导致的Transform.Position赋值后不起作用
        //移动
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime));

        //使用鼠标来控制相机的视角的旋转
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

        //相机的限制
        CameraRestraint();
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
    public void PlaySound(string s)
    {
        if(s=="Action")
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
        if(s=="Scale")
            GetComponent<AudioSource>().PlayOneShot(scaleAudioClips[Random.Range(0,2)]);
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
        //鼠标按下左键
        if (Input.GetMouseButtonDown(0) && GamePause.isPause == false)
        {
            redNum = 0;
            yNum = 0;
            pNum =0;
            PlaySound("Action");
            TriggerFlash();
            foreach (var item in Objs)//遍历所有的球
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
    private void CameraRestraint()
    {
        //让相机z轴保持不变，防止相机倾斜
        if (camera.localEulerAngles.z != 0)
        {
            float rotX = camera.localEulerAngles.x;
            float rotY = camera.localEulerAngles.y;
            camera.localEulerAngles = new Vector3(rotX, rotY, 0);
        }

        Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * 80f;
        if (Camera.main.fieldOfView > 50)
            Camera.main.fieldOfView = 50;
        if (Camera.main.fieldOfView < 20)
            Camera.main.fieldOfView = 20;
        slider.value = (Camera.main.fieldOfView-20) / 30;
        
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

    //给所有的Ball添加悬浮脚本
    public void BallAddFloatingScript(GameObject[] Objs)
    {
        for(int i = 0; i < Objs.Length; i++)
        {
            if(Objs[i].GetComponent<Hit>().isFloat)
            Objs[i].AddComponent<Floating>();
        }
    }
    public void TriggerFlash()
    {
        StartCoroutine(Flashing());
    }
    private IEnumerator Flashing()
    {
        // 逐渐显示
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / (flashDuration / 2)); // 前半部分渐渐增加
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 瞬间透明
        flash.color = new Color(1, 1, 1, 1);

        // 逐渐消失
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / (flashDuration / 2))); // 后半部分渐渐减少
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // 确保完全透明
        flash.color = new Color(1, 1, 1, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SphereMove>()&&other.GetComponent<SphereMove>().isDead)
        {
            transform.position = initPos;
        }

        //撞到车了
        if(other.gameObject.tag == "Car")
        {
            transform.position = initPos;
        }

        //进门
        if(other.gameObject.tag == "NextLevel")
        {
            GameManager.Instance.LoadNextScene();//加载下一关
        }
    }

    //给第三关的球添加运动脚本
    public void BallAddJumpAndChangeScaleScript(GameObject[] Objs)
    {
        for (int i = 0; i < Objs.Length; i++)
        {
            Objs[i].gameObject.AddComponent<JumpBallAndChangeScale>();
        }
    }
}