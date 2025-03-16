using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DiceKingdom
{
    [CustomEditor(typeof(Tile))]
    public class TileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Tile tile = (Tile)target;

            if (GUILayout.Button("Set as Path Tile"))
            {
                tile.tileData = Resources.Load<TileData>("Tiles/PathTile");
                EditorUtility.SetDirty(tile);
            }

            if (GUILayout.Button("Set as Placement Tile"))
            {
                tile.tileData = Resources.Load<TileData>("Tiles/PlacementTile");
                EditorUtility.SetDirty(tile);
            }
        }
    }
}
