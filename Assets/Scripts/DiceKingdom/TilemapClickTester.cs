using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DiceKingdom
{
    public class TilemapClickTester : MonoBehaviour
    {
        public Tilemap backgroundTilemap;
        public Tilemap pathTilemap;
        public Tilemap placementTilemap;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = backgroundTilemap.WorldToCell(worldPos);

                Debug.Log($"Ŭ���� Ÿ�� ��ġ: {cellPos}");

                // ��� Ÿ�� Ȯ��
                TileBase pathTile = pathTilemap.GetTile(cellPos);
                if (pathTile != null)
                {
                    Debug.Log($"��� Ÿ��: {pathTile.name}");
                    if (pathTile is CustomTile customPathTile)
                        LogCustomTileInfo(customPathTile);
                }
                else
                    Debug.Log("��� Ÿ�� ����");

                // ��ġ Ÿ�� Ȯ��
                TileBase placementTile = placementTilemap.GetTile(cellPos);
                if (placementTile != null)
                {
                    Debug.Log($"��ġ Ÿ��: {placementTile.name}");
                    if (placementTile is CustomTile customPlacementTile)
                        LogCustomTileInfo(customPlacementTile);
                }
                else
                    Debug.Log("��ġ Ÿ�� ����");
                
                // ��� Ÿ�� Ȯ��
                TileBase bgTile = backgroundTilemap.GetTile(cellPos);
                if (bgTile != null)
                    Debug.Log($"��� Ÿ��: {bgTile.name}");
                else
                    Debug.Log("��� Ÿ�� ����");               
            }
        }

        void LogCustomTileInfo(CustomTile tile)
        {
            Debug.Log($"Ÿ�� ����: {tile.tileData.tileType}");
            if (tile.tileData.effects != null)
            {
                foreach (var effect in tile.tileData.effects)
                    Debug.Log($"Ÿ�� ȿ��: {effect.name}");
            }
        }
    }
}
