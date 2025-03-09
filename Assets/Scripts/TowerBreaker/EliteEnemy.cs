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
        Debug.Log("엘리트 적 속도 증가");
        Speed += 5;
    }

    void IncreaseWeight()
    {
        Debug.Log("엘리트 적 무게 증가");
    }

    void IncreaseHealth()
    {
        Debug.Log("엘리트 적 체력 증가");
        MaxHealth *= 2;
        CurrentHealth = MaxHealth;
    }
}
