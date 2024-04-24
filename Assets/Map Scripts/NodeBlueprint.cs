using UnityEngine;

namespace Map
{
    public enum NodeType //각 유형은 게임 맵에서 특정 노드를 식별하는 데 사용
    {
        MinorEnemy,// 일반 몬스터
        EliteEnemy, // 엘리트 몬스터
        RestSite, // 회복하는 곳(휴식)
        Treasure, // 보물
        Store, // 상점
        Boss, // 보스
        Mystery // 랜덤 이벤트
    }
}

namespace Map
{
    [CreateAssetMenu]
    public class NodeBlueprint : ScriptableObject
    {
        public Sprite sprite;
        public NodeType nodeType;
    }
}