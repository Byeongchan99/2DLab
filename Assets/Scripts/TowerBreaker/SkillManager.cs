using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField]
    BaseSkill _currentArmorSkill; // 현재 장착된 방어구 스킬
    BaseSkill _currentShieldSkill; // 현재 장착된 방패 스킬
    BaseSkill _currentWeaponSkill; // 현재 장착된 무기 스킬

    float _armorSkillGauge; // 방어구 스킬 게이지
    float _shieldSkillGauge; // 방패 스킬 게이지
    float _weaponSkillGauge; // 무기 스킬 게이지

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
            Debug.Log("방어구 스킬이 없습니다.");
        }
        else
        {
            if (ArmorSkillGauge < 100)
            {
                Debug.Log("방어구 스킬 게이지가 부족합니다.");
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
            Debug.Log("방패 스킬이 없습니다.");
        }
        else
        {
            if (ShieldSkillGauge < 100)
            {
                Debug.Log("방패 스킬 게이지가 부족합니다.");
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
            Debug.Log("무기 스킬이 없습니다.");
        }
        else
        {
            if (WeaponSkillGauge < 100)
            {
                Debug.Log("무기 스킬 게이지가 부족합니다.");
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
