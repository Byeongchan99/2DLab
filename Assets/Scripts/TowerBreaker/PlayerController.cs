using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    PlayerBehavior _playerBehavior;
    SkillManager _skillManager;

    private void Awake()
    {
        if (_playerBehavior == null)
        {
            _playerBehavior = GetComponent<PlayerBehavior>();
        }
    }

    public void OnClickDashButton()
    {
        Debug.Log("�뽬 ��ư Ŭ��");
        _playerBehavior.Dash();
    }

    public void OnClickDefenseButton()
    {
        Debug.Log("��� ��ư Ŭ��");
        _playerBehavior.Defense();
    }

    public void OnClickAttackButton()
    {
        Debug.Log("���� ��ư Ŭ��");
        _playerBehavior.Attack();
    }

    public void OnClickArmorSkillButton()
    {
        Debug.Log("��ų ��ư Ŭ��");
        _skillManager.UseArmorSkill();
    }

    public void OnClickShieldSkillButton()
    {
        Debug.Log("��ų ��ư Ŭ��");
        _skillManager.UseShieldSkill();
    }

    public void OnClickWeaponSkillButton()
    {
        Debug.Log("��ų ��ư Ŭ��");
        _skillManager.UseWeaponSkill();
    }
}
