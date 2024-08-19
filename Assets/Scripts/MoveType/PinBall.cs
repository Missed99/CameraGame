using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PinBall : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 dir;
    private Vector3 lastDir;
    public float speed;

    public float jumpHeight;
    private Vector3 forceDir;
    public float force;

    public bool isTouchPlane;
    public bool isTouchWall;

    public float changeScaleAmount;//����scale����
    private Vector3 originalScale;//ԭ����scale
    public Vector3 targetScale;//Ŀ���С
    public Vector3 currentScale;//��ǰ��С
    public float changeSpeed_Y;//��Сʱ������ٶ�

    private IEnumerator increaseCoroutine;
    private IEnumerator shrinkCoroutine;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dir = this.transform.TransformDirection(Vector3.forward);
        rb.velocity = -dir * speed;
        lastDir = rb.velocity;
        //Debug.Log("��ʼ��lastDir:" + lastDir);

        originalScale = transform.localScale;
        currentScale = transform.localScale;

        increaseCoroutine = IncreaseCoroutine();
        shrinkCoroutine = ShrinkCoroutine();
    }

    private void LateUpdate()
    {
        lastDir = rb.velocity;
        //Debug.Log("lastDir" + lastDir);
        //Debug.Log(rb.velocity.y);

        if (isTouchPlane)
        {
            //��ʼ�����Э��
            if (increaseCoroutine == null)
            {
                increaseCoroutine = IncreaseCoroutine();
                StartCoroutine(increaseCoroutine);
            }
            else
            {
                StopCoroutine(increaseCoroutine);
                increaseCoroutine = null;
                increaseCoroutine = IncreaseCoroutine();
                StartCoroutine(increaseCoroutine);
            }
        }

        if (isTouchWall)
        {
            if (currentScale.magnitude < targetScale.magnitude)
            {
                //��ʼ�����Э��
                if (increaseCoroutine == null)
                {
                    increaseCoroutine = IncreaseCoroutine();
                    StartCoroutine(increaseCoroutine);
                }
                else
                {
                    StopCoroutine(increaseCoroutine);
                    increaseCoroutine = null;
                    increaseCoroutine = IncreaseCoroutine();
                    StartCoroutine(increaseCoroutine);
                }
            }
        }

        if (lastDir.y <= changeSpeed_Y)
        {
            //��ʼ��С��Э��
            if (shrinkCoroutine == null)
            {
                shrinkCoroutine = ShrinkCoroutine();
                StartCoroutine(shrinkCoroutine);
            }
            else
            {
                StopCoroutine(shrinkCoroutine);
                shrinkCoroutine = null;
                shrinkCoroutine = ShrinkCoroutine();
                StartCoroutine(shrinkCoroutine);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            isTouchPlane = true;

            //rb.velocity = new Vector3(lastDir.x, lastDir.y + jumpHeight, lastDir.z);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpHeight, rb.velocity.z);
            //Debug.Log("rb.velocity" + rb.velocity);

        }

        if (collision.gameObject.tag == "Wall")
        {
            isTouchWall = true;

            forceDir = collision.contacts[0].normal;//��ײ��ķ���
            rb.AddForce(forceDir * force, ForceMode.Impulse);//˲�����

        }
    }

    //�����Э��
    public IEnumerator IncreaseCoroutine()
    {
        //yield return new WaitForSeconds(0.5f);//�ӳ�0.5S
        isTouchPlane = false;
        isTouchWall = false;
        while (currentScale.magnitude <= targetScale.magnitude)
        {
            this.transform.localScale += Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //��С��Э��
    public IEnumerator ShrinkCoroutine()
    {
        //yield return new WaitForSeconds(1.5f);//�ӳ�1.5S
        while (currentScale.magnitude >= originalScale.magnitude)
        {
            this.transform.localScale -= Vector3.one * changeScaleAmount * 0.5f;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }
}
