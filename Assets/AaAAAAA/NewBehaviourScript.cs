using UnityEngine;

public class OpacityController : MonoBehaviour
{
    public Material material;  // 할당할 Material

    // 투명도를 조절하는 함수
    public void SetOpacity(float opacity)
    {
        Color color = material.color;
        color.a = Mathf.Clamp01(opacity); // 0에서 1 사이로 클램프
        material.color = color;
    }
}