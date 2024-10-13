using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }
    public bool IsInit { get; private set; }

    [SerializeField]
    Transform txtShowState;

    [SerializeField]
    Transform btnLogin;

    private GoogleSheetManager _csvSync;
    private CardResourceLoader _loader;
    
    //--------Resource-------
    public ConfigLoader Config { get; private set; }
    public Dictionary<decimal, BattleCardData> CardDataMap { get; private set; }
    public Dictionary<string, Sprite> CardSpriteMap { get; private set; }
    public List<int> AttackCardIdList { get; private set; }
    public List<int> SkillCardIdList { get; private set; }
    public List<int> HeroCardIdList { get; private set; }
    //--------Resource-------

    private void Awake()
    {
        Assert.IsNotNull(btnLogin, "Need Object: Login Button");
        Assert.IsNotNull(txtShowState, "Need Object: LogResourceManager");


        btnLogin.GetComponent<Button>().interactable = false;
        txtShowState.GetChild(0).GetComponent<TMP_Text>().text = string.Empty;

        Init();
    }

    void Init()
    {
        Assert.IsNotNull(Config = GetComponent<ConfigLoader>(), "Need Component: ConfigLoader");
        _loader = new CardResourceLoader();

        Config.Init();


#if UNITY_EDITOR
        Assert.IsNotNull(_csvSync = GetComponent<GoogleSheetManager>(), "Need Component: GoogleSheetManager");

        _csvSync.Init(completeCSV_EDITOR);

#else

#endif
    }

    void completeCSV_EDITOR()
    {
        _loader.Init(_csvSync.RawDataCSV);

        _loader.LoadCardSpriteMap();
        _loader.LoadCardDataMap();

        this.CardSpriteMap = _loader.CardSpriteMap;
        this.CardDataMap = _loader.CardDataMap;
        this.AttackCardIdList = _loader.AttackCardIdList;
        this.SkillCardIdList = _loader.SkillCardIdList;
        this.HeroCardIdList = _loader.HeroCardIdList;

        btnLogin.GetComponent<Button>().interactable = true;
        txtShowState.GetChild(0).GetComponent<TMP_Text>().text = "Resource Completed";

        DontDestroyOnLoad(this);
        Instance = this;
        IsInit = true;
    }


}
