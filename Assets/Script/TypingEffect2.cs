using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingEffect2 : MonoBehaviour
{
    public TextMeshProUGUI Tx;
    private string M_Text = "앞에 상자 하나가 보인다.. 열어볼까?..";
    private string M2_Text = "골드를 획득했다.";
    public GameObject Player;
    public GameObject Open;
    public GameObject Out;

    void Start()
    {
        Open.SetActive(false);
        Out.SetActive(false);
        Tx.text = "";
        StartCoroutine(_Typing());
    }

    IEnumerator _Typing()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= M_Text.Length; i++)
        {
            Tx.text = M_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);
        }
        Open.SetActive(true);
        Out.SetActive(true);
    }

    // Open 버튼을 클릭할 때 실행될 메서드
    public void OnOpenClicked()
    {
        Debug.Log("상자 열기 클릭됨");
        StartCoroutine(OpenChest());
    }

    IEnumerator OpenChest()
    {
        Open.SetActive(false);
        Tx.text = "";
        yield return new WaitForSeconds(3f);
        for (int i = 0; i <= M2_Text.Length; i++)
        {
            Tx.text = M2_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);
        }
    }
}
