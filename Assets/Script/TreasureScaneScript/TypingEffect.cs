using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI Tx;
    private string m_Text = "석상이 말을 건다... 무슨 말을 하려는 걸까...";
    private string s_Text = "휴식을 취하면 체력을 회복시켜주지... 필요없다면 그냥 가도 좋다...";
    public GameObject Statue;
    public GameObject Player;
    public GameObject NextBtn;
    public GameObject Rest;
    public GameObject Out;

    void Start()
    {
        NextBtn.SetActive(false);
        Rest.SetActive(false);
        Out.SetActive(false);
        Statue.SetActive(false);
        Tx.text = "";

        // NextBtn의 Button 컴포넌트에 OnClick 이벤트를 연결합니다.
        Button nextButtonComponent = NextBtn.GetComponent<Button>();
        if (nextButtonComponent != null)
        {
            nextButtonComponent.onClick.AddListener(OnNextBtnClicked);
        }
        else
        {
            Debug.LogError("NextBtn에 Button 컴포넌트가 없습니다!");
        }

        StartCoroutine(_Typing());
    }

    IEnumerator _Typing()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= m_Text.Length; i++) // <= 로 변경하여 전체 텍스트가 나오도록 수정
        {
            Tx.text = m_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);
        }
        NextBtn.SetActive(true);
    }

    IEnumerator s_typing()
    {
        Tx.text = "";
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= s_Text.Length; i++) // <= 로 변경하여 전체 텍스트가 나오도록 수정
        {
            Tx.text = s_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);
        }
        Rest.SetActive(true);
        Out.SetActive(true);
    }

    // NextBtn을 클릭할 때 실행될 메서드
    public void OnNextBtnClicked()
    {
        Debug.Log("NextBtn 클릭");
        NextBtn.SetActive(false); // 다음 버튼 비활성화
        Player.SetActive(false);
        Statue.SetActive(true);
        StartCoroutine(s_typing()); // s_typing 코루틴 실행
    }
}
