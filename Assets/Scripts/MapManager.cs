using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;

namespace Map
{
    public class MapManager : MonoBehaviour // 게임 맵을 관리하고 생성하며 플레이어의 진행 상황을 저장하는 데 사용된다.
    {
        public MapConfig config;
        public MapView view;

        public Map CurrentMap { get; private set; }

        private void Start()
        {
            if (PlayerPrefs.HasKey("Map"))
            {
                var mapJson = PlayerPrefs.GetString("Map"); // PlayerPrefs에 "Map"이라는 키가 있는지 확인
                var map = JsonConvert.DeserializeObject<Map>(mapJson); //만약 "Map"이라는 키가 있다면, 저장된 맵 데이터를 가져온다. 가져온 맵 데이터를 Deserialize하여 Map 객체로 변환
                // using this instead of .Contains()
                if (map.path.Any(p => p.Equals(map.GetBossNode().point))) //맵 데이터에 보스 노드에 대한 경로가 포함되어 있는지 확인
                {
                    // 플레이어가 이미 보스에 도달했을 때 새로운 맵을 생성해야 함
                    GenerateNewMap(); // 맵 생성 메소드
                }
                else
                {
                    CurrentMap = map;
                    // 플레이어가 아직 보스에 도달하지 않았을 때 현재 맵을 로드해야 함
                    view.ShowMap(map);
                }
            }
            else //PlayerPrefs에 "Map"이라는 키가 없다면, 새로운 맵을 생성 
            {
                GenerateNewMap();
            }
        }

        public void GenerateNewMap() // 새로운 맵 생성 메소드
        {
            var map = MapGenerator.GetMap(config); //MapGenerator 클래스의 GetMap 메소드를 호출하여 새로운 맵을 생성, 이때, MapConfig 객체(config)를 사용하여 맵을 생성
            CurrentMap = map; //변수에 새로 생성된 맵을 할당합니다. 이렇게 함으로써 게임에서 현재 사용 중인 맵을 추적
            Debug.Log(map.ToJson()); //생성된 맵을 JSON 형식의 문자열로 변환하여 디버그 로그에 출력
            view.ShowMap(map); //view 객체의 ShowMap 메소드를 호출하여 생성된 맵을 게임 화면에 표시
        }

        public void SaveMap() //현재 맵을 저장
        {
            if (CurrentMap == null) return; //현재 맵이 null인지 확인합니다. 만약 현재 맵이 없으면 아무 작업도 수행하지 않고 종료

            var json = JsonConvert.SerializeObject(CurrentMap, Formatting.Indented, //현재 맵을 JSON 형식으로 직렬화. 이때 JsonConvert.SerializeObject() 메소드를 사용하여 CurrentMap 객체를 JSON 문자열로 변환
                new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            PlayerPrefs.SetString("Map", json); //JSON 문자열로 변환된 맵을 PlayerPrefs에 저장. 이를 위해 PlayerPrefs.SetString() 메소드를 사용하여 "Map" 키에 해당하는 값을 설정
            PlayerPrefs.Save(); // 플레이어 프리팹에 저장
        }

        private void OnApplicationQuit() //어플리케이션이 종료될 때 호출되는 Unity 메소드
        {
            SaveMap(); // 맵 저장
        }
    }
}
