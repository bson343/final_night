using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSE : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalSoundManager.Instance.PlaySE(ESE.UIClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GlobalSoundManager.Instance.PlaySE(ESE.UIHover);
    }
}
