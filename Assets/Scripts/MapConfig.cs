using System.Collections.Generic;
using Malee;
using OneLine;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu]
    public class MapConfig : ScriptableObject //MapConfig 클래스는 ScriptableObject를 상속하여 Unity 에디터에서 사용할 수 있는 설정 파일로 만들어졌다. 이는 게임의 맵 구성을 정의하고 저장하기 위한 것
    {
        public List<NodeBlueprint> nodeBlueprints; //맵에 포함될 노드의 블루프린트 목록입니다. 각 노드 블루프린트는 실제 맵에 생성될 노드의 속성을 정의
        public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max); //맵의 그리드 너비입니다. 이 값은 numOfPreBossNodes와 numOfStartingNodes 중 더 큰 값을 사용

        [OneLineWithHeader]
        public IntMinMax numOfPreBossNodes; //보스 노드 이전에 생성될 노드의 수를 정의하는 최소 및 최대 값의 범위
        [OneLineWithHeader]
        public IntMinMax numOfStartingNodes; //작 노드의 수를 정의하는 최소 및 최대 값의 범위

        [Tooltip("경로 추가수")]
        public int extraPaths; //더 많은 경로를 생성하기 위한 추가 경로의 수
        [Reorderable]
        public ListOfMapLayers layers;

        [System.Serializable]
        public class ListOfMapLayers : ReorderableArray<MapLayer>
        {
        }
    }
}