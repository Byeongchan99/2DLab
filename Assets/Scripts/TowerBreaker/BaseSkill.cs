using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    SkillData skillData;

    public abstract void Execute(int level);
}
