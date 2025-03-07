using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BaseSkill : MonoBehaviour
{
    SkillData skillData;

    public abstract void Execute();
}
