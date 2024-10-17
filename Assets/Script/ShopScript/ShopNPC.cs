using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    public GameObject shopPopup;
    public GameObject exitBtn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ToggleShopPopup();
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
}