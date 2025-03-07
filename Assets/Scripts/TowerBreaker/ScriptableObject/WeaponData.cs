using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "GameData/WeaponData")]
public class WeaponData : ScriptableObject
{
    string weaponName;
    int level;
    Grade grade;
    AttackMethod attackMethod;
    float totalPower;
    float currentDamage;
    float maxDamage;
    float currentCiriticalRate;
    float maxCriticalRate;
    int skillLevel;
    string skillName;
}
