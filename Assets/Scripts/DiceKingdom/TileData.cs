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

        // �ɼ� ȿ�� �߰� ����
        public List<TileEffect> effects;
    }
}
