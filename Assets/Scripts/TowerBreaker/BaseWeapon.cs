using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BaseWeapon : MonoBehaviour
{
    WeaponData weaponData;
    BaseSkill weaponSkill;

    // OverlapBox를 사용하여 적과 충돌하는지 확인
    abstract public void Attack();
}
