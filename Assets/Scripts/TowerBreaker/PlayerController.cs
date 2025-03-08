using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerBehavior _playerBehavior;

    private void Awake()
    {
        if (_playerBehavior == null)
        {
            _playerBehavior = GetComponent<PlayerBehavior>();
        }
    }

    public void OnClickDashButton()
    {
        Debug.Log("대쉬 버튼 클릭");
        _playerBehavior.Dash();
    }

    public void OnClickDefenseButton()
    {
        Debug.Log("방어 버튼 클릭");
        _playerBehavior.Defense();
    }

    public void OnClickAttackButton()
    {
        Debug.Log("공격 버튼 클릭");
        _playerBehavior.Attack();
    }
}
