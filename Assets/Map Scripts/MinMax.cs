using UnityEngine;

namespace Map
{
    [System.Serializable]
    public class FloatMinMax //Random.Range 함수를 사용하여 min과 max 사이의 난수를 생성
    {
        public float min;
        public float max;

        public float GetValue()
        {
            return Random.Range(min, max);
        }
    }
}

namespace Map
{
    [System.Serializable]
    public class IntMinMax //min과 max를 나타내는 두 개의 public 필드와, 해당 범위 내에서 무작위로 int 값을 생성하는 GetValue 메서드를 제공
    {
        public int min;
        public int max;

        public int GetValue()
        {
            return Random.Range(min, max + 1);
        }
    }
}
