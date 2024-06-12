using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[System.Serializable]
public class Pattern
{
    public EnemyPatternData patternData;
    public IndentData indentData;
    public int amount;
    public int secondAmount;
}


public class EnemyPattern : MonoBehaviour
{
    private Enemy _enemy;

    public Pattern alreadyPattern;  // 이미 있는 패턴
    public Pattern enemyFirstPattern;  // 제일 처음 패턴
    public List<Pattern> enemyPatterns;  // 그 외에 패턴
    public List<Pattern> enemyCyclePatterns; // 순환하는 패턴
    public bool isAlreadyPattern = false;
    public bool isFirstPattern = false;  // 제일 처음 패턴이 있는가
    public bool isCyclePattern = false; // 패턴이 순환하는가

    [SerializeField] private Image _patternImage;
    [SerializeField] private Text _patternText;

    private bool _isDecided = false;
    private int _patternTurn = 1;
    private Pattern _currentPattern;

    private bool isActFirst = true;

    private VFXGenerator vfxGenerator => ServiceLocator.Instance.GetService<VFXGenerator>();
    private BattleManager battleManager => ServiceLocator.Instance.GetService<BattleManager>();

    public void Init(Enemy enemy)
    {
        _enemy = enemy;
        isActFirst = true;
        if (isAlreadyPattern)
        {
            _currentPattern = alreadyPattern;
        }

        if (_currentPattern == null)
        {
            DecidePattern();
            _isDecided = true;
        }
    }

    public void Act()
    {
        // 첫번째 턴에 할 패턴이 있으면 실행
        ActPattern();

        _patternTurn++;
    }

    public void DecidePattern()
    {
        // 패턴 결정하고 ui로 보여주기
        // 내 턴 시작 델리게이트에 이 함수를 넣어줘야 함

        // 첫번째 패턴이 있는 적이면 첫번째 패턴을 해줘야 함
        // 아니면 랜덤(일단은...)

        if(_isDecided)
        {
            _isDecided = false;
        }
        else if(_patternTurn == 1 && isFirstPattern && isActFirst)
        {
            _currentPattern = enemyFirstPattern;
            isActFirst = false;
            _patternTurn = 0;
        }
        else if(isCyclePattern)
        {
            _currentPattern = enemyCyclePatterns[(_patternTurn - 1) % enemyCyclePatterns.Count];
        }
        else
        {
            _currentPattern = enemyPatterns[Random.Range(0, enemyPatterns.Count)];
        }

        _patternImage.sprite = _currentPattern.patternData.patternIcon;
        _patternText.text = GetPatternAmount();
    }

    public void DecidePattern(Pattern pattern)
    {
        _currentPattern = pattern;
        _isDecided = true;

        _patternImage.sprite = _currentPattern.patternData.patternIcon;
        _patternText.text = "";
    }

    private void ActPattern()
    {
        switch (_currentPattern.patternData.patternType)
        {
            case EPatternType.Attack:
                battleManager.Player.Hit(_currentPattern.amount + _enemy.CharacterStat.Power, _enemy);
                break;
            default:
                Assert.IsTrue(false, "Non PatternType");
                break;
        }
    }

    private string GetPatternAmount()
    {
        string result = "";

        switch (_currentPattern.patternData.patternType)
        {
            case EPatternType.Attack:
            case EPatternType.AttackDefend:
            case EPatternType.AttackDebuff:
                result = (_currentPattern.amount + _enemy.CharacterStat.Power).ToString();
                break;
        }

        return result;
    }
}
