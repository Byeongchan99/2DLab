using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "New TileEffect", menuName = "DiceKingdom/TileEffect")]
    public abstract class TileEffect : ScriptableObject
    {
        public abstract void ApplyEffect(GameObject target);
    }
}
