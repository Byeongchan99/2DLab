using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "DiceKingdom/Effects/Slow")]
    public class SlowEffect : TileEffect
    {
        public float slowAmount;
        public override void ApplyEffect(GameObject target)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
                enemy.ApplySlow(slowAmount);
        }
    }
}
