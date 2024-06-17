using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Map
{
    public class MapPlayerTracker : MonoBehaviour //게임에서 플레이어의 위치를 추적하고, 게임 맵에서 노드를 선택할 때 발생하는 동작을 처리
    {
        public bool lockAfterSelecting = false; //플레이어가 노드를 선택한 후에 잠금 여부를 결정하는 불 변수
        public float enterNodeDelay = 1f; //노드에 도착하기 전의 지연 시간을 나타냄
        public MapManager mapManager; // 맵을 관리하는 MapManager 인스턴스
        public MapView view; // 맵을 표시하는 MapView 인스턴스

        public static MapPlayerTracker Instance;

        public bool Locked { get; set; } // 현재 플레이어가 잠겨 있는지 여부를 나타내는 속성

        private void Awake()
        {
            Instance = this; //이렇게 게임 시작하기전 선언
        }

        public void SelectNode(MapNode mapNode) //주어진 MapNode를 선택하는 데 필요한 동작을 처리
        {
            if (Locked) return; //Locked 속성이 true인 경우, 즉 플레이어가 현재 잠겨 있는 상태라면, 노드 선택을 처리하지 않고 함수를 종료

            // Debug.Log("Selected node: " + mapNode.Node.point);

            if (mapManager.CurrentMap.path.Count == 0) //현재 플레이어가 선택한 노드가 없는 경우.
                                                       //이 경우 플레이어는 아직 노드를 선택하지 않았으므로, y 좌표가 0인 노드 중 하나를 선택할 수 있다.
            {
                if (mapNode.Node.point.y == 0)
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed(); // 그렇지않으면 못접근한다고 알림
            }
            else //현재 플레이어가 이미 선택한 노드가 있는 경우
            {
                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1]; //플레이어가 선택한 마지막 노드(currentPoint)를 가져와서
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point))) //연결되어 있다면 SendPlayerToNode 메소드를 호출하여 플레이어를 해당 노드로 이동
                    SendPlayerToNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed(); // 아니면 PlayWarningThatNodeCannotBeAccessed();메소드를 호출하여 접근 못한다고 알림
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private static void EnterNode(MapNode mapNode) // 플레이어를 선택한 노드로 이동시키는 역할 (여기서 코드를 수정하면 개발자가 원하는대로 노드에 따라 이벤트 수정가능)
        {
            // 당 메서드에서 노드의 블루프린트 이름에 접근할 수 있다
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);
            // 이것은 특정 유형의 노드에 따라 적절한 상황을 고려하여 적절한 장면을 로드
            // 특정 유형의 노드에 따라 맵 위에 적절한 GUI(그래픽 사용자 인터페이스)를 표시
            // GUI가 표시되는 특정 상황에서 Locked 속성을 false로 설정하여 플레이어에게 이동 및 상호작용의 자유를 제공해야 한다는 것을 개발자에게 상기
            switch (mapNode.Node.nodeType)
            {
                case NodeType.MinorEnemy: //일반몹
                    NightSceneManager.Instance.GameLoadScene("TestBattleScene");
                    break;
                case NodeType.EliteEnemy: // 엘리트 몹

                    break;
                case NodeType.RestSite: // 회복?
                    NightSceneManager.Instance.GameLoadScene("shop");
                    break;
                case NodeType.Treasure: // 마찬가지

                    break;
                case NodeType.Store: // 상점
                    NightSceneManager.Instance.GameLoadScene("shop");
                    break;
                case NodeType.Boss: // 보스

                    break;
                case NodeType.Mystery: // 이벤트 씬
                    GlobalSoundManager.Instance.FadeBGM(EBGM.EventScene, 0.2f);
                    NightSceneManager.Instance.LoadRandomScene();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayWarningThatNodeCannotBeAccessed() // 디버그 출력
        {
            Debug.Log("Selected node cannot be accessed"); // 선택한 노드에 접근할수없다.
        }
    }
}