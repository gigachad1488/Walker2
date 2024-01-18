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
    private Light[] lights;

    [SerializeField]
    private CanvasGroup buttonsCanvas;

    [SerializeField]
    private Image img;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Tween.LocalPositionX(sl.transform, 600, 17f);
        buttonsCanvas.alpha = 0f;
        source.time = 8f;
        yield return new WaitUntil(() => source.time >= 11.8f);
        for (int i = 0; i < lights.Length; i++) 
        {

            lights[i].intensity = 2000f;
            Tween.LightIntensity(lights[i], 73f, 2, Ease.OutQuad);                
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < lights.Length - 1; i++)
        {

            Tween.LightIntensity(lights[i], 0f, 0.5f);
        }

        Material mat = img.material;
        Tween.Custom(0, 1, 5.5f, x => buttonsCanvas.alpha = x, Ease.OutQuad);
        Tween.Custom(0, 10, 5.5f, x => mat.SetColor("_EmissionColor", Color.red * x));
    }
}
