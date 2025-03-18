using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "DiceKingdom/Effects/Damage")]
    public class DamageEffect : TileEffect
    {
        public float damageAmount = 10f;
        private float timer = 0f;
        private const float tickInterval = 0.2f;

        public override void OnEnter(GameObject target)
        {
            timer = 0f; // 타이머 초기화
        }

        public override void OnStay(GameObject target, float deltaTime)
        {
            timer += deltaTime;
            if (timer >= tickInterval)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                    Debug.Log($"DamageEffect: {damageAmount} damage applied.");
                }
                timer = 0f;
            }
        }

        public override void OnExit(GameObject target)
        {
            timer = 0f;
        }
    }
}
