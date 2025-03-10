using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : BaseEnemy
{
    public EliteEnemyType eliteEnemyType; // 오브젝트 풀에서 구분하기 위한 엘리트 적 타입
    EnemySkill _enemySkill;

    protected override void OnEnable()
    {
        base.OnEnable();
        ActiveSkill();
    }

    public void SetStat(EliteEnemyData data)
    {
        MaxHealth = data.maxHealth;
        CurrentHealth = data.currentHealth;
        Speed = data.speed;
        eliteEnemyType = data.eliteEnemyType;
        _enemySkill = data.enemySkill;
    }

    void ActiveSkill()
    {
        switch(_enemySkill)
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
