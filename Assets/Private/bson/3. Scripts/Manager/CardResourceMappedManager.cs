using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardResourceMappedManager : MonoBehaviour
{
    [SerializeField] string CSV_Path;

    void Awake()
    {
        CardResourceLoader loader = new CardResourceLoader();

        loader.Init("CSV/CardInfo");

        loader.LoadCardDataMap();
        loader.LoadCardSpriteMap();

        CardMap.Instance.Init(loader);
    }

}
