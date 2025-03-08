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
}
