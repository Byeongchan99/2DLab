using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "DiceKingdom/Effects/Slow")]
    public class SlowEffect : TileEffect
    {
        public float slowMultiplier = 0.5f; // 속도 50% 감소
        private bool applied = false;

        public override void OnEnter(GameObject target)
        {
            if (!applied)
            {
                // 예시: Monster 컴포넌트가 있을 경우 속도 조정
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplySlow(slowMultiplier);
                    applied = true;
                }
            }
        }

        public override void OnStay(GameObject target, float deltaTime)
        {
            // 필요한 경우 지속적인 효과 처리
        }

        public override void OnExit(GameObject target)
        {
            if (applied)
            {
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.RemoveSlow();
                    applied = false;
                }
            }
        }
    }
}
