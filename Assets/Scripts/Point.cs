using System;

namespace Map
{
    public class Point : IEquatable<Point>
    {
        public int x;
        public int y;

        public Point(int x, int y) //Point(int x, int y): 두 개의 정수 값을 받아서 Point 클래스의 새 인스턴스를 초기화
        {
            this.x = x;
            this.y = y;
        }

        // * 자동 생성
        public bool Equals(Point other) //두 개의 Point 객체가 같은지를 확인하는 메서드
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj) //객체가 동일한지 여부를 확인하는 메서드입니다. 이는 IEquatable<Point> 인터페이스의 구현과 함께 사용
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point)obj);
        }

        public override int GetHashCode() //객체의 해시 코드를 계산하는 메서드 , 해시 테이블과 같은 자료 구조에서 객체를 저장하고 검색할 때 사용
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }
        // * 자동 생성 종료

        public override string ToString() //객체를 문자열로 변환하는 메서드입니다. 이를 통해 좌표를 읽기 쉬운 형태로 출력
        {
            return $"({x}, {y})";
        }
    }
}
