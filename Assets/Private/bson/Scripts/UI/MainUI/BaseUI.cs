using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    private Vector3 _exitButtonOriginPos;

    [SerializeField]
    private bool isFirst;  // 무조건 처음이여야 하는 UI 
    [SerializeField]
    private bool irrevocable;  // 뒤로 갈 수 없는 UI (이벤트에서 꼭 카드를 강화하거나 제거해야하는 등)

    public virtual void Show()
    {
        if(isFirst || irrevocable)
        {
        }
        else
        {
        }
    }

    public virtual void Hide()
    {
        if(isFirst || irrevocable)
        {
        }
        else
        {
        }
    }

    public virtual void Init()
    {

    }

}
