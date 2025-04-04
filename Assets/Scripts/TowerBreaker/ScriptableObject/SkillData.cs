using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "GameData/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillLevel;
    public string skillName;
    public float gaugeAmount; // 한번에 충전되는 게이지 양
}
