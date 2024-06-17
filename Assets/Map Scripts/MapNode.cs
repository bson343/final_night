using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map
{
    public enum NodeStates //odeStates 열거형은 노드의 상태를 정의
    {
        Locked,// 잠김
        Visited, // 방문함
        Attainable // 도달 가능 (갈 수 있음)
    }
}

namespace Map
{
    public class MapNode : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler 
        //Unity에서 사용되는 게임 오브젝트로, UI 요소와 상호 작용할 수 있으며 이벤트에 대응하여 특정 동작을 수행가능
    {
        public SpriteRenderer sr; // Unity에서 2D 스프라이트를 렌더링하는 데 사용되는 컴포넌트
        public Image image; //Unity UI에서 이미지를 나타내는 컴포넌트
        public SpriteRenderer visitedCircle; //방문한 노드를 표시하는 데 사용되는 스프라이트 렌더러
        public Image circleImage; //노드 주변에 원 형태의 이미지를 표시하는 데 사용되는 이미지 (방문한 노드에 동그라미 쳐짐)
        public Image visitedCircleImage; //방문한 노드 주변에 표시되는 특정 이미지

        public Node Node { get; private set; } //Node 클래스의 인스턴스를 나타내는 속성으로, 외부에서 읽을 수 있지만 수정은 불가능
        public NodeBlueprint Blueprint { get; private set; } //NodeBlueprint 클래스의 인스턴스

        private float initialScale; // 초기 크기 저장 변수
        private const float HoverScaleFactor = 1.2f; //마우스를 올렸을 때 크기를 조절하기 위한 상수
        private float mouseDownTime; //마우스 버튼이 눌린 시간을 저장하는 변수

        private const float MaxClickDuration = 0.5f; //클릭한 시간이 유효한 최대 시간을 나타내는 상수

        public void SetUp(Node node, NodeBlueprint blueprint) //MapNode 클래스의 인스턴스를 설정하는 데 사용
                                                              //주어진 노드와 노드 블루프린트를 기반으로 해당 MapNode의 상태를 초기화
        {
            Node = node;
            Blueprint = blueprint;
            if (sr != null) sr.sprite = blueprint.sprite; //sr (SpriteRenderer)가 null이 아니라면, 노드의 블루프린트에 있는 스프라이트를 sr.sprite에 할당
            if (image != null) image.sprite = blueprint.sprite; //image가 null이 아니라면, 노드의 블루프린트에 있는 스프라이트를 image.sprite에 할당
            if (node.nodeType == NodeType.Boss) transform.localScale *= 1.5f; //만약 노드의 타입이 보스 노드라면, 해당 노드의 스케일을 1.5배로 증가
            if (sr != null) initialScale = sr.transform.localScale.x; //sr이 null이 아니라면, 초기 스케일을 sr.transform.localScale.x에 저장
            if (image != null) initialScale = image.transform.localScale.x; //image가 null이 아니라면, 초기 스케일을 image.transform.localScale.x에 저장

            if (visitedCircle != null) //visitedCircle가 null이 아니라면, 해당 원의 색상을 MapView.Instance.visitedColor로 설정하고 비활성화(방문한 곳의 색상으로 바뀌고 클릭 못하게)
            {
                visitedCircle.color = MapView.Instance.visitedColor;
                visitedCircle.gameObject.SetActive(false);
            }

            if (circleImage != null) //circleImage가 null이 아니라면, 해당 이미지의 색상을 MapView.Instance.visitedColor로 설정하고 비활성화
            {
                circleImage.color = MapView.Instance.visitedColor;
                circleImage.gameObject.SetActive(false);    
            }
            
            SetState(NodeStates.Locked); //노드를 잠긴 상태로 설정하기 위해 SetState(NodeStates.Locked)를 호출
        }

        public void SetState(NodeStates state) // SetState 메서드는 MapNode의 상태를 변경하는 데 사용 (방문한곳, 닫힌곳 , 갈 수 있는 곳) 
        {
            if (visitedCircle != null) visitedCircle.gameObject.SetActive(false);
            if (circleImage != null) circleImage.gameObject.SetActive(false);
            
            switch (state)
            {
                case NodeStates.Locked:
                    if (sr != null)
                    {
                        sr.DOKill();
                        sr.color = MapView.Instance.lockedColor;
                    }

                    if (image != null)
                    {
                        image.DOKill();
                        image.color = MapView.Instance.lockedColor;
                    }

                    break;
                case NodeStates.Visited:
                    if (sr != null)
                    {
                        sr.DOKill();
                        sr.color = MapView.Instance.visitedColor;
                    }
                    
                    if (image != null)
                    {
                        image.DOKill();
                        image.color = MapView.Instance.visitedColor;
                    }
                    
                    if (visitedCircle != null) visitedCircle.gameObject.SetActive(true);
                    if (circleImage != null) circleImage.gameObject.SetActive(true);
                    break;
                case NodeStates.Attainable:
                    // start pulsating from visited to locked color:
                    if (sr != null)
                    {
                        sr.color = MapView.Instance.lockedColor;
                        sr.DOKill();
                        sr.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    }
                    
                    if (image != null)
                    {
                        image.color = MapView.Instance.lockedColor;
                        image.DOKill();
                        image.DOColor(MapView.Instance.visitedColor, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null); //상태가 지정되지 않은 경우(default)에는 ArgumentOutOfRangeException를 발생
            }
        }

        public void OnPointerEnter(PointerEventData data) //마우스 포인터가 MapNode 오브젝트 위에 올라갔을 때 호출되는 Unity 이벤트 핸들러
        {
            if (sr != null)
            {
                sr.transform.DOKill();
                sr.transform.DOScale(initialScale * HoverScaleFactor, 0.3f); //마우스를 올렸을 때 해당 오브젝트의 크기를 키우는 효과
            }

            if (image != null)
            {
                image.transform.DOKill();
                image.transform.DOScale(initialScale * HoverScaleFactor, 0.3f); //마우스를 올렸을 때 해당 오브젝트의 크기를 키우는 효과
            }
        }

        public void OnPointerExit(PointerEventData data) //마우스 포인터가 MapNode 오브젝트를 벗어날 때 호출되는 Unity 이벤트 핸들러
        {
            if (sr != null)
            {
                sr.transform.DOKill();
                sr.transform.DOScale(initialScale, 0.3f); // 마우스가 벗어났을때 원래대로
            }

            if (image != null)
            {
                image.transform.DOKill();
                image.transform.DOScale(initialScale, 0.3f); //원래대로
            }
        }

        public void OnPointerDown(PointerEventData data) //마우스 버튼을 누르는 순간 호출되는 이벤트 핸들러
        {
            GlobalSoundManager.Instance.PlaySE(ESE.UIClick);
            mouseDownTime = Time.time; // 마우스 버튼을 누를때 누른 시간을 저장 (얼마나 오래 눌렀는가 계산할때 쓰인다.)
        }

        public void OnPointerUp(PointerEventData data) //마우스 버튼을 누른 후 해당 버튼을 뗄 때 호출되는 이벤트 핸들러
        {
            if (Time.time - mouseDownTime < MaxClickDuration) //약 시간 간격이 MaxClickDuration보다 작다면,
                                                              
            {
                GlobalSoundManager.Instance.PlaySE(ESE.EnterRoom);
                // 유저가 노드를 클릭했다는걸 알린다.
                MapPlayerTracker.Instance.SelectNode(this);
            }
        }

        public void ShowSwirlAnimation() //특정한 애니메이션을 사용하여 방문한 노드를 강조하는 데 사용
        {
            if (visitedCircleImage == null)
                return;

            const float fillDuration = 0.3f;
            visitedCircleImage.fillAmount = 0;

            DOTween.To(() => visitedCircleImage.fillAmount, x => visitedCircleImage.fillAmount = x, 1f, fillDuration);
        }
    }
}
