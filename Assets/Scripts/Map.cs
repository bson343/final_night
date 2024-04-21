using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Map
{
    public class Map //Map 클래스를 나타냅니다. 이 클래스는 게임 맵에 대한 정보를 저장하고 조작하는 데 사용
    {
        public List<Node> nodes;
        public List<Point> path;
        public string bossNodeName;
        public string configName; // similar to the act name in Slay the Spire

        public Map(string configName, string bossNodeName, List<Node> nodes, List<Point> path)  //맵의 설정 이름(configName), 보스 노드의 이름(bossNodeName),
                                                                                                //노드 목록(nodes), 그리고 경로(path)를 받아서 Map 클래스의 새 인스턴스를 초기화
        {
            this.configName = configName;
            this.bossNodeName = bossNodeName;
            this.nodes = nodes;
            this.path = path;
        }

        public Node GetBossNode() //맵에서 보스 노드를 가져온다. nodes 목록에서 첫 번째로 발견되는 보스 노드를 반환
        {
            return nodes.FirstOrDefault(n => n.nodeType == NodeType.Boss);
        }

        public float DistanceBetweenFirstAndLastLayers() //첫 번째 층과 마지막 층 사이의 거리를 계산 , 이는 보스 노드와 첫 번째 층 노드의 위치를 사용하여 계산
        {
            var bossNode = GetBossNode();
            var firstLayerNode = nodes.FirstOrDefault(n => n.point.y == 0);

            if (bossNode == null || firstLayerNode == null)
                return 0f;

            return bossNode.position.y - firstLayerNode.position.y;
        }

        public Node GetNode(Point point) //주어진 좌표와 일치하는 노드를 가져옵니다
        {
            return nodes.FirstOrDefault(n => n.point.Equals(point));
        }

        public string ToJson() //객체를 JSON 문자열로 변환하여 반환 , 이를 통해 맵의 정보를 JSON 형식으로 저장하거나 전송
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}