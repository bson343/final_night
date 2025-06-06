using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterIndent : MonoBehaviour
{
    private Character _character;

    [SerializeField] private Transform indentParent;
    [SerializeField] private IndentObject indentPrefab;

    public List<IndentObject> indentList = new List<IndentObject>();

    [SerializeField]
    public IndentData[] indentData;

    public void Init(Character character)
    {
        _character = character;
    }

    public void ClearIndentList()
    {
        while(indentList.Count != 0)
        {
            Destroy(indentList[0].gameObject);
            indentList.RemoveAt(0);
        }
        indentList = new List<IndentObject>();
    }

    public void AddIndent(IndentData indentData, int value) //왜인지 이코드 전체가 잘 적용안됨
    {
        if (_character == null)
        {
            Debug.LogError("_character가 초기화되지 않았습니다. Init() 메서드가 호출되었는지 확인하세요.");
            return;
        }

        if (indentData == null)
        {
            Debug.LogError("indentData가 null입니다. 올바른 IndentData를 전달했는지 확인하세요.");
            return;
        }

        _character.indent[(int)indentData.indent] = true;

        // 이미 있는 Indent라면 turn만 증가
        for (int i = 0; i < indentList.Count; i++)
        {
            if (indentList[i].indentData == indentData)
            {
                indentList[i].AddTurn(value);
                Visualize();
                return;
            }
        }

        indentList.Add(Instantiate(indentPrefab, indentParent));
        indentList[indentList.Count - 1].Init(indentData, value);

        Visualize();
    }

    // 시각화 => indent를 얻을 때나 턴이 시작되면 실행해줌
    public void Visualize()
    {
        for(int i = 0; i < indentList.Count; i++)
        {
            indentList[i].UpdateIndent();
        }
    }

    public void BurnDamageUpDate() //  화상데미지    
    {
        List<IndentObject> list = new List<IndentObject>();

        for (int i = 0; i < indentList.Count; i++)
        {
            if (indentList[i].indentData.isTurn)
            {
                       
                if (indentList[i].indentData.indent == EIndent.Burn)
                {
                    int damage = indentList[i].turn;     
                    Debug.Log(damage);
                    _character.Hit(damage, _character);
                }
            }
        }
    }

    public void DeathCountOver() // 헌신 디버프   
    {
        List<IndentObject> list = new List<IndentObject>();

        for (int i = 0; i < indentList.Count; i++)
        {
            if (indentList[i].indentData.isTurn)
            {

                if (indentList[i].indentData.indent == EIndent.DeathCount)
                {
                    int damage = 30;
                    Debug.Log(damage);
                    _character.Hit(damage, _character);
                }
            }
        }
    }

    public void UpdateIndents()
    {
        // 0이 되면 그 indent destroy와
        // indent 배열 false

        List<IndentObject> list = new List<IndentObject>();

        for(int i = 0; i < indentList.Count; i++)
        {
            if(indentList[i].indentData.isTurn)
                indentList[i].turn--;
        }

        for(int i = 0; i < indentList.Count; i++)
        {
            if(indentList[i].indentData.isTurn && indentList[i].turn <= 0)
            {
                _character.indent[(int)indentList[i].indentData.indent] = false;
                list.Add(indentList[i]);
            }
        }

        while (list.Count > 0)
        {
            IndentObject temp = list[0];

            // 원래 리스트에서 temp에 대한 조건 확인
            if (temp.indentData.indent == EIndent.DeathCount)
            {
                _character.Hit(4444, _character); // DeathCount일 경우 데미지 처리
            }

            // 리스트에서 제거 및 게임 오브젝트 삭제
            indentList.Remove(temp); // 원래 리스트에서 제거
            list.Remove(temp);       // 복사 리스트에서 제거
            Destroy(temp.gameObject); // 게임 오브젝트 파괴
        }
    }

    /*public void RemoveIndent(EIndent indentType)
    {
        for (int i = 0; i < indentList.Count; i++)
        {
            if (indentList[i].indentData.indent == indentType)
            {
                IndentObject temp = indentList[i];
                indentList.Remove(temp);
                Destroy(temp.gameObject);
            }
        }
    }*/


    // 특정 EIndent 타입에 맞는 IndentData를 반환하는 메서드
    public IndentData GetIndentData(EIndent indentType)
    {
        foreach (var indent in indentData)
        {
            if (indent.indent == indentType)
            {
                return indent;
            }
        }
        return null; // 원하는 타입이 없으면 null 반환
    }
}
