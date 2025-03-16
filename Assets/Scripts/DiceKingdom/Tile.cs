using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    public class Tile : MonoBehaviour
    {
        public TileData tileData;

        public void TriggerTileEffect(GameObject target)
        {
            foreach (var effect in tileData.effects)
                effect.ApplyEffect(target);
        }
    }
}
