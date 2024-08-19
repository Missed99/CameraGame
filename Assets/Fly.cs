using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Transform[] targets;
    public bool isStart;
    int i = 0;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isStart)
        {
            if (i == targets.Length - 1)
                transform.RotateAround(targets[targets.Length-1].position, transform.up, 90 * Time.deltaTime);
            else
                transform.position += (targets[i].position - transform.position).normalized * Time.deltaTime*speed;

        }
        if (Vector3.Distance(targets[i].position, transform.position) < 0.5f)
        {
            if(i < targets.Length-1)
                i++;
        }
            
    }
}
