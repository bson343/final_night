using UnityEngine;

public class animation2 : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();  // Animator 컴포넌트를 가져옵니다.
    }

    void Update()
    {
        // 'a' 키를 누르면 애니메이션 트리거 설정
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("a");  // 'a'라는 트리거를 활성화합니다.
            Debug.Log("'a' 애니메이션 재생!");
            animator.SetTrigger("back");
        }
    }
}
