using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterOn : MonoBehaviour
{
    [SerializeField] private CanvasGroup _actInfo; //  켄버스
    // Start is called before the first frame update
    
    private void Start()
    {
        StartCoroutine(CoAppearActInfo());
    }

    private IEnumerator CoAppearActInfo()
    {
        _actInfo.gameObject.SetActive(true);

        // 1.5초간 등장
        float timeElapsed = 0;
        while (true)
        {
            _actInfo.alpha = Mathf.Lerp(0, 1, timeElapsed / 1.5f);
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 1.5f)
                break;
            yield return null;
        }

        // 1초간 유지
        yield return new WaitForSeconds(1f);

        // 1.5초간 사라짐
        timeElapsed = 0;
        while (true)
        {
            _actInfo.alpha = Mathf.Lerp(1, 0, timeElapsed / 1.5f);
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 1.5f)
                break;
            yield return null;
        }

        _actInfo.gameObject.SetActive(false);
    }
}
