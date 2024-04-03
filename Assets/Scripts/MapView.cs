using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map // map에 성격을 가지고있는것들을 모아둔 네임스페이스
{
    public class MapView : MonoBehaviour // 맵을 보여주는 역할을 하는 클래스
    {
        public enum MapOrientation // 열거형(또는 열거형 형식)은 기본 정수 숫자 형식의 명명된 상수 집합에 의해 정의되는 값 형식입니다.
        {
            BottomToTop, //아무 설정을 안할경우INT값 0이 들어가있다.
            TopToBottom,
            RightToLeft,
            LeftToRight
        }

        public MapManager mapManager; //public 접근 제한자로 선언된 MapManager 유형의 mapManager라는 변수를 정의합니다.
        public MapOrientation orientation; 
        //이 변수는 지도의 방향을 나타내기 위해 사용될 것으로 예상됩니다. 이 변수를 통해 현재 지도가 어떤 방향으로 표시되는지를 나타내는 값을 저장할 수 있습니다.

        [Tooltip(
            "지도를 구성하는 데 사용될 수 있는 Assets 폴더에서 모든 MapConfig 스크립터블 오브젝트의 목록입니다." +
            "Slay The Spire의 Acts와 유사한 것으로, 일반적인 레이아웃과 보스 종류를 정의합니다.")]
        public List<MapConfig> allMapConfigs;
        //"다른 클래스에서 접근할 수 있는 리스트 변수인 'allMapConfigs'는 'MapConfig' 타입의 요소들을 저장합니다."
        //이 변수는 아마도 게임에서 지도 설정을 관리하는 데 사용될 것으로 예상됩니다.
        public GameObject nodePrefab; // "Inspector에서 접근 가능한 'nodePrefab' 변수는 게임 오브젝트 타입의 프리팹을 나타냅니다."
        [Tooltip("맵의 시작/끝 노드의 화면 가장자리로부터 얼마나 떨어져있는지 저장할 수 있는 오프셋")]
        public float orientationOffset;
        [Header("백그라운드 세팅")]
        [Tooltip("만약 배경 스프라이트가 널(null)이라면, 배경이 표시되지 않습니다.")]
        public Sprite background; // 배경 스프라이트
        public Color32 backgroundColor = Color.white; // 흰색으로 설정된 백그라운드 색상 Inspector 창에서 색상 변경가능

        public float xSize; // float 타입의 x 크기 변수
        public float yOffset; // float 타입의 y 크기 변수
        [Header("선 설정")]
        public GameObject linePrefab; // "Inspector에서 접근 가능한 'linePrefab' 변수는 게임 오브젝트 타입의 프리팹을 나타냅니다."
        [Tooltip("선의 점 개수는 부드러운 색상 그라데이션을 얻으려면 2보다 커야 합니다.")]
        [Range(3, 10)] // 이 어트리뷰트는 Inspector에서 해당 변수를 조절할 때 허용되는 범위를 지정합니다.
        public int linePointsCount = 10;
        [Tooltip("선 시작점부터 노드까지의 거리")]
        public float offsetFromNodes = 0.5f; // 노드 거리
        [Header("색상")]
        [Tooltip("도착한 노드의 색상")]
        public Color32 visitedColor = Color.white;
        [Tooltip("아직 도달하지않은 노드의 색상")]
        public Color32 lockedColor = Color.gray;
        [Tooltip("방문한 경로의 색상")]
        public Color32 lineVisitedColor = Color.white;
        [Tooltip("방문하지 않은 노드의 색상")]
        public Color32 lineLockedColor = Color.gray;

        protected GameObject firstParent; //게임 오브젝트의 첫 번째 부모를 나타냅니다.
        protected GameObject mapParent;  //게임 오브젝트의 지도 관련 부모를 나타냅니다.
        private List<List<Point>> paths;
        //"클래스 내부에서만 접근할 수 있는 paths 변수는 2차원 리스트로, 다중 경로를 저장하는 데 사용됩니다."
        //이 변수는 여러 경로를 관리하고 각 경로는 여러 점(Point)으로 이루어져 있을 것으로 예상됩니다.
        private Camera cam; //cam 변수는 카메라 객체를 참조합니다.
        // ALL nodes:
        public readonly List<MapNode> MapNodes = new List<MapNode>();  // 맵의 노드 , 읽기전용 , 한번초기화된 다음엔 값을 변경할 수 없음
        protected readonly List<LineConnection> lineConnections = new List<LineConnection>(); // 노드 사이의 커넥션, 읽기전용 , 한번초기화된 다음엔 값을 변경할 수 없음

        public static MapView Instance; // : MapView 타입의 전역변수 Instance 생성 아마 MapView 자주 사용되기때문에 static으로 선언한게 아닌가싶음.
        // MapView를 하나만 쓰고싶거나

        public Map Map { get; protected set; }
        // "다른 클래스에서 접근 가능한 Map 속성은 Map 타입의 객체를 나타내며, 해당 객체의 값을 읽을 수 있지만 외부에서는 값을 설정할 수 없습니다.
        // 값은 클래스 내부 또는 해당 클래스를 상속한 파생 클래스에서만 설정할 수 있습니다."

        private void Awake() // 항상 Start 함수 전에 호출되며 프리팹이 인스턴스화 된 직후에 호출됩니다.
        {
            Instance = this; // 싱글톤 패턴 구현을 워해 사용된듯함 따라서 이 스크립트의 인스턴스를 다른 곳에서 참조할 때 사용될 것
            cam = Camera.main; // 메인카메라 참조
        }

        protected virtual void ClearMap() // virtual 함수 자식클래스에서 재정의 가능
        {
            if (firstParent != null)
                Destroy(firstParent); // firstParent가 널이 아니라면 firstParent를 파괴한다. 

            MapNodes.Clear(); //MapNodes에 저장된 모든 노드를 제거
            lineConnections.Clear(); // lineConnections에 저장된 모든 요소제거
        }

        public virtual void ShowMap(Map m)
        {
            if (m == null) // 만약 지도가 null이라면, 경고 메시지를 출력하고 함수를 종료합니다.
            {
                Debug.LogWarning("Map was null in MapView.ShowMap()");
                return;
            }

            Map = m; // 전달된 지도를 m에 할당합니다.

            ClearMap(); // 요소 초기화

            CreateMapParent(); // 지도 부모 오브젝트 생성

            CreateNodes(m.nodes); // 지도에 있는 노드 생성

            DrawLines(); // 노드 간의 연결 선을 그립니다.

            SetOrientation(); // 지도 방향 설정

            ResetNodesRotation(); // 노드들의 회전 설정

            SetAttainableNodes(); // 도달 가능한 노드 설정

            SetLineColors(); // 선의 색상 설정

            CreateMapBackground(m); // 지도의 배경 설정
        }

        protected virtual void CreateMapBackground(Map m) 
            // 게임 지도의 배경 생성하고 설정 , 하위 클래스에서 오버라이딩 가능하도록 virtual 키워드로 선언 
        {
            if (background == null) return; // 배경 스프라이트 값이 null 일 경우 함수 종료

            var backgroundObject = new GameObject("Background"); // 새로운 게임 오브젝트 생성
            backgroundObject.transform.SetParent(mapParent.transform); // 배경 오브젝트의 부모를 mapParent로 설정, 이렇게 함으로써 배경이 지도의 일부로서 위치 가능
            var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);  // MapNodes 리스트에서 보스 노드를 찾는다.
            var span = m.DistanceBetweenFirstAndLastLayers(); // 지도의 첫번째와 마지막 레이어 사이의 거리를 계산
            backgroundObject.transform.localPosition = new Vector3(bossNode.transform.localPosition.x, span / 2f, 0f); 
            backgroundObject.transform.localRotation = Quaternion.identity; // 배경 오브젝트의 로컬 회전을 기본 값인 Quaternion.identity로 설정
            var sr = backgroundObject.AddComponent<SpriteRenderer>(); //배경 오브젝트에 SpriteRenderer 컴포넌트를 추가 , 이걸통해 배경에 스프라이트를 그릴 수 있다.
            sr.color = backgroundColor; // 배경 색상 변수로 설정
            sr.drawMode = SpriteDrawMode.Sliced; // 스프라이트 그리기 모드를 슬라이스 모드로 설정
            sr.sprite = background; // 배경 스프라이트 설정
            sr.size = new Vector2(xSize, span + yOffset * 2f); // 배경 스프라이트 사이즈 설정
        }

        protected virtual void CreateMapParent() // 지도의 부모 객체 생성 메소드 ,지도의 스크롤이나 충돌 처리 등과 관련된 기능을 담당
        {
            firstParent = new GameObject("OuterMapParent"); // 게임지도의 외부부모객체
            mapParent = new GameObject("MapParentWithAScroll"); // 지도의 부모객체 스크롤선언
            mapParent.transform.SetParent(firstParent.transform); //mapParent 오브젝트를 firstParent 오브젝트의 자식으로 설정
            var scrollNonUi = mapParent.AddComponent<ScrollNonUI>(); //mapParent 오브젝트에 ScrollNonUI 컴포넌트를 추가
            scrollNonUi.freezeX = orientation == MapOrientation.BottomToTop || orientation == MapOrientation.TopToBottom; 
            //scrollNonUi 컴포넌트의 freezeX 속성을 설정 ,이 속성은 스크롤의 X 축을 얼릴지 여부를 결정
            scrollNonUi.freezeY = orientation == MapOrientation.LeftToRight || orientation == MapOrientation.RightToLeft;
            //scrollNonUi 컴포넌트의 freezeY 속성을 설정. 이 속성은 스크롤의 Y 축을 얼릴지 여부를 결정
            var boxCollider = mapParent.AddComponent<BoxCollider>(); //mapParent 오브젝트에 BoxCollider 컴포넌트를 추가 , 게임지도에 대한 충돌처리나 상호작용에 사용될듯
            boxCollider.size = new Vector3(100, 100, 1); // boxCollider 사이즈 설정
        }

        protected void CreateNodes(IEnumerable<Node> nodes) //  지도에 노드를 생성 ,foreach문을 통해 Node 배열을 순회, IEnumerable을 구현해야함.
        {
            foreach (var node in nodes) //nodes 열거형에 대해 반복
            {
                var mapNode = CreateMapNode(node); //현재 반복되고 있는 node를 사용하여 지도 노드를 생성 ,CreateMapNode() 메서드를 호출하여 node를 기반으로 새로운 MapNode 객체를 생성
                MapNodes.Add(mapNode); //생성된 지도 노드를 MapNodes 리스트에 추가 ,이 리스트는 현재 지도에 있는 모든 노드를 추적하는 데 사용
            }
        }

        protected virtual MapNode CreateMapNode(Node node) //Node 객체를 기반으로 MapNode를 생성하는 CreateMapNode() 메서드를 정의
        {
            var mapNodeObject = Instantiate(nodePrefab, mapParent.transform); //nodePrefab을 복제하여 새로운 게임 오브젝트를 생성
            var mapNode = mapNodeObject.GetComponent<MapNode>(); //게임 오브젝트에서 MapNode 컴포넌트를 가져온다.
            var blueprint = GetBlueprint(node.blueprintName);
            mapNode.SetUp(node, blueprint); // 노드 설정
            mapNode.transform.localPosition = node.position; // 노드 위치 설정
            return mapNode; //생성된 노드 반환
        }

        public void SetAttainableNodes()
        {
            // 먼저, 모든 노드를 잠기게설정
            foreach (var node in MapNodes) // MapNodes를 모두 돈다
                node.SetState(NodeStates.Locked); //상태를 잠긴상태로

            if (mapManager.CurrentMap.path.Count == 0) // 만약 이동하지않았다면
            {
                // 이 지도에서 아직 이동하지않았다면, 전체 첫 번째 노드를 모두 이동 가능하도록
                foreach (var node in MapNodes.Where(n => n.Node.point.y == 0))
                    node.SetState(NodeStates.Attainable); // 모두 이동가능하게
            }
            else
            {
                // 지도에서 방문했다면 먼저 경로에 방문한걸 표시
                foreach (var point in mapManager.CurrentMap.path) // 먼저이동한 경로
                {
                    var mapNode = GetNode(point); // 경로들 포인트 반환
                    if (mapNode != null) // 지도를 가져왔다면
                        mapNode.SetState(NodeStates.Visited); // 지도 상태를 방문한것들로
                }

                var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1]; // 현재 경로의 마지막 노드
                var currentNode = mapManager.CurrentMap.GetNode(currentPoint); // 그 노드의 포인트( 마지막에 방문한거)

                // 현재 위치에서 갈 수 있는 노드를 표시 
                foreach (var point in currentNode.outgoing)
                {
                    var mapNode = GetNode(point);
                    if (mapNode != null)
                        mapNode.SetState(NodeStates.Attainable);
                }
            }
        }

        public virtual void SetLineColors() // 선 색 설정
        {
            // 모든 선을 방문하지않은 선의 색으로 변경
            foreach (var connection in lineConnections) //위에서 리스트로 선언한 lineConnections을 순회한다.
                connection.SetColor(lineLockedColor); //  모든 선들을 잠겨있는선의 색상으로 설정한다.

            // 경로의 일부인 모든 선을 방문한 색상으로 설정합니다
            // 지도에서 아직 안움직인 경우, 메소드를 종료합니다.
            if (mapManager.CurrentMap.path.Count == 0) 
                return;

            // 어떤 경우에도, 최종 노드에서 나가는 연결을 보이는/도달 가능한 색으로 표시합니다.
            var currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1]; // 현재경로의 마지막 포인트를 currentPoint 변수에 준다.
            var currentNode = mapManager.CurrentMap.GetNode(currentPoint);

            foreach (var point in currentNode.outgoing)
            {
                var lineConnection = lineConnections.FirstOrDefault(conn => conn.from.Node == currentNode &&
                                                                            conn.to.Node.point.Equals(point));
                lineConnection?.SetColor(lineVisitedColor);
            }

            if (mapManager.CurrentMap.path.Count <= 1) return;

            for (var i = 0; i < mapManager.CurrentMap.path.Count - 1; i++)
            {
                var current = mapManager.CurrentMap.path[i];
                var next = mapManager.CurrentMap.path[i + 1];
                var lineConnection = lineConnections.FirstOrDefault(conn => conn.@from.Node.point.Equals(current) &&
                                                                            conn.to.Node.point.Equals(next));
                lineConnection?.SetColor(lineVisitedColor);
            }
        }

        protected virtual void SetOrientation()
        {
            var scrollNonUi = mapParent.GetComponent<ScrollNonUI>();
            var span = mapManager.CurrentMap.DistanceBetweenFirstAndLastLayers();
            var bossNode = MapNodes.FirstOrDefault(node => node.Node.nodeType == NodeType.Boss);
            Debug.Log("Map span in set orientation: " + span + " camera aspect: " + cam.aspect);

            // setting first parent to be right in front of the camera first:
            firstParent.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
            var offset = orientationOffset;
            switch (orientation)
            {
                case MapOrientation.BottomToTop:
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.yConstraints.max = 0;
                        scrollNonUi.yConstraints.min = -(span + 2f * offset);
                    }
                    firstParent.transform.localPosition += new Vector3(0, offset, 0);
                    break;
                case MapOrientation.TopToBottom:
                    mapParent.transform.eulerAngles = new Vector3(0, 0, 180);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.yConstraints.min = 0;
                        scrollNonUi.yConstraints.max = span + 2f * offset;
                    }
                    // factor in map span:
                    firstParent.transform.localPosition += new Vector3(0, -offset, 0);
                    break;
                case MapOrientation.RightToLeft:
                    offset *= cam.aspect;
                    mapParent.transform.eulerAngles = new Vector3(0, 0, 90);
                    // factor in map span:
                    firstParent.transform.localPosition -= new Vector3(offset, bossNode.transform.position.y, 0);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.xConstraints.max = span + 2f * offset;
                        scrollNonUi.xConstraints.min = 0;
                    }
                    break;
                case MapOrientation.LeftToRight:
                    offset *= cam.aspect;
                    mapParent.transform.eulerAngles = new Vector3(0, 0, -90);
                    firstParent.transform.localPosition += new Vector3(offset, -bossNode.transform.position.y, 0);
                    if (scrollNonUi != null)
                    {
                        scrollNonUi.xConstraints.max = 0;
                        scrollNonUi.xConstraints.min = -(span + 2f * offset);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawLines()
        {
            foreach (var node in MapNodes)
            {
                foreach (var connection in node.Node.outgoing)
                    AddLineConnection(node, GetNode(connection));
            }
        }

        private void ResetNodesRotation()
        {
            foreach (var node in MapNodes)
                node.transform.rotation = Quaternion.identity;
        }

        protected virtual void AddLineConnection(MapNode from, MapNode to)
        {
            if (linePrefab == null) return;

            var lineObject = Instantiate(linePrefab, mapParent.transform);
            var lineRenderer = lineObject.GetComponent<LineRenderer>();
            var fromPoint = from.transform.position +
                            (to.transform.position - from.transform.position).normalized * offsetFromNodes;

            var toPoint = to.transform.position +
                          (from.transform.position - to.transform.position).normalized * offsetFromNodes;

            // drawing lines in local space:
            lineObject.transform.position = fromPoint;
            lineRenderer.useWorldSpace = false;

            // line renderer with 2 points only does not handle transparency properly:
            lineRenderer.positionCount = linePointsCount;
            for (var i = 0; i < linePointsCount; i++)
            {
                lineRenderer.SetPosition(i,
                    Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
            }

            var dottedLine = lineObject.GetComponent<DottedLineRenderer>();
            if (dottedLine != null) dottedLine.ScaleMaterial();

            lineConnections.Add(new LineConnection(lineRenderer, null, from, to));
        }

        protected MapNode GetNode(Point p)
        {
            return MapNodes.FirstOrDefault(n => n.Node.point.Equals(p));
        }

        protected MapConfig GetConfig(string configName)
        {
            return allMapConfigs.FirstOrDefault(c => c.name == configName);
        }

        protected NodeBlueprint GetBlueprint(NodeType type)
        {
            var config = GetConfig(mapManager.CurrentMap.configName);
            return config.nodeBlueprints.FirstOrDefault(n => n.nodeType == type);
        }

        protected NodeBlueprint GetBlueprint(string blueprintName) //주어진 블루프린트 이름에 해당하는 노드 블루프린트를 반환
        {
            var config = GetConfig(mapManager.CurrentMap.configName); // 현재맵의 설정을 가져온다.
            return config.nodeBlueprints.FirstOrDefault(n => n.name == blueprintName); //람다식 n의 name 속성이 blueprintName 과 일치하는지 확인 맞다면 찾은 요소를 리턴
        }
    }
}
