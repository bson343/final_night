using UnityEngine;
using System.Collections;

public class FadeOutEffect : MonoBehaviour
{
    

    private SpriteRenderer spriteRenderer; // SpriteRenderer 컴포넌트
    public float duration = 2f; // 페이드아웃에 걸리는 시간 (초)

    private void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옴
        spriteRenderer = GetComponent<SpriteRenderer>();

        // SpriteRenderer가 없을 경우 경고 출력
        if (spriteRenderer == null)
        {
            Debug.LogWarning("이 게임 오브젝트에는 SpriteRenderer가 없습니다.");
            return;
        }

        // 페이드아웃 효과 시작
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // 초기 색상과 알파 값을 가져옴
        Color color = spriteRenderer.color;
        float startAlpha = color.a;

        // 페이드아웃 효과 진행
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            // 점진적으로 Alpha 값을 줄임
            color.a = Mathf.Lerp(startAlpha, 0, t / duration);
            spriteRenderer.color = color;
            yield return null;
        }

        // 최종 Alpha 값을 0으로 설정
        color.a = 0;
        spriteRenderer.color = color;
    }
}
