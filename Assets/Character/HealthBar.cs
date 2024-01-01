using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image fillImage;

    [SerializeField]
    private Image impactImage;

    private Tween tween;

    public void Change(float value)
    {
        if (fillImage.fillAmount > value)
        {
            //StartCoroutine(ImpactLerp(value));

            fillImage.fillAmount = value;
            tween.Stop();
            tween = Tween.UIFillAmount(impactImage, value, 0.2f, Ease.InQuint);
        }
    }

    public void Set(float fill, float impact)
    {
        fillImage.fillAmount = fill;
        impactImage.fillAmount = impact;
    }

    private IEnumerator ImpactLerp(float value)
    {
        fillImage.fillAmount = value;
        while (impactImage.fillAmount > fillImage.fillAmount)
        {
            float v = Mathf.Lerp(fillImage.fillAmount, impactImage.fillAmount, fillImage.fillAmount - impactImage.fillAmount * Time.deltaTime);
            impactImage.fillAmount = v;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Visible(bool visible)
    {
        fillImage.gameObject.SetActive(visible);
        fillImage.gameObject.SetActive(visible);
    }
}
