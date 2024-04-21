using OneLine;
using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class MapLayer
    {
        [Tooltip("Default node for this map layer. If Randomize Nodes is 0, you will get this node 100% of the time")]
        public NodeType nodeType; //해당 층에서 사용될 기본 노드의 타입을 나타내는 열거형 변수입니다. 이는 해당 층에서 가장 흔히 사용되는 노드의 타입을 설정
       
        [OneLineWithHeader] public FloatMinMax distanceFromPreviousLayer; //[OneLineWithHeader] 에디터에서 해당 변수를 한 줄로 표시하는 역할을 한다.
        [Tooltip("이 계층의 노드 사이의 거리")]
        public float nodesApartDistance;
        [Tooltip("randomizePosition이 0으로 설정되면, 층에 있는 노드들은 직선으로 배치됩니다. 즉, 모든 노드들이 일직선으로 나열 ,1에 가까워질수록, 노드들의 위치는 더 많은 무작위화가 발생합니다. 즉, 노드들이 더 무작위로 배치")] //이 변수는 해당 층에서 노드들의 위치를 얼마나 무작위로 배치할지를 결정
        [Range(0f, 1f)] public float randomizePosition;
        [Tooltip("이러한 변수는 해당 층에서 생성되는 노드들의 다양성을 조절하는 데 사용됩니다. 예를 들어, 값이 0.5로 설정되면, 기본(default) 노드와 다른 무작위 노드를 각각 50%의 확률로 얻을 수 있다.")]
        [Range(0f, 1f)] public float randomizeNodes;
    }
} // [OneLineWithHeader] public IntMinMax numOfNodes;