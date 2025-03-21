using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldData", menuName = "GameData/ShieldData")]
public class ShieldData : ScriptableObject
{
    public string shieldName;
    public BaseSkill shieldSkill;
}
