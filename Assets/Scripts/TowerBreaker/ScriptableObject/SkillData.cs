using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "GameData/SkillData")]
public class SkillData : ScriptableObject
{
    int skillLevel;
    string skillName;
    float gaugeAmount; // �ѹ��� �����Ǵ� ������ ��
}
