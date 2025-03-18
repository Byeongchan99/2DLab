using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "DiceKingdom/Effects/Damage")]
    public class DamageEffect : TileEffect
    {
        public float damageAmount;
        public override void ApplyEffect(GameObject target)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damageAmount);
        }
    }
}
