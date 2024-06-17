using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect2 : MonoBehaviour
{
    public TextMeshProUGUI Tx;
    private string M_Text = "앞에 상자 하나가 보인다.. 열어볼까?..";
    private string M2_Text = "골드를 획득했다.";
    public GameObject Player;
    public GameObject Open;
    public GameObject Out;
    public AudioClip a;
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 컴포넌트를 추가합니다.
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = a;

        Open.SetActive(false);
        Out.SetActive(false);
        Tx.text = "";
        StartCoroutine(_Typing());
    }

    IEnumerator _Typing()
    {
        yield return new WaitForSeconds(1.5f);

        // 오디오 재생을 시작합니다.
        audioSource.Play();

        for (int i = 0; i <= M_Text.Length; i++)
        {
            Tx.text = M_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);

            // 첫 번째 텍스트 출력 후에 오디오 재생을 시작합니다.
            if (i == 0)
            {
                audioSource.Play();
            }
        }

        // 오디오 재생을 중지합니다.
        audioSource.Stop();

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

        // 오디오 재생을 시작합니다.
        audioSource.Play();

        for (int i = 0; i <= M2_Text.Length; i++)
        {
            Tx.text = M2_Text.Substring(0, i);
            yield return new WaitForSeconds(0.07f);

            // 첫 번째 텍스트 출력 후에 오디오 재생을 시작합니다.
            if (i == 0)
            {
                audioSource.Play();
            }
        }

        // 오디오 재생을 중지합니다.
        audioSource.Stop();
    }
}
