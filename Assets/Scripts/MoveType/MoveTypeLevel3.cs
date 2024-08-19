using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTypeLevel3 : MonoBehaviour
{
    public float jumpHeight;

    public float jumpSpeed;

    public Transform checkPointTransform;

    public float sphereRadius;

    public bool isGrounded;
    public Vector3 dir;
    public float g = -9.8f;

    private void Update()
    {
        isGrounded = CheckIsGrounded();

        //�ŵ�
        if (isGrounded)
        {
            dir.y = Mathf.Sqrt(jumpHeight * -2f * g);
        }

        dir.y -= g * Time.deltaTime;

        this.transform.Translate(dir * jumpSpeed * Time.deltaTime);
    }

    //����Ƿ��ڵ���
    private bool CheckIsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(checkPointTransform.position, sphereRadius);//��ⷶΧ����ײ������
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

    //���������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //��������ӻ�
        Gizmos.DrawWireSphere(checkPointTransform.position, sphereRadius);
    }
}
