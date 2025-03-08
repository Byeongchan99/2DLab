using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// �÷��̾��� ���� ���
    /// </summary>
    ArmorData _currentArmor;
    ShieldData _currentShield;
    WeaponData _currentWeapon;
    float _demonicPower; // %�� �߰� �����

    public ArmorData CurrentArmor { get => _currentArmor; set => _currentArmor = value; }
    public ShieldData CurrentShield { get => _currentShield; set => _currentShield = value; }
    public WeaponData CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
    public float DemonicPower { get => _demonicPower; set => _demonicPower = value; }


}
