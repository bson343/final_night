using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardResourceMappedManager : MonoBehaviour
{
    [SerializeField] string CSV_Path;

    public void Start()
    {
        GoogleSheetManager m = GetComponent<GoogleSheetManager>();

        m.init(init);
    }

    public void init(string RowData)
    {
        CardResourceLoader loader = new CardResourceLoader();

        //loader.Init("CSV/CardInfo");
        loader.Init(RowData);

        loader.LoadCardDataMap();
        loader.LoadCardSpriteMap();

        CardMap.Instance.Init(loader);
    }

}
