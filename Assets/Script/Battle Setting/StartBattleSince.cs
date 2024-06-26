using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattleSince : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // "OuterMapParent" 오브젝트를 찾습니다.
        GameObject outerMapParent = GameObject.Find("OuterMapParent");

        // 오브젝트가 존재하면 비활성화합니다.
        if (outerMapParent != null)
        {
            outerMapParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("OuterMapParent 오브젝트를 찾을 수 없습니다.");
        }
    }

    void OnApplicationQuit()
    {
        UserManager.Instance.SetCurrentHP(0);
    }
}
