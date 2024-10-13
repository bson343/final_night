using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CardResourceMappedManager : MonoBehaviour
{
    [SerializeField] string CSV_Path;

    public void Start()
    {
        Assert.IsFalse(true); //사용하지 않는 클래스; 삭제 예정
        GoogleSheetManager m = GetComponent<GoogleSheetManager>();

    }

    public void init(string RowData)
    {
        Assert.IsFalse(true); //사용하지 않는 클래스; 삭제 예정
        CardResourceLoader loader = new CardResourceLoader();

        //loader.Init("CSV/CardInfo");
        loader.Init(RowData);

        loader.LoadCardDataMap();
        loader.LoadCardSpriteMap();

        //ResourceManager.Instance.Init(loader);
    }

}
