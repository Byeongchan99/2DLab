using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    float _currentHealth;
    float _maxHealth;
    float _speed;
    public NormalEnemyType normalEnemyType; // ������Ʈ Ǯ���� �����ϱ� ���� �Ϲ� �� Ÿ��

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float Speed { get => _speed; set => _speed = value; }

    protected virtual void OnEnable()
    {
        // �� �ʵڿ� Ȱ��ȭ ����
    }

    public void SetStat(NormalEnemyData data)
    {
        MaxHealth = data.currentHealth;
        CurrentHealth = data.maxHealth;
        Speed = data.speed;
        normalEnemyType = data.normalEnemyType;
    }

    public virtual void Move()
    {
        // �÷��̾ ���� �̵�
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // ������Ʈ Ǯ�� ��ȯ �� �������� �Ŵ������� ������ �˸�
        EnemyPool.Instance.ReturnToNormalPool(this);
    }
}
