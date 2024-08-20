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

    //暂停相关
    private void Update()
    {
        if (GamePause.isPause == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    //加载下一关
    public void LoadNextScene()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneIndex++;
        SceneManager.LoadScene(SceneIndex);
    }

    //退出
    public void Quit()
    {
        Application.Quit();
    }

    //通关
    public void Win()
    {
        Debug.Log("通关");
    }
}
