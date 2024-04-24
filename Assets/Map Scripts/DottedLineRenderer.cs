using UnityEngine;

namespace Map
{
    public class DottedLineRenderer : MonoBehaviour //클래스는 주어진 Line Renderer 오브젝트의 렌더링된 선에 점선 효과를 추가
    {
        public bool scaleInUpdate = false;
        private LineRenderer lR;
        private Renderer rend;

        private void Start()
        {
            ScaleMaterial();
            enabled = scaleInUpdate;
        }

        public void ScaleMaterial()
        {
            lR = GetComponent<LineRenderer>();
            rend = GetComponent<Renderer>();
            rend.material.mainTextureScale =
                new Vector2(Vector2.Distance(lR.GetPosition(0), lR.GetPosition(lR.positionCount - 1)) / lR.widthMultiplier,
                    1);
        }

        private void Update()
        {
            rend.material.mainTextureScale =
                new Vector2(Vector2.Distance(lR.GetPosition(0), lR.GetPosition(lR.positionCount - 1)) / lR.widthMultiplier,
                    1);
        }
    }
}