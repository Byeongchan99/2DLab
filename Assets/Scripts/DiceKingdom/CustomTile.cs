using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DiceKingdom
{
    [CreateAssetMenu(fileName = "NewCustomTile", menuName = "Tiles/Custom Tile")]
    public class CustomTile : Tile
    {
        public TileData tileData;
    }
}
