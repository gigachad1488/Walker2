using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rofls : MonoBehaviour
{
    [SerializeField]
    private Light sl;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private CanvasGroup buttonsCanvas;

    [SerializeField]
    private Image img;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        sl.intensity = 0.02f;
        buttonsCanvas.alpha = 0f;
        source.time = 8f;
        yield return new WaitUntil(() => source.time >= 11.8f);

        sl.intensity = 2000f;
        Tween.LightIntensity(sl, 4.5f, 2, Ease.OutQuad);

        buttonsCanvas.gameObject.SetActive(true);
        buttonsCanvas.interactable = false;
    
        Tween.Custom(0f, 1, 2.5f, x => buttonsCanvas.alpha = x, Ease.OutQuad);

        yield return new WaitForSeconds(1f);

        buttonsCanvas.interactable = true;
    }
}
