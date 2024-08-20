using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public int objNum;
    public float num;
    public GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        if (num == 0 && Player.instance.redNum > objNum)
        {
                num += Time.deltaTime;
                transform.position += transform.right * 3f ;
            Player.instance.doorNum--;
        }
            

        if (num == 1 && Player.instance.yNum > objNum )
        {
            num += Time.deltaTime;
            transform.position += transform.right * 3f;
            Player.instance.doorNum--;
        }

        if (num == 2 && Player.instance.pNum > objNum)
        {
            num += Time.deltaTime;
            transform.position += transform.right * 3f ;
            Player.instance.doorNum--;
        }
        if(Player.instance.doorNum==0)
        {
            Destroy(p, 2);
            Player.instance.PlaySound("Success");
            StartCoroutine(Delay());
            Player.instance.doorNum = 1;
        }
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        int SceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneIndex < 6)
        {
            SceneIndex++;
            SceneManager.LoadScene(SceneIndex);
        }
        else if (SceneIndex == 6)
        {
            SceneManager.LoadScene(0);
        }
    }
}
