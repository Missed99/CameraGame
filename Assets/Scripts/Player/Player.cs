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
    public Vector3 initPos;//�����ʼλ��
    public int redNum;
    public int yNum;
    public int pNum;
    public GameObject[] Objs;//Tag == Ball
    public GameObject[] Objs_Level3;//Tag == Level3_Ball
    public Slider slider;

    public static Player instance;
    public AudioClip[] audioClips;
    public AudioClip[] scaleAudioClips;
    CharacterController player;  //�����ɫ���������
    public new Transform camera; //�½�һ��camera�������ڷ�����Ҫʵ�ֵĵ�һ�˳����
    public float speed = 2f;			 //��ɫ�ƶ��ٶ�
    public int fastSpeedX;
    float x, y;                  //�����תx��y�����
    float g = -9.8f;               //����
    Vector3 playerrun;           //��������˶�������
    public Image flash;  
    public float flashDuration = 0.01f;

    //������
    public Transform groundCheckTransform;//���ĵ�
    public float sphereRadius;//���뾶
    public float jumpHeight;
    public bool isGrounded;
    public bool isJumping;
    
    public float slopeForceRayLength = 0.2f;
    public bool isSlope;
    public float slopeForce = 6.0f;

    private void Awake()
    {
        instance = this;
        Objs = GameObject.FindGameObjectsWithTag("Ball");//�ҵ����е�ball
        Objs_Level3 = GameObject.FindGameObjectsWithTag("Level3_Ball"); ;//�ҵ������ض�ӦTag��Ball
        player = GetComponent<CharacterController>();//��ȡ����Ľ�ɫ��������� 
        flash.color = new Color(1, 1, 1, 0); 
    }

    void Start()
    {
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate {
            PlaySound("Scale");
        }) ;
        //����������������ű�
        BallAddFloatingScript(Objs);

        //�õ����ض�ӦTag������ӽű�
        BallAddJumpAndChangeScaleScript(Objs_Level3);
        initPos = transform.position;
    }

    void Update()
    {
        //б�¼����ӻ�
        //Debug.DrawRay(transform.position + player.height / 2 * Vector3.down, Vector3.down * player.height / 2 * slopeForceRayLength, Color.blue);

        //������
        isSlope = OnSlope();
        
        //����
        TakePhoto();

        //�������
        if (GamePause.isPause == false)
        {
            MouseSetting();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//��ҿ����ƶ�
            speed *= fastSpeedX;
        else if(Input.GetKeyUp(KeyCode.LeftShift))
            speed /= fastSpeedX;
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //MouseSetting();

        //��������˶�
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        //�ƶ�����
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
            // ���ŵ�ʱ��ֹ��ֱ�ٶ������½�
            if (playerrun.y < 0.0f)
            {
                playerrun.y = -2f;
            }

            //��Ծ
            if (jump)
            {
                playerrun.y = Mathf.Sqrt(jumpHeight * -2f * g);
            }
        }

        //����
        playerrun.y += g * Time.deltaTime;

        Physics.autoSyncTransforms = true;//�������CharacterController���µ�Transform.Position��ֵ��������
        //�ƶ�
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime));

        //ʹ�����������������ӽǵ���ת
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

        //���������
        CameraRestraint();
    }

    //����б��
    public void SetSlope()
    {
        //�������б��
        if (isSlope && !isJumping)
        {
            //����������
            playerrun.y = player.height / 2 * slopeForceRayLength;
            player.Move(Vector3.down * player.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    //��������
    public void PlaySound(string s)
    {
        if(s=="Action")
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
        if(s=="Scale")
            GetComponent<AudioSource>().PlayOneShot(scaleAudioClips[Random.Range(0,2)]);
    }

    //����Ƿ��ڵ���
    private bool CheckIsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(groundCheckTransform.position, sphereRadius);//��ⷶΧ����ײ������
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Plane"))
            {
                Debug.Log("��⵽�˵���");
                return true;
            }
        }
        return false;
    }

    //����
    private void TakePhoto()
    {
        //��갴�����
        if (Input.GetMouseButtonDown(0) && GamePause.isPause == false)
        {
            redNum = 0;
            yNum = 0;
            pNum =0;
            PlaySound("Action");
            TriggerFlash();
            foreach (var item in Objs)//�������е���
            {
                item.GetComponent<Hit>().Detect();
            }
        }
    }

    //���
    private void MouseSetting()
    {
        Cursor.lockState = CursorLockMode.Locked; // ������굽��ͼ����
        Cursor.visible = false;//�������
    }

    //�������
    private void CameraRestraint()
    {
        //�����z�ᱣ�ֲ��䣬��ֹ�����б
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

    ////���������
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    //��������ӻ�
    //    Gizmos.DrawWireSphere(groundCheckTransform.position, sphereRadius);
    //}

    //�Ƿ���б��
    public bool OnSlope()
    {
        RaycastHit hit;

        // ���´�����ߣ�����Ƿ���б���ϣ�
        if (Physics.Raycast(transform.position + player.height / 2 * Vector3.down, Vector3.down, out hit, player.height / 2 * slopeForceRayLength))
        {
            // ����Ӵ����ĵ�ķ��ߣ�����(0,1,0)�ķ����ϣ���ô�������б����
            if (hit.normal != Vector3.up)
                return true;
        }
        return false;
    }

    //�����е�Ball��������ű�
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
        // ����ʾ
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / (flashDuration / 2)); // ǰ�벿�ֽ�������
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // ˲��͸��
        flash.color = new Color(1, 1, 1, 1);

        // ����ʧ
        elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / (flashDuration / 2))); // ��벿�ֽ�������
            flash.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // ȷ����ȫ͸��
        flash.color = new Color(1, 1, 1, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<SphereMove>()&&other.GetComponent<SphereMove>().isDead)
        {
            transform.position = initPos;
        }

        //ײ������
        if(other.gameObject.tag == "Car")
        {
            transform.position = initPos;
        }

        //����
        if(other.gameObject.tag == "NextLevel")
        {
            GameManager.Instance.LoadNextScene();//������һ��
        }
    }

    //�������ص�������˶��ű�
    public void BallAddJumpAndChangeScaleScript(GameObject[] Objs)
    {
        for (int i = 0; i < Objs.Length; i++)
        {
            Objs[i].gameObject.AddComponent<JumpBallAndChangeScale>();
        }
    }
}