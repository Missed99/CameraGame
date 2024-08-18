using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove : MonoBehaviour
{
    public float moveSpeed=1f;
    public bool isMove;
    public Transform point;
    public Vector3 initPos;
    public Vector3 targetPos;
    public bool state;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        targetPos = point.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(targetPos, transform.position) < 0.5f)
        {
            state = !state;
                
        }
        if(state)
        {
            targetPos = initPos;
        }
        else
        {
            targetPos = point.position;

        }
        if(isMove)
        transform.position += (targetPos - transform.position).normalized * Time.deltaTime* moveSpeed;
    }
}
