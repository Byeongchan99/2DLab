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

                Debug.Log($"클릭한 타일 위치: {cellPos}");

                // 경로 타일 확인
                TileBase pathTile = pathTilemap.GetTile(cellPos);
                if (pathTile != null)
                {
                    Debug.Log($"경로 타일: {pathTile.name}");
                    if (pathTile is CustomTile customPathTile)
                        LogCustomTileInfo(customPathTile);
                }
                else
                    Debug.Log("경로 타일 없음");

                // 배치 타일 확인
                TileBase placementTile = placementTilemap.GetTile(cellPos);
                if (placementTile != null)
                {
                    Debug.Log($"배치 타일: {placementTile.name}");
                    if (placementTile is CustomTile customPlacementTile)
                        LogCustomTileInfo(customPlacementTile);
                }
                else
                    Debug.Log("배치 타일 없음");
                
                // 배경 타일 확인
                TileBase bgTile = backgroundTilemap.GetTile(cellPos);
                if (bgTile != null)
                    Debug.Log($"배경 타일: {bgTile.name}");
                else
                    Debug.Log("배경 타일 없음");               
            }
        }

        void LogCustomTileInfo(CustomTile tile)
        {
            Debug.Log($"타일 종류: {tile.tileData.tileType}");
            if (tile.tileData.effects != null)
            {
                foreach (var effect in tile.tileData.effects)
                    Debug.Log($"타일 효과: {effect.name}");
            }
        }
    }
}
