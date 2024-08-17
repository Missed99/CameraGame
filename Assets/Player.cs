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
    float g = 10f;               //����
    Vector3 playerrun;           //��������˶�������

    void Start()
    {
        Objs = GameObject.FindGameObjectsWithTag("Ball");
        instance = this;
        player = GetComponent<CharacterController>();//��ȡ����Ľ�ɫ���������    
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



        Cursor.lockState = CursorLockMode.Locked; // ������굽��ͼ����
        Cursor.visible = false;//�������

        //��������˶�
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");
        if (player.isGrounded)
        {
            playerrun = new Vector3(_horizontal, 0, _vertical);
        }
        playerrun.y -= g * Time.deltaTime;
        player.Move(player.transform.TransformDirection(playerrun * Time.deltaTime * speed));

        //ʹ�����������������ӽǵ���ת
        x += Input.GetAxis("Mouse X");
        y -= Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(0, x, 0);
        y = Mathf.Clamp(y, -45f, 45f);
        camera.eulerAngles = new Vector3(y, x, 0);

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
    public void PlaySound()
    {
        GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
    }
}