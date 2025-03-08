using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "GameData/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName; // �̸�
    public int level; // ����
    public Grade grade; // ���
    public AttackMethod attackMethod; // ���� ���
    public float totalPower; // �� �Ŀ�
    public float currentDamage; // ���� ������
    public float maxDamage; // �ִ� ������
    public float currentCiriticalRate; // ���� ũ��Ƽ�� Ȯ��
    public float maxCriticalRate; // �ִ� ũ��Ƽ�� Ȯ��
    public int skillLevel; // ��ų ����
    public string skillName; // ��ų �̸�
}
