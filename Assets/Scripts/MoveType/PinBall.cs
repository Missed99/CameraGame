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

    public float changeScaleAmount;//控制scale的量
    private Vector3 originalScale;//原本的scale
    public Vector3 targetScale;//目标大小
    public Vector3 currentScale;//当前大小
    public float changeSpeed_Y;//缩小时需求的速度

    private IEnumerator increaseCoroutine;
    private IEnumerator shrinkCoroutine;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dir = this.transform.TransformDirection(Vector3.forward);
        rb.velocity = -dir * speed;
        lastDir = rb.velocity;
        //Debug.Log("初始化lastDir:" + lastDir);

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
            //开始增大的协程
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
                //开始增大的协程
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
            //开始缩小的协程
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

            forceDir = collision.contacts[0].normal;//碰撞点的法线
            rb.AddForce(forceDir * force, ForceMode.Impulse);//瞬间冲力

        }
    }

    //增大的协程
    public IEnumerator IncreaseCoroutine()
    {
        //yield return new WaitForSeconds(0.5f);//延迟0.5S
        isTouchPlane = false;
        isTouchWall = false;
        while (currentScale.magnitude <= targetScale.magnitude)
        {
            this.transform.localScale += Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //减小的协程
    public IEnumerator ShrinkCoroutine()
    {
        //yield return new WaitForSeconds(1.5f);//延迟1.5S
        while (currentScale.magnitude >= originalScale.magnitude)
        {
            this.transform.localScale -= Vector3.one * changeScaleAmount * 0.5f;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }
}
