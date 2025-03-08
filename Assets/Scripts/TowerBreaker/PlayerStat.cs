using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField]
    /// <summary>
    /// 플레이어의 현재 장비
    /// </summary>
    ArmorData _currentArmor;
    ShieldData _currentShield;
    WeaponData _currentWeapon;
    float _demonicPower; // %로 추가 대미지

    public ArmorData CurrentArmor { get => _currentArmor; set => _currentArmor = value; }
    public ShieldData CurrentShield { get => _currentShield; set => _currentShield = value; }
    public WeaponData CurrentWeapon { get => _currentWeapon; set => _currentWeapon = value; }
    public float DemonicPower { get => _demonicPower; set => _demonicPower = value; }


}
