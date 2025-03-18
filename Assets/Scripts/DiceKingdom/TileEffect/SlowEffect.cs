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
        public float slowMultiplier = 0.5f; // �ӵ� 50% ����
        private bool applied = false;

        public override void OnEnter(GameObject target)
        {
            if (!applied)
            {
                // ����: Monster ������Ʈ�� ���� ��� �ӵ� ����
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
            // �ʿ��� ��� �������� ȿ�� ó��
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
