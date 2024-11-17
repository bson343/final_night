using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    protected delegate void OnChangeHp();
    protected OnChangeHp onChangeHp;

    private Character _character;

    [Header("Stat")]
    [SerializeField] private int _maxHp;
    private int _currentHp;
    private int _shield;
     private int _power;
    // private int _agility;

    [Header("UI")]
    [SerializeField] private GameObject _hpCanvas;
    [SerializeField] private HpBar _hpBar;

    [Header("Info")]
    [SerializeField]
    private bool _isPlayer = false;
    
    private VFXGenerator vfxGenerator => ServiceLocator.Instance.GetService<VFXGenerator>();

    public int MaxHp
    {
        get { return _maxHp; }
        set
        {
            int changeValue = value - _maxHp;
            _maxHp = value;

            // 최대 hp증가한 만큼 현재체력도 올라감
            if (changeValue > 0)
                CurrentHp += changeValue;

            onChangeHp?.Invoke();
        }
    }

    public int CurrentHp
    {
        get { return _currentHp; }
        set
        {
            // 체력바
            _currentHp = value;
            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

            _hpBar.DisplayHpBar(_currentHp, _maxHp);

            onChangeHp?.Invoke();

            if (_currentHp <= 0)
            {
                Debug.Log("죽었당");
                Dead();
            }
        }
    }

    public int Shield
    {
        get { return _shield; }
        set
        {
            int shieldAmount = value - _shield;

            _shield += shieldAmount;
            _shield = Mathf.Clamp(_shield, 0, 999);

            _hpBar.DisplayShield(_shield);
        }
    }

    public int Power { get; set; }

    public bool IsDead => CurrentHp == 0;

    public virtual void Init(Character character)
    {
        this._character = character;
        CurrentHp = MaxHp;
    }

    private void Dead()
    {
        _character.Dead();
    }

    public void Hit(int damage)
    {
        if (_isPlayer)
        {
            vfxGenerator.CreateVFX(EVFX.EnemyAttack, transform.position);
        }
        else
        {
            vfxGenerator.CreateVFX(EVFX.PlayerAttack, transform.position);
        }

        if (_character.indent[(int)EIndent.Weak])
        {
            damage = Mathf.RoundToInt((float)damage * 1.5f);
        }

        if (_shield > 0)
        {
            if (_shield >= damage)
            {
                Shield -= damage; // 쉴드로 모든 피해를 막을 수 있으면 체력에 영향을 주지 않음
                damage = 0;
            }
            else
            {
                damage -= _shield; // 쉴드를 소모하고 남은 피해를 체력에 반영
                Shield = 0;
            }
        }

        // 남은 피해를 체력에 반영
        if (damage > 0)
        {
            CurrentHp -= damage;
        }
    }

    public void IsBattle(bool flag)
    {
        _hpCanvas.SetActive(flag);
    }
}
