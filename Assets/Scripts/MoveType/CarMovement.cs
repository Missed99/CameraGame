using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    //public List<GameObject> signList = new List<GameObject>();//���ת���־�ļ���

    public float speed;
    public float turnAngle = 90f;//ת��Ƕ�
    public float turnAngeleTime;//��ת����Ҫ��ʱ��
    public bool isTurn;

    public IEnumerator cardTurnDirCoroutine;

    private void Awake()
    {
        cardTurnDirCoroutine = CardTurnDirCoroutine();
    }

    public void LateUpdate()
    {
        //����������ǰ�� 
        if (isTurn) return;
        //this.transform.Translate(transform.forward * speed * Time.deltaTime);
        this.transform.Translate(-1f * Vector3.forward * speed * Time.deltaTime);
        //Debug.Log("transform.forward" + transform.forward);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Sign")
    //    {
    //        Debug.Log("��⵽��");

    //        isTurn = true;
    //        //��ʼЭ��
    //        if (cardTurnDirCoroutine == null)
    //        {
    //            cardTurnDirCoroutine = CardTurnDirCoroutine();
    //            StartCoroutine(cardTurnDirCoroutine);
    //        }
    //        else
    //        {
    //            StopCoroutine(cardTurnDirCoroutine);
    //            cardTurnDirCoroutine = null;
    //            cardTurnDirCoroutine = CardTurnDirCoroutine();
    //            StartCoroutine(cardTurnDirCoroutine);
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sign")
        {
            Debug.Log("��⵽��");

            isTurn = true;
            //��ʼЭ��
            if (cardTurnDirCoroutine == null)
            {
                cardTurnDirCoroutine = CardTurnDirCoroutine();
                StartCoroutine(cardTurnDirCoroutine);
            }
            else
            {
                StopCoroutine(cardTurnDirCoroutine);
                cardTurnDirCoroutine = null;
                cardTurnDirCoroutine = CardTurnDirCoroutine();
                StartCoroutine(cardTurnDirCoroutine);
            }
        }
    }

    //����ת���Э��
    public IEnumerator CardTurnDirCoroutine()
    {
        float num = turnAngeleTime / Time.fixedDeltaTime;
        float nextAngle = turnAngle / num;

        //yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < num; i++)
        {
            this.transform.Rotate(new Vector3(0, -nextAngle, 0));

            //yield return new WaitForFixedUpdate();
            yield return null;
        }

        isTurn = false;
    }
}
