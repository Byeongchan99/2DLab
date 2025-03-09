using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : BaseEnemy
{
    EnemySkill enemySkill;

    protected override void OnEnable()
    {
        base.OnEnable();
        ActiveSkill();
    }

    void ActiveSkill()
    {
        switch(enemySkill)
        {
            case EnemySkill.SpeedUp:
                SpeedUp();
                break;
            case EnemySkill.IncreaseWeight:
                IncreaseWeight();
                break;
            case EnemySkill.IncreaseHealth:
                IncreaseHealth();
                break;           
        }
    }

    void SpeedUp()
    {
        Debug.Log("����Ʈ �� �ӵ� ����");
        Speed += 5;
    }

    void IncreaseWeight()
    {
        Debug.Log("����Ʈ �� ���� ����");
    }

    void IncreaseHealth()
    {
        Debug.Log("����Ʈ �� ü�� ����");
        MaxHealth *= 2;
        CurrentHealth = MaxHealth;
    }
}
