using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Map
{
    public class MapViewUI : MapView
    //이 클래스에서 실행 시 맵 ui를 생성해주고 삭제해주는듯하다. MapView를 상속받았다.
    {
        [Header("UI Map Settings")]
        [Tooltip("ScrollRect that will be used for orientations: Left To Right, Right To Left")]
        // 주석같은거라고 한다.
        [SerializeField] private ScrollRect scrollRectHorizontal;
        //속성(Attribute)입니다. 이를 통해 private 접근 제한자로 선언된 변수를 Inspector 창에서도 보이게 만듭니다.
        [Tooltip("ScrollRect that will be used for orientations: Top To Bottom, Bottom To Top")]
        [SerializeField] private ScrollRect scrollRectVertical;
        [Tooltip("Multiplier to compensate for larger distances in UI pixels on the canvas compared to distances in world units")]
        [SerializeField] private float unitsToPixelsMultiplier  = 10f;
        [Tooltip("Padding of the first and last rows of nodes from the sides of the scroll rect")]
        [SerializeField] private float padding;
        [Tooltip("Padding of the background from the sides of the scroll rect")]
        [SerializeField] private Vector2 backgroundPadding;
        [Tooltip("Pixels per Unit multiplier for the background image")]
        [SerializeField] private float backgroundPPUMultiplier = 1;
        [Tooltip("Prefab of the UI line between the nodes (uses scripts from Unity UI Extensions)")]
        [SerializeField] private UILineRenderer uiLinePrefab;

        /*[Header("UI Map Settings")]: Inspector에서 해당 변수들을 그룹화하는 헤더를 정의합니다.
        [Tooltip("설명")]: Inspector에서 해당 변수에 대한 툴팁(설명)을 정의합니다.
        [SerializeField]: 해당 변수를 Inspector에 노출하여 직렬화합니다. 이것은 private 변수도 Inspector에 표시할 수 있게 해줍니다.
        따라서 이러한 코드들은 Unity Editor에서의 편의성을 높이기 위해 사용되는 것이며, 게임 실행에 직접적인 영향을 미치지 않습니다. 
        게임 실행 중에는 이러한 속성들이 무시되고, Unity Editor에서만 사용됩니다.*/
        protected override void ClearMap()
        {
            scrollRectHorizontal.gameObject.SetActive(false);
            scrollRectVertical.gameObject.SetActive(false);

            foreach (var scrollRect in new []{scrollRectHorizontal, scrollRectVertical})
            foreach (Transform t in scrollRect.content)
                Destroy(t.gameObject);
            
            MapNodes.Clear();
            lineConnections.Clear();
        }

        private ScrollRect GetScrollRectForMap() 
            // 
        {
            return orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft
                ? scrollRectHorizontal
                : scrollRectVertical;
        }

        protected override void CreateMapParent()
        {
            var scrollRect = GetScrollRectForMap();
            scrollRect.gameObject.SetActive(true);
            
            firstParent = new GameObject("OuterMapParent");
            firstParent.transform.SetParent(scrollRect.content);
            firstParent.transform.localScale = Vector3.one;
            var fprt = firstParent.AddComponent<RectTransform>();
            Stretch(fprt);
            
            mapParent = new GameObject("MapParentWithAScroll");
            mapParent.transform.SetParent(firstParent.transform);
            mapParent.transform.localScale = Vector3.one;
            var mprt = mapParent.AddComponent<RectTransform>();
            Stretch(mprt);
            
            SetMapLength();
            ScrollToOrigin();
        }

        private void SetMapLength()
        {
            var rt = GetScrollRectForMap().content;
            var sizeDelta = rt.sizeDelta;
            var length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
            if (orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft)
                sizeDelta.x = length;
            else
                sizeDelta.y = length;
            rt.sizeDelta = sizeDelta;
        }

        private void ScrollToOrigin()
        {
            switch (orientation)
            {
                case MapOrientation.BottomToTop:
                    scrollRectVertical.normalizedPosition = Vector2.zero;
                    break;
                case MapOrientation.TopToBottom:
                    scrollRectVertical.normalizedPosition = new Vector2(0, 1);
                    break;
                case MapOrientation.RightToLeft:
                    scrollRectHorizontal.normalizedPosition = new Vector2(1, 0);
                    break;
                case MapOrientation.LeftToRight:
                    scrollRectHorizontal.normalizedPosition = Vector2.zero;
                    break;
                default:
                    break;
            }
        }

        private static void Stretch(RectTransform tr)
        {
            tr.localPosition = Vector3.zero;
            tr.anchorMin = Vector2.zero;
            tr.anchorMax = Vector2.one;
            tr.sizeDelta = Vector2.zero;
            tr.anchoredPosition = Vector2.zero;
        }

        protected override MapNode CreateMapNode(Node node)
        {
            var mapNodeObject = Instantiate(nodePrefab, mapParent.transform);
            var mapNode = mapNodeObject.GetComponent<MapNode>();
            var blueprint = GetBlueprint(node.blueprintName);
            mapNode.SetUp(node, blueprint);
            mapNode.transform.localPosition = GetNodePosition(node);
            return mapNode;
        }

        private Vector2 GetNodePosition(Node node)
        {
            var length = padding + Map.DistanceBetweenFirstAndLastLayers() * unitsToPixelsMultiplier;
            
            switch (orientation)
            {
                case MapOrientation.BottomToTop:
                    return new Vector2(-backgroundPadding.x / 2f, (padding - length) / 2f) +
                           node.position * unitsToPixelsMultiplier;
                case MapOrientation.TopToBottom:
                    return new Vector2(backgroundPadding.x / 2f, (length - padding) / 2f) -
                           node.position * unitsToPixelsMultiplier;
                case MapOrientation.RightToLeft:
                    return new Vector2((length - padding) / 2f, backgroundPadding.y / 2f) -
                           Flip(node.position) * unitsToPixelsMultiplier;
                case MapOrientation.LeftToRight:
                    return new Vector2((padding - length) / 2f, -backgroundPadding.y / 2f) +
                           Flip(node.position) * unitsToPixelsMultiplier;
                default:
                    return Vector2.zero;
            }
        }

        private static Vector2 Flip(Vector2 other) => new Vector2(other.y, other.x);

        protected override void SetOrientation()
        {
            // do nothing here for UI:
        }

        protected override void CreateMapBackground(Map m)
        {
            var backgroundObject = new GameObject("Background");
            backgroundObject.transform.SetParent(mapParent.transform);
            backgroundObject.transform.localScale = Vector3.one;
            var rt = backgroundObject.AddComponent<RectTransform>();
            Stretch(rt);
            rt.SetAsFirstSibling();
            rt.sizeDelta = backgroundPadding;
            
            var image = backgroundObject.AddComponent<Image>();
            image.color = backgroundColor;
            image.type = Image.Type.Sliced;
            image.sprite = background;
            image.pixelsPerUnitMultiplier = backgroundPPUMultiplier;
        }

        protected override void AddLineConnection(MapNode from, MapNode to)
        {
            if (uiLinePrefab == null) return;
            
            var lineRenderer = Instantiate(uiLinePrefab, mapParent.transform);
            lineRenderer.transform.SetAsFirstSibling();
            var fromRT = from.transform as RectTransform;
            var toRT = to.transform as RectTransform;
            var fromPoint = fromRT.anchoredPosition +
                            (toRT.anchoredPosition - fromRT.anchoredPosition).normalized * offsetFromNodes;

            var toPoint = toRT.anchoredPosition +
                          (fromRT.anchoredPosition - toRT.anchoredPosition).normalized * offsetFromNodes;

            // drawing lines in local space:
            lineRenderer.transform.position = from.transform.position +
                                              (Vector3) (toRT.anchoredPosition - fromRT.anchoredPosition).normalized *
                                              offsetFromNodes;

            // line renderer with 2 points only does not handle transparency properly:
            var list = new List<Vector2>();
            for (var i = 0; i < linePointsCount; i++)
            {
                list.Add(Vector3.Lerp(Vector3.zero, toPoint - fromPoint +
                                                    2 * (fromRT.anchoredPosition - toRT.anchoredPosition).normalized *
                                                    offsetFromNodes, (float) i / (linePointsCount - 1)));
            }
            
            Debug.Log("From: " + fromPoint + " to: " + toPoint + " last point: " + list[list.Count - 1]);

            lineRenderer.Points = list.ToArray();

            var dottedLine = lineRenderer.GetComponent<DottedLineRenderer>();
            if (dottedLine != null) dottedLine.ScaleMaterial();

            lineConnections.Add(new LineConnection(null, lineRenderer, from, to));
        }
    }
}