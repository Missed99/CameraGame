using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //[SerializeField] private GameObject eventSystem;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Application.Quit();
    }

    /*private void Start()
    {
        DontDestroyOnLoad(eventSystem);
    }*/
    private void Update()
    {
        if (GamePause.isPause == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
