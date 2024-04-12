using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main")]

    [SerializeField]
    private Light sl;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private CanvasGroup mainCanvas;

    [SerializeField]
    private Button settingsButton;

    [Header("Settings")]      

    [SerializeField]
    private CanvasGroup settingsCanvas;

    [SerializeField]
    private Button settingsBackButton;

    IEnumerator Start()
    {
        settingsButton.onClick.AddListener(ShowSettings);
        settingsBackButton.onClick.AddListener(ShowMain);

        settingsCanvas.gameObject.SetActive(true);
        mainCanvas.gameObject.SetActive(true);

        ShowMain();
        mainCanvas.interactable = false;
        mainCanvas.alpha = 0f;
      
        
        sl.intensity = 0.02f;
        source.time = 8f;

        yield return new WaitUntil(() => source.time >= 11.8f);

        sl.intensity = 2000f;
        Tween.LightIntensity(sl, 4.5f, 2, Ease.OutQuad);

        Tween.Custom(0f, 1, 1.5f, x => mainCanvas.alpha = x, Ease.OutQuad).OnComplete(this, x => x.mainCanvas.interactable = true);
    }

    public void ShowSettings()
    {
        mainCanvas.interactable = false;
        mainCanvas.alpha = 0f;
        mainCanvas.blocksRaycasts = false;
        

        settingsCanvas.interactable = true;
        settingsCanvas.alpha = 1f;
        settingsCanvas.blocksRaycasts = true;
    }

    public void ShowMain()
    {
        mainCanvas.interactable = true;
        mainCanvas.alpha = 1f;
        mainCanvas.blocksRaycasts = true;


        settingsCanvas.interactable = false;
        settingsCanvas.alpha = 0f;
        settingsCanvas.blocksRaycasts = false;
    }

    public async void StartNewGame()
    {
        await SceneManager.LoadSceneAsync("Loc1");
    }
}
