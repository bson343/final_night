using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomCard : MonoBehaviour
{
    private List<int> generatedCardIds = new List<int>();

    public int GetUniqueRandomCardId() // 메서드를 public으로 변경
    {
        List<int> availableCardIds = new List<int>();
        for (int i = 1; i <= 16; i++) // 카드 ID 범위 (1~16)
        {
            if (!generatedCardIds.Contains(i)) // 이미 생성된 카드 ID는 제외
            {
                availableCardIds.Add(i);
            }
        }

        if (availableCardIds.Count == 0)
        {
            Debug.LogError("No more unique cards available to generate.");
            return -1; // 더 이상 생성할 수 있는 카드가 없을 때
        }

        int randomIndex = Random.Range(0, availableCardIds.Count);
        int randomCardId = availableCardIds[randomIndex];
        generatedCardIds.Add(randomCardId); // 생성된 카드 ID를 리스트에 추가
        return randomCardId;
    }
}
