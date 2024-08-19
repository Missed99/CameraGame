using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int objNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.isRed > objNum)
            transform.DOMoveZ(-6, 1);//开门
        else
            transform.DOMoveZ(-1.75f, 1);//不动
    }
}
