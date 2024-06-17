using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChoiseScript : MonoBehaviour
{
    public void Event_One_Scene()
    {
        UserManager.Instance.UpdateMaxHP(UserManager.Instance.MaxHP+20);
        UserManager.Instance.UpdateCurrentHP(UserManager.Instance.CurrentHP + 20);
    }

    public void Event_Two_Scene()
    {
        UserManager.Instance.UpdateMaxHP(UserManager.Instance.MaxHP - 10);
        UserManager.Instance.UpdateCurrentHP(UserManager.Instance.CurrentHP - 10);
    }

    public void Event_Three_One_Scene()
    {
        UserManager.Instance.UpdateMaxHP(UserManager.Instance.MaxHP + 10);
        UserManager.Instance.UpdateCurrentHP(UserManager.Instance.CurrentHP + 10);
    }

    public void Event_Three_Two_Scene()
    {
        UserManager.Instance.UpdateCurrentSP(UserManager.Instance.CurrentSP + 10);
    }

    public void Event_Three_Three_Scene()
    {
        UserManager.Instance.UpdateMaxHP(UserManager.Instance.MaxHP + 50);
        UserManager.Instance.UpdateCurrentHP(UserManager.Instance.CurrentHP + 50);
        UserManager.Instance.UpdateCurrentSP(UserManager.Instance.CurrentSP + 50);
    }

    public void Event_Four_Scene()
    {
        UserManager.Instance.UpdateMaxHP(UserManager.Instance.MaxHP - 10);
        UserManager.Instance.UpdateCurrentHP(UserManager.Instance.CurrentHP - 10);
    }

    public void Event_Five_Scene()
    {

    }
}
