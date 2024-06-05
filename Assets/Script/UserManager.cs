using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UserManager Instance { get; private set; }

    // userNum을 저장할 변수 (클래스 외부에서는 값을 읽을 수 있지만 설정할 수는 없도록 제한)
    public long UserNum { get; private set; }

    // userNickname을 저장할 변수
    public string UserNickname { get; private set; }

    public int Gold { get; private set; }
    // 플레이어 골드 
    public int HP { get; private set; }
    // 플레이어 체력 
    public List<CardData> CardDeck { get; private set; }
    // 플레이어 덱
    public string Map { get; private set; }
    // 플레이어 맵 정보

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 이 객체를 새로운 씬 로드 시에도 파괴되지 않게 함
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 이미 존재하면 새로운 객체를 파괴
        }
    }

    // userNum 설정 메서드
    public void SetUserNum(long userNum)
    {
        UserNum = userNum;
    }

    // userNickname 설정 메서드
    public void SetUserNickname(string userNickname)
    {
        UserNickname = userNickname;
    }
}