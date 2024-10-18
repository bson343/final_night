using System.Collections.Generic;
using TMPro;
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

        //Init();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        Config = GetComponent<ConfigLoader>();
        _loader = new CardResourceLoader();
        _csvSync = GetComponent<GoogleSheetManager>();

        Assert.IsNotNull(Config, "Need Component: ConfigLoader");
        Assert.IsNotNull(_csvSync, "Need Component: GoogleSheetManager");

        Debug.Log($"Config : {Config.ToString()}");

        Config.Init();
        _csvSync.Init(completeCSV);

    }

    void completeCSV()
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

#if UNITY_EDITOR
        txtShowState.GetChild(0).GetComponent<TMP_Text>().text = "Resource Completed";
#else
        txtShowState.GetChild(0).GetComponent<TMP_Text>().text = "";
#endif

        Instance = this;
        IsInit = true;
        DontDestroyOnLoad(this);
    }
}
