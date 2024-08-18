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
    CharacterController player;  //�����ɫ���������
    public new Transform camera; //�½�һ��camera�������ڷ�����Ҫʵ�ֵĵ�һ�˳����
    public float speed = 2f;			 //��ɫ�ƶ��ٶ�
    float x, y;                  //�����תx��y�����
    float g = 9.8f;               //����
    Vector3 playerrun;           //��������˶�������

    //������
    public Transform groundCheckTransform;//���ĵ�
    public float sphereRadius;//���뾶
    public float jumpHeight;
    public bool isGrounded;

    void Start()
    {
        Objs = GameObject.FindGameObjectsWithTag("Ball");
        instance = this;
        player = GetComponent<CharacterController>();//��ȡ����Ľ�ɫ��������� 
    }

    void Update()
    {
        //������
        isGrounded = CheckIsGrounded();

        //����
        TakePhoto();

        //�������
        MouseSetting();

        //��������˶�
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        //�ƶ�����
        if (isGrounded)
        {
            playerrun = new Vector3(_horizontal, 0, _vertical) * speed;
        }

        //��Ծ
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerrun.y = Mathf.Sqrt(jumpHeight * 2f * g);
        }

        //����
        playerrun.y -= g * Time.deltaTime;

        //�ƶ�
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime));

        //ʹ�����������������ӽǵ���ת
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

        //���������
        cameraRestraint();
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
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    //����
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

    //���
    private void MouseSetting()
    {
        Cursor.lockState = CursorLockMode.Locked; // ������굽��ͼ����
        Cursor.visible = false;//�������
    }

    //�������
    private void cameraRestraint()
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

    //���������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //��������ӻ�
        Gizmos.DrawWireSphere(groundCheckTransform.position, sphereRadius);
    }
}