using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.instance.isRed > 0)
            transform.DOMoveZ(-6, 1);//开门
        else
            transform.DOMoveZ(-1.75f, 1);//不动
    }
}
