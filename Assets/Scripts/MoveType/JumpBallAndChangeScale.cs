using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpBallAndChangeScale : MonoBehaviour
{
    public float jumpHeight;
    public float jumpSpeed;

    public Transform groundCheckTransform;//���ߵ���ʼ��
    //public float sphereRadius;

    public float RayLength;

    public bool isGrounded;
    public Vector3 dir;
    public float g = -10f;

    public float changeScaleAmount;//����scale����
    private Vector3 originalScale;//ԭ����scale
    public Vector3 targetScale;//Ŀ���С
    public Vector3 currentScale;//��ǰ��С
    private float timer;//��ʱ��
    public bool hadIncreased;//�Ƿ��Ѿ�����

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

        //�ŵ�
        if (isGrounded)
        {
            dir.y = Mathf.Sqrt(jumpHeight * -2f * g);

            //��ʼ�����Э��
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

        dir.y += g * Time.deltaTime; 
        this.transform.Translate(dir * jumpSpeed * Time.deltaTime);
    }

    //�����Э��
    public IEnumerator IncreaseCoroutine()
    {
        yield return new WaitForSeconds(0.5f);//�ӳ�0.5S
        while (currentScale.magnitude <=  targetScale.magnitude)
        {
            this.transform.localScale += Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //��С��Э��
    public IEnumerator ShrinkCoroutine()
    {
        yield return new WaitForSeconds(1.5f);//�ӳ�1.5S
        while (currentScale.magnitude >= originalScale.magnitude)
        {
            this.transform.localScale -= Vector3.one * changeScaleAmount;
            currentScale = this.transform.localScale;
            yield return null;
        }
    }

    //����Ƿ��ڵ���
    private bool CheckIsGrounded()
    {
        RaycastHit hit;

        // ���´������
        if (Physics.Raycast(groundCheckTransform.position, Vector3.down, out hit, RayLength))
        {
                return true;
        }
        return false;
    }

 





}
