using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    float _currentHealth;
    float _maxHealth;
    float _speed;

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float Speed { get => _speed; set => _speed = value; }

    public virtual void Move()
    {
        // �÷��̾ ���� �̵�
    }

    public virtual void Die()
    {
        // ������Ʈ Ǯ�� ��ȯ �� �������� �Ŵ������� ������ �˸�
    }
}
