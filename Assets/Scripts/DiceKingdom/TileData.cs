using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "New TileData", menuName = "DiceKingdom/TileData")]
    public class TileData : ScriptableObject
    {
        public TileType tileType;
        public Sprite tileSprite;

        // 옵션 효과 추가 가능
        public List<TileEffect> effects;
    }
}
