using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
  @Author:Rekite
 */
public class Player : MonoBehaviour
{
    public int isRed;
    public GameObject[] Objs;
    public static Player instance;
    public AudioClip[] audioClips;
    CharacterController player;  //�����ɫ���������
    public new Transform camera; //�½�һ��camera�������ڷ�����Ҫʵ�ֵĵ�һ�˳����
    public float speed = 2f;			 //��ɫ�ƶ��ٶ�
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
        player = GetComponent<CharacterController>();//��ȡ����Ľ�ɫ��������� 
        flash.color = new Color(1, 1, 1, 0); 
    }

    void Start()
    {
        //����������������ű�
        BallAddFloatingScript(Objs);
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
    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
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
            isRed = 0;
            PlaySound();
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

        Camera.main.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * 100f;
        if (Camera.main.fieldOfView > 50)
            Camera.main.fieldOfView = 50;
        if (Camera.main.fieldOfView < 30)
            Camera.main.fieldOfView = 30;
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
            Objs[i].gameObject.AddComponent<Floating>();
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
}