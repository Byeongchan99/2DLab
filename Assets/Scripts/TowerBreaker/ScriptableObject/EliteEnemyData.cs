using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EliteEnemyData", menuName = "GameData/EliteEnemyData")]
public class EliteEnemyData : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public float speed;
    public EliteEnemyType eliteEnemyType;
    public EnemySkill enemySkill;
}
