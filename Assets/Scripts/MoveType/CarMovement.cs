using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    //public List<GameObject> signList = new List<GameObject>();//存放转向标志的集合

    public float speed;
    public float turnAngle = 90f;//转向角度
    public float turnAngeleTime;//旋转所需要的时间
    public bool isTurn;

    public IEnumerator cardTurnDirCoroutine;

    private void Awake()
    {
        cardTurnDirCoroutine = CardTurnDirCoroutine();
    }

    public void LateUpdate()
    {
        //车辆不断向前进 
        if (isTurn) return;
        //this.transform.Translate(transform.forward * speed * Time.deltaTime);
        this.transform.Translate(-1f * Vector3.forward * speed * Time.deltaTime);
        //Debug.Log("transform.forward" + transform.forward);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Sign")
    //    {
    //        Debug.Log("检测到了");

    //        isTurn = true;
    //        //开始协程
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
            Debug.Log("检测到了");

            isTurn = true;
            //开始协程
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

    //车辆转弯的协程
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
