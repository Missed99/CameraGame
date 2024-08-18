using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePause : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    public static bool isPause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;

            canvas.enabled = isPause;

            if (isPause)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
                canvasGroup.DOFade(0f, 0.25f).SetUpdate(true);
            }
        }
    }
    public void GameContinue()
    {
        Time.timeScale = 1f;
        isPause = false;
        canvas.enabled = isPause;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        isPause = false;
        canvas.enabled = isPause;

        SceneManager.LoadScene(0);
    }
}
