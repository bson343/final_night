using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public static class MapGenerator
    {
        private static MapConfig config;

        private static readonly List<NodeType> RandomNodes = new List<NodeType>
        {NodeType.Mystery, NodeType.Store, NodeType.Treasure, NodeType.MinorEnemy, NodeType.RestSite};

        private static List<float> layerDistances;
        private static List<List<Point>> paths;
        // ALL nodes by layer:
        private static readonly List<List<Node>> nodes = new List<List<Node>>();

        public static Map GetMap(MapConfig conf) //맵 구성을 받아들여서 실제 맵을 생성하고 반환
        {
            if (conf == null)
            {
                Debug.LogWarning("Config was null in MapGenerator.Generate()");
                return null;
            }

            config = conf;
            nodes.Clear();

            GenerateLayerDistances(); ////게임 맵의 각 층 간의 거리를 생성하여 layerDistances 리스트에 저장

            for (var i = 0; i < conf.layers.Count; i++)
                PlaceLayer(i);

            GeneratePaths(); //이 메서드는 맵의 경로를 생성하는 역할

            RandomizeNodePositions();

            SetUpConnections();

            RemoveCrossConnections();

            // 연결된 모든 노드를 선택합니다:
            var nodesList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

            // 맵의 보스 레벨의 이름을 랜덤하게 선택
            var bossNodeName = config.nodeBlueprints.Where(b => b.nodeType == NodeType.Boss).ToList().Random().name;
            return new Map(conf.name, bossNodeName, nodesList, new List<Point>());
        }

        private static void GenerateLayerDistances() //게임 맵의 각 층 간의 거리를 생성하여 layerDistances 리스트에 저장
        {
            layerDistances = new List<float>();
            foreach (var layer in config.layers)
                layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
        }

        private static float GetDistanceToLayer(int layerIndex) //각 층까지의 누적 거리를 계산할 수 있습니다. 이는 맵 생성 과정에서 각 층의 위치를 결정하거나 노드 간의 연결을 설정할 때 사용될 수 있다.
        {
            if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

            return layerDistances.Take(layerIndex + 1).Sum();
        }

        private static void PlaceLayer(int layerIndex) //주어진 층에 대해 노드를 배치하고, 각 노드의 위치와 타입을 설정합니다. 이 메서드는 맵 생성 프로세스의 한 단계로써, 각 층에 노드를 배치하는 역할을 수행
        {
            var layer = config.layers[layerIndex];
            var nodesOnThisLayer = new List<Node>();

            // offset of this layer to make all the nodes centered:
            var offset = layer.nodesApartDistance * config.GridWidth / 2f;

            for (var i = 0; i < config.GridWidth; i++)
            {
                var nodeType = Random.Range(0f, 1f) < layer.randomizeNodes ? GetRandomNode() : layer.nodeType;
                var blueprintName = config.nodeBlueprints.Where(b => b.nodeType == nodeType).ToList().Random().name;
                var node = new Node(nodeType, blueprintName, new Point(i, layerIndex))
                {
                    position = new Vector2(-offset + i * layer.nodesApartDistance, GetDistanceToLayer(layerIndex))
                };
                nodesOnThisLayer.Add(node);
            }

            nodes.Add(nodesOnThisLayer);
        }

        private static void RandomizeNodePositions() //맵의 각 노드의 위치를 일정한 범위 내에서 무작위로 조정하여 다양성을 추가.
                                                     //이것은 맵을 생성할 때 노드의 배치를 더 자연스럽고 다양하게 만들어준다.
        {
            for (var index = 0; index < nodes.Count; index++)
            {
                var list = nodes[index];
                var layer = config.layers[index];
                var distToNextLayer = index + 1 >= layerDistances.Count
                    ? 0f
                    : layerDistances[index + 1];
                var distToPreviousLayer = layerDistances[index];

                foreach (var node in list)
                {
                    var xRnd = Random.Range(-1f, 1f);
                    var yRnd = Random.Range(-1f, 1f);

                    var x = xRnd * layer.nodesApartDistance / 2f;
                    var y = yRnd < 0 ? distToPreviousLayer * yRnd / 2f : distToNextLayer * yRnd / 2f;

                    node.position += new Vector2(x, y) * layer.randomizePosition;
                }
            }
        }

        private static void SetUpConnections() //각 경로에서 인접한 노드들 사이의 연결을 설정
        {
            foreach (var path in paths)
            {
                for (var i = 0; i < path.Count - 1; ++i)
                {
                    var node = GetNode(path[i]);
                    var nextNode = GetNode(path[i + 1]);
                    node.AddOutgoing(nextNode.point);
                    nextNode.AddIncoming(node.point);
                }
            }
        }

        private static void RemoveCrossConnections() //격자 모양의 맵에서 교차된 연결을 관리하여 맵의 노드 간 더 자연스러운 연결 구조를 유지 ,(부자연스럽게 얽힌 연결을 관리 해준다.)
        {
            for (var i = 0; i < config.GridWidth - 1; ++i)
                for (var j = 0; j < config.layers.Count - 1; ++j)
                {
                    var node = GetNode(new Point(i, j));
                    if (node == null || node.HasNoConnections()) continue;
                    var right = GetNode(new Point(i + 1, j));
                    if (right == null || right.HasNoConnections()) continue;
                    var top = GetNode(new Point(i, j + 1));
                    if (top == null || top.HasNoConnections()) continue;
                    var topRight = GetNode(new Point(i + 1, j + 1));
                    if (topRight == null || topRight.HasNoConnections()) continue;

                    // Debug.Log("Inspecting node for connections: " + node.point);
                    if (!node.outgoing.Any(element => element.Equals(topRight.point))) continue;
                    if (!right.outgoing.Any(element => element.Equals(top.point))) continue;

                    // Debug.Log("Found a cross node: " + node.point);

                    // we managed to find a cross node:
                    // 1) add direct connections:
                    node.AddOutgoing(top.point);
                    top.AddIncoming(node.point);

                    right.AddOutgoing(topRight.point);
                    topRight.AddIncoming(right.point);

                    var rnd = Random.Range(0f, 1f);
                    if (rnd < 0.2f)
                    {
                        // remove both cross connections:
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                    else if (rnd < 0.6f)
                    {
                        // a) 
                        node.RemoveOutgoing(topRight.point);
                        topRight.RemoveIncoming(node.point);
                    }
                    else
                    {
                        // b) 
                        right.RemoveOutgoing(top.point);
                        top.RemoveIncoming(right.point);
                    }
                }
        }

        private static Node GetNode(Point p) //GetNode 메서드는 주어진 좌표에 해당하는 노드를 검색하여 반환합니다. 이 메서드는 맵 생성 프로세스 중에 노드를 참조할 때 사용
        {
            if (p.y >= nodes.Count) return null;
            if (p.x >= nodes[p.y].Count) return null;

            return nodes[p.y][p.x];
        }

        private static Point GetFinalNode() //맵에서 최종 노드의 좌표를 결정
        {
            var y = config.layers.Count - 1; // 먼저, 최종 노드가 배치될 층을 결정하기 위해 config.layers.Count - 1 값을 사용
            if (config.GridWidth % 2 == 1) //맵의 너비가 홀수인지 확인 맞으면 최종 노드를 중앙에 설치
                return new Point(config.GridWidth / 2, y);

            return Random.Range(0, 2) == 0 //너비가 짝수인 경우, 최종 노드를 중앙에 배치하는 것이 불가능 ,  두 가지 가능한 위치 중 하나를 무작위로 선택하여 최종 노드를 배치
                ? new Point(config.GridWidth / 2, y)
                : new Point(config.GridWidth / 2 - 1, y);
        }

        private static void GeneratePaths() //이 메서드는 맵의 경로를 생성하는 역할
        {
            var finalNode = GetFinalNode();
            paths = new List<List<Point>>();
            var numOfStartingNodes = config.numOfStartingNodes.GetValue();
            var numOfPreBossNodes = config.numOfPreBossNodes.GetValue();

            var candidateXs = new List<int>();
            for (var i = 0; i < config.GridWidth; i++)
                candidateXs.Add(i);

            candidateXs.Shuffle();
            var startingXs = candidateXs.Take(numOfStartingNodes);
            var startingPoints = (from x in startingXs select new Point(x, 0)).ToList();

            candidateXs.Shuffle();
            var preBossXs = candidateXs.Take(numOfPreBossNodes);
            var preBossPoints = (from x in preBossXs select new Point(x, finalNode.y - 1)).ToList();

            int numOfPaths = Mathf.Max(numOfStartingNodes, numOfPreBossNodes) + Mathf.Max(0, config.extraPaths);
            for (int i = 0; i < numOfPaths; ++i)
            {
                Point startNode = startingPoints[i % numOfStartingNodes];
                Point endNode = preBossPoints[i % numOfPreBossNodes];
                var path = Path(startNode, endNode);
                path.Add(finalNode);
                paths.Add(path);
            }
        }

        // Generates a random path bottom up.
        private static List<Point> Path(Point fromPoint, Point toPoint) //주어진 시작점과 끝점 사이에 있는 경로를 생성하여 반환 (경로 랜덤 생성)
        {
            int toRow = toPoint.y;
            int toCol = toPoint.x;

            int lastNodeCol = fromPoint.x;

            var path = new List<Point> { fromPoint };
            var candidateCols = new List<int>();
            for (int row = 1; row < toRow; ++row)
            {
                candidateCols.Clear();

                int verticalDistance = toRow - row;
                int horizontalDistance;

                int forwardCol = lastNodeCol;
                horizontalDistance = Mathf.Abs(toCol - forwardCol);
                if (horizontalDistance <= verticalDistance)
                    candidateCols.Add(lastNodeCol);

                int leftCol = lastNodeCol - 1;
                horizontalDistance = Mathf.Abs(toCol - leftCol);
                if (leftCol >= 0 && horizontalDistance <= verticalDistance)
                    candidateCols.Add(leftCol);

                int rightCol = lastNodeCol + 1;
                horizontalDistance = Mathf.Abs(toCol - rightCol);
                if (rightCol < config.GridWidth && horizontalDistance <= verticalDistance)
                    candidateCols.Add(rightCol);

                int RandomCandidateIndex = Random.Range(0, candidateCols.Count);
                int candidateCol = candidateCols[RandomCandidateIndex];
                var nextPoint = new Point(candidateCol, row);

                path.Add(nextPoint);

                lastNodeCol = candidateCol;
            }

            path.Add(toPoint);

            return path;
        }

        private static NodeType GetRandomNode() //무작위로 노드 유형을 반환
        {
            return RandomNodes[Random.Range(0, RandomNodes.Count)];
        }
    }
}