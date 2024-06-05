using UnityEngine;
using UnityEngine.UI;

public class rewardPopup : MonoBehaviour
{
    public GameObject popup; // 팝업 창
    public Button button1;   // 첫 번째 버튼
    public Button button2;   // 두 번째 버튼

    private void Start()
    {
            popup.SetActive(false);
    }
    void Update()
    {
        CheckButtons();
    }

    void CheckButtons()
    {
        // 두 버튼이 모두 비활성화되었는지 확인
        if (!button1.interactable && !button2.interactable)
        {
            ClosePopup();
        }
    }

    void ClosePopup()
    {
        // 팝업 창을 비활성화하거나 닫는 로직
        popup.SetActive(false);
    }
}