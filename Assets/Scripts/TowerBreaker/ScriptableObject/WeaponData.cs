using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "GameData/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // 이름
    public int level; // 레벨
    public Grade grade; // 등급
    public AttackMethod attackMethod; // 공격 방식
    public float totalPower; // 총 파워
    public float currentDamage; // 현재 데미지
    public float maxDamage; // 최대 데미지
    public float currentCiriticalRate; // 현재 크리티컬 확률
    public float maxCriticalRate; // 최대 크리티컬 확률
    public int skillLevel; // 스킬 레벨
    public string skillName; // 스킬 이름
}
