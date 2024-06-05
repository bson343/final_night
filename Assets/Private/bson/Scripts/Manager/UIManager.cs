using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IRegisterable
{
    private Stack<BaseUI> stackUI = new Stack<BaseUI>();

    public BaseUI CurrentUI
    {
        get
        {
            if (stackUI.Count == 0)
                return null;
            return stackUI.Peek();
        }
    }

    public void Init()
    {
        
    }

    public void ClearUI()
    {
        stackUI = new Stack<BaseUI>();
    }

    // ui를 보여줍니다.
    public void ShowUI(BaseUI ui)
    {
        if(stackUI.Count > 0)
        {
            stackUI.Peek().gameObject.SetActive(false);
            stackUI.Peek().Hide();
        }
        stackUI.Push(ui);
        if(ui != null)
        {
            stackUI.Peek().gameObject.SetActive(true);
            stackUI.Peek().Show();
            stackUI.Peek().Init();
        }
    }

    // ui를 닫습니다.
    public void PopUI()
    {
        if (stackUI.Count > 0)
        {
            stackUI.Peek().gameObject.SetActive(false);
            stackUI.Peek().Hide();
        }
        stackUI.Pop();
        if (stackUI.Count > 0)
        {
            stackUI.Peek().gameObject.SetActive(true);
            stackUI.Peek().Show();
        }
    }

    public void ShowThisUI(BaseUI ui)
    {
        PopAllUI();
        ShowUI(ui);
    }

    // 모든 UI를 종료합니다.
    public void PopAllUI()
    {
        while(stackUI.Count > 0)
        {
            PopUI();
        }
    }

}
