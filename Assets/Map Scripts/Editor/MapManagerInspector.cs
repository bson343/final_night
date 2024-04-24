using UnityEditor;
using UnityEngine;

namespace Map
{
    [CustomEditor(typeof(MapManager))]

    public class MapManagerInspector : Editor //이 스크립트를 사용하면 Unity 에디터에서 MapManager 오브젝트를 선택하고 인스펙터 창을 보면 "Generate" 버튼 생성
                                              //이 버튼을 클릭하면 MapManager 클래스의 GenerateNewMap() 메서드가 실행

    {
        public override void OnInspectorGUI() //에디터의 GUI를 그리는 메서드로, 이 메서드를 오버라이드하여 사용자 지정한 인스펙터를 그린다.
        {
            DrawDefaultInspector(); //메서드를 호출하여 기본 인스펙터를 그린 후, 추가적인 GUI 요소를 그릴 수 있음.

            var myScript = (MapManager)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Generate"))
                myScript.GenerateNewMap();
        }
    }
}