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

        //着地
        if (isGrounded)
        {
            dir.y = Mathf.Sqrt(jumpHeight * -2f * g);
        }

        dir.y -= g * Time.deltaTime;

        this.transform.Translate(dir * jumpSpeed * Time.deltaTime);
    }

    //检测是否在地面
    private bool CheckIsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(checkPointTransform.position, sphereRadius);//检测范围内碰撞的物体
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Plane"))
            {
                Debug.Log("检测到了地面");
                return true;
            }
        }
        return false;
    }

    //地面检测调试
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        //地面检测可视化
        Gizmos.DrawWireSphere(checkPointTransform.position, sphereRadius);
    }
}
