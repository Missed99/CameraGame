using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorPoen : MonoBehaviour
{
    private bool isOpen;

    public Transform doorTransform;


    private void Update()
    {
        if (isOpen)
        {
            doorTransform.position += doorTransform.right * 3f;

            //通关提示
            GameManager.Instance.Win();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isOpen = true;
        }
    }

}
