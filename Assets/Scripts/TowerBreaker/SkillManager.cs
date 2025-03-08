using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    BaseSkill _currentArmorSkill; // ���� ������ �� ��ų
    BaseSkill _currentShieldSkill; // ���� ������ ���� ��ų
    BaseSkill _currentWeaponSkill; // ���� ������ ���� ��ų

    float _armorSkillGauge; // �� ��ų ������
    float _shieldSkillGauge; // ���� ��ų ������
    float _weaponSkillGauge; // ���� ��ų ������

    public BaseSkill CurrentArmorSkill { get => _currentArmorSkill; set => _currentArmorSkill = value; }
    public BaseSkill CurrentShieldSkill { get => _currentShieldSkill; set => _currentShieldSkill = value; }
    public BaseSkill CurrentWeaponSkill { get => _currentWeaponSkill; set => _currentWeaponSkill = value; }
    public float ArmorSkillGauge { get => _armorSkillGauge; set => _armorSkillGauge = value; }
    public float ShieldSkillGauge { get => _shieldSkillGauge; set => _shieldSkillGauge = value; }
    public float WeaponSkillGauge { get => _weaponSkillGauge; set => _weaponSkillGauge = value; }

    public void UseArmorSkill()
    {
        if (CurrentArmorSkill == null)
        {
            Debug.Log("�� ��ų�� �����ϴ�.");
        }
        else
        {
            if (ArmorSkillGauge < 100)
            {
                Debug.Log("�� ��ų �������� �����մϴ�.");
                return;
            }
            else
            {
                int skillLevel = (int)ArmorSkillGauge / 100;
                ExecuteSkill(skillLevel, CurrentArmorSkill);
                ArmorSkillGauge = 0;
            }
        }
    }

    public void UseShieldSkill()
    {
        if (CurrentShieldSkill == null)
        {
            Debug.Log("���� ��ų�� �����ϴ�.");
        }
        else
        {
            if (ShieldSkillGauge < 100)
            {
                Debug.Log("���� ��ų �������� �����մϴ�.");
                return;
            }
            else
            {
                int skillLevel = (int)ShieldSkillGauge / 100;
                ExecuteSkill(skillLevel, CurrentShieldSkill);
                ShieldSkillGauge = 0;
            }
        }
    }

    public void UseWeaponSkill()
    {
        if (CurrentWeaponSkill == null)
        {
            Debug.Log("���� ��ų�� �����ϴ�.");
        }
        else
        {
            if (WeaponSkillGauge < 100)
            {
                Debug.Log("���� ��ų �������� �����մϴ�.");
                return;
            }
            else
            {
                int skillLevel = (int)WeaponSkillGauge / 100;
                ExecuteSkill(skillLevel, CurrentWeaponSkill);
                WeaponSkillGauge = 0;
            }
        }
    }

    void ExecuteSkill(int level, BaseSkill skill)
    {
        skill.Execute(level);
    }
}
