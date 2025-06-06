using UnityEngine;

public class TreasureNPC : MonoBehaviour
{
    public GameObject shopPopup;
    public GameObject exitBtn;
    [SerializeField] private RewardManager rewardManager;

    private bool hasGeneratedCard = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleShopPopup();
                if (!hasGeneratedCard)
                {
                    GetCardReward();
                    hasGeneratedCard = true;
                }
            }
        }
    }

    private void ToggleShopPopup()
    {
        if (shopPopup != null)
        {
            bool isActive = shopPopup.activeSelf;
            shopPopup.SetActive(!isActive);
            exitBtn.SetActive(false);
        }
    }

    private void GetCardReward()
    {
        if (rewardManager != null)
        {
            rewardManager.GetTreasureCard();
        }
        else
        {
            Debug.LogWarning("RewardManager가 할당되지 않았습니다.");
        }
    }
}
