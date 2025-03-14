using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "GameData/ArmorData")]
public class ArmorData : ScriptableObject
{
    public string ArmorName;
    public BaseSkill ArmorSkill;
}
