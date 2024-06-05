using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : CharacterStat
{
    [SerializeField] private Text hpText;
    [SerializeField] private Text energyText;
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

    public override void Init(Character character)
    {
        base.Init(character);

        Height = 0;
        MaxOrb = 3;
        CurrentOrb = MaxOrb;
        onChangeHp += (() => hpText.text = CurrentHp + "/" + MaxHp);
    }
}
