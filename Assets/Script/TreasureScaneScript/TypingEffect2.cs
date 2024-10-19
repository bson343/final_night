using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingEffect2 : MonoBehaviour
{
    public TextMeshProUGUI Tx;
    private string[] texts = {
        "도대체... 왜..? 우린 아무에게도 해를 끼치지 않았어요..",
        "하나님의 명으로 우리는 재앙을 불러올 사악한 마녀들은 모두 척결해야한다.",
        "그렇군요. 이건 그대들이 불러온 재앙입니다."
    };
    public GameObject Player;
    public GameObject Paladin;
    public GameObject NextBtn;
    private int currentTextIndex = 0;

    void Start()
    {
        NextBtn.SetActive(false);
        
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
        if (Player.activeSelf)
        {
            Player.SetActive(false);
            Paladin.SetActive(true);
        }
        else
        {
            Player.SetActive(true);
            Paladin.SetActive(false);
        }

        for (int i = 0; i <= texts[currentTextIndex].Length; i++)
        {
            Tx.text = texts[currentTextIndex].Substring(0, i);
            yield return new WaitForSeconds(0.07f);
        }
        NextBtn.SetActive(true);
    }

    // NextBtn을 클릭할 때 실행될 메서드
    public void OnNextBtnClicked()
    {
        Debug.Log("NextBtn 클릭");
        NextBtn.SetActive(false); // 다음 버튼 비활성화

        // 플레이어와 팔라딘의 활성화 상태를 전환
      

        if (currentTextIndex < texts.Length - 1)
        {
            currentTextIndex++;
            StartCoroutine(_Typing()); // 다음 텍스트 타이핑 시작
        }
        else
        {
            NightSceneManager.Instance.LoadScene("NodeMap Test");
        }
    }
}
