using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int SceneIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    //��ͣ���
    private void Update()
    {
        if (GamePause.isPause == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    //������һ��
    public void LoadNextScene()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
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

    //�˳�
    public void Quit()
    {
        Application.Quit();
    }

    //ͨ��
    public void Win()
    {
        Debug.Log("ͨ��");
    }
}
