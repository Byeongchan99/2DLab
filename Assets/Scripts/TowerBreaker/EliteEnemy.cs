using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : BaseEnemy
{
    public EliteEnemyType eliteEnemyType; // ������Ʈ Ǯ���� �����ϱ� ���� ����Ʈ �� Ÿ��
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
