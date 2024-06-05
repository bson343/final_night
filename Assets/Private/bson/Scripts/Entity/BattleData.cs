using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "aboutBattleData", menuName = "aboutBattleData/BattleData")]
public class BattleData : ScriptableObject
{
    [SerializeField]
    private List<Enemy> enemies;
    [SerializeField]
    private List<Vector3> spawnPos;

    public int maxMoney;
    public int minMoney;

    public List<Enemy> Enemies => enemies;
    public List<Vector3> SpawnPos => spawnPos;
}