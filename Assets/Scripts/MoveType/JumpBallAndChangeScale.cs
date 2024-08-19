using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpBallAndChangeScale : MonoBehaviour
{
    public float jumpHeight;
    public float jumpSpeed;

    public Transform groundCheckTransform;//射线的起始点
    //public float sphereRadius;

    public float RayLength;

    public bool isGrounded;
    public Vector3 dir;
    public float g = -10f;

    public float changeScaleAmount;//控制scale的量
    private Vector3 originalScale;//原本的scale
    public Vector3 targetScale;//目标大小
    public Vector3 currentScale;//当前大小
    private float timer;//计时器
    public bool hadIncreased;//是否已经增大

    private IEnumerator increaseCoroutine;
    private IEnumerator shrinkCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        currentScale = transform.localScale;

        increaseCoroutine = IncreaseCoroutine();
        shrinkCoroutine = ShrinkCoroutine();

        timer = Time.time;
    }


    private void FixedUpdate()
    {
        Debug.DrawRay(groundCheckTransform.position, Vector3.down * RayLength, Color.blue);

        isGrounded = CheckIsGrounded();

        //着地
        if (isGrounded)
        {
            dir.y = Mathf.Sqrt(jumpHeight * -2f * g);

            //开始增大的协程
            if(increaseCoroutine == null)
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

        dir.y += g * Time.deltaTime; 
        this.transform.Translate(dir * jumpSpeed * Time.deltaTime);
    }

    //增大的协程
    public IEnumerator IncreaseCoroutine()
    {
        yield return new WaitForSeconds(0.5f);//延迟0.5S
        while (currentScale.magnitude <=  targetScale.magnitude)
        {
            this.transform.localScale += Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //减小的协程
    public IEnumerator ShrinkCoroutine()
    {
        yield return new WaitForSeconds(1.5f);//延迟1.5S
        while (currentScale.magnitude >= originalScale.magnitude)
        {
            this.transform.localScale -= Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //检测是否在地面
    private bool CheckIsGrounded()
    {
        RaycastHit hit;

        // 向下打出射线
        if (Physics.Raycast(groundCheckTransform.position, Vector3.down, out hit, RayLength))
        {
                return true;
        }
        return false;
    }

 





}
