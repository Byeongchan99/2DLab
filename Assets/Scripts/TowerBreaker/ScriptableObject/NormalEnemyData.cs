using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalEnemyData", menuName = "GameData/NormalEnemyData")]
public class NormalEnemyData : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    public float speed;
    public NormalEnemyType normalEnemyType;
}
