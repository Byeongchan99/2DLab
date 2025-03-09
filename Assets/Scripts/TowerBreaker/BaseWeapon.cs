using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class BaseWeapon : MonoBehaviour
{
    WeaponData weaponData;
    BaseSkill weaponSkill;

    // OverlapBox�� ����Ͽ� ���� �浹�ϴ��� Ȯ��
    abstract public void Attack();
}
