using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    float _currentHealth;
    float _maxHealth;
    float _speed;

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float Speed { get => _speed; set => _speed = value; }

    protected virtual void OnEnable()
    {
        // 몇 초뒤에 활성화 로직
    }

    public void SetStat(float health, float speed)
    {
        MaxHealth = health;
        CurrentHealth = MaxHealth;
        Speed = speed;
    }

    public virtual void Move()
    {
        // 플레이어를 향해 이동
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
        // 오브젝트 풀로 반환 및 스테이지 매니저에게 죽음을 알림
    }
}
