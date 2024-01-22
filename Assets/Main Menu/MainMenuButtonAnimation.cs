using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2 initPos;
    private Vector2 movePos;

    private Tween moveTween;

    public Image borderImage;

    void Start()
    {
        borderImage = gameObject.GetComponentsInChildren<Image>()[1];

        initPos = borderImage.GetComponent<RectTransform>().localPosition;

        movePos = new Vector2(initPos.x + 50f, initPos.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tween.StopAll(moveTween);
        moveTween = Tween.LocalPosition(borderImage.transform, movePos, 0.2f, Ease.OutSine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tween.StopAll(moveTween);
        moveTween = Tween.LocalPosition(borderImage.transform, initPos, 0.2f, Ease.OutSine);
    }
}
