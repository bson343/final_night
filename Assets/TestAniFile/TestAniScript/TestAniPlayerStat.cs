using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAniPlayerStat : CharacterStat
{
    [SerializeField] private Text hpText;
    [SerializeField] private Text energyText;
    [SerializeField] private Text moneyText;
    [SerializeField]  public bool testMode = false;

    private int _money;
    private int _maxOrb;
    private int _currentOrb;
    private int _height;

    public int MaxOrb
    {
        get { return _maxOrb; }
        set
        {
            _maxOrb = value;
            _maxOrb = Mathf.Clamp(_maxOrb, 0, 99);
            energyText.text = _currentOrb + "/" + _maxOrb;
        }
    }

   

    public int CurrentOrb
    {
        get { return _currentOrb; }
        set
        {
            _currentOrb = value;
            _currentOrb = Mathf.Clamp(_currentOrb, 0, 99);
            energyText.text = _currentOrb + "/" + _maxOrb;
        }
    }

    public int Height
    {
        get { return _height; }
        set
        {
            _height = value;
        }
    }

    public int Money
    {
        get { return _money; }
        set
        {
            _money = value;
            _money = Mathf.Clamp(_money, 0, 9999);
            moneyText.text = _money.ToString();
        }
    }

    public override void Init(Character character)
    {
        base.Init(character);

        if (testMode)
        {
            MaxHp = 80;
            CurrentHp = 80;
        }
        else
        {
            MaxHp = UserManager.Instance.MaxHP;
            CurrentHp = UserManager.Instance.CurrentHP;
        }

        setPlayerStatData();
    }

    /*public void Init(Character character) //테스트모드 아닐시
    {
        base.Init(character);

        MaxHp = UserManager.Instance.MaxHP;
        CurrentHp = UserManager.Instance.CurrentHP;
        
        setPlayerStatData();
    }*/

    private void setPlayerStatData()
    {
        Height = 0;
        MaxOrb = 3;
        CurrentOrb = MaxOrb;
        onChangeHp += (() => hpText.text = CurrentHp + "/" + MaxHp);
        onChangeHp += (() => hpText.text = CurrentHp + "/" + MaxHp);
    }
}
