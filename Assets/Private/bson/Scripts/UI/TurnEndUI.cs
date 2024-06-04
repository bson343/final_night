using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurnEndUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Image image;
    private string title;

    [SerializeField]
    private Color lockSpriteColor, originSpriteColor;
    
    [SerializeField]
    private GameObject hoverTurnEnd;

    [SerializeField]
    private Text turnEndText;

    private bool isActive = true;
   

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        title = "턴 종료(E)";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 액티브시 호버하면 불들어오고 팁 생김
        if(isActive)
        {
            hoverTurnEnd.SetActive(true);
        }
        else
        {
            hoverTurnEnd.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTurnEnd.SetActive(false);
    }

    public void ActiveButton()
    {
        isActive = true;

        turnEndText.text = "턴 종료";
        image.color = originSpriteColor;
    }

    public void OnClickButtonEvent()
    {
        if(isActive)
        {
            isActive = false;

            hoverTurnEnd.SetActive(false);

            turnEndText.text = "적 턴";

            image.color = lockSpriteColor;
        }
    }
}
