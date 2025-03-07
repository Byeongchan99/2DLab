using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BaseWeapon : MonoBehaviour
{
    WeaponData weaponData;
    BaseSkill weaponSkill;

    abstract public void Attack();
}
