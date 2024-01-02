using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PrimeTween;
using System;

public class LevelManager : MonoBehaviour
{
    public string sceneName;

    public PlayerHealth player;

    public Canvas endCanvas;
    private CanvasGroup endCanvasGroup;

    public static LevelManager instance;

    private void Start()
    {
        endCanvasGroup = endCanvas.GetComponent<CanvasGroup>();
        endCanvas.gameObject.SetActive(false);
        player.onDeath += Player_onDeath;

        if (instance == null)
        {
            instance = this;
        }

        Time.timeScale = 1.0f;
    }

    private void Player_onDeath(Vector3 position)
    {
        endCanvasGroup.alpha = 0;
        endCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;

        Sequence.Create()
            .Chain(Tween.Custom(0, 1, 1f, x => endCanvasGroup.alpha = x))
            .Group(Tween.Custom(1, 0, 1f, x => Time.timeScale = x));
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
