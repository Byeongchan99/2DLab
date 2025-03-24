using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DiceKingdom
{
    public class PathManager : MonoBehaviour
    {
        public Tilemap pathTilemap;
        public List<Vector3> pathPositions;

        // 경로 초기화
        public void InitializePath(Vector3 spawnPoint, Vector3 endPoint)
        {
            pathPositions.Clear();

            var tilePositions = new List<Vector3Int>();

            foreach (var pos in pathTilemap.cellBounds.allPositionsWithin)
            {
                if (pathTilemap.HasTile(pos))
                {
                    CustomTile tile = pathTilemap.GetTile<CustomTile>(pos);
                    if (tile != null && tile.tileData.tileType == TileType.Path)
                    {
                        tilePositions.Add(pos);
                    }
                }
            }

            var worldPositions = new List<Vector3>();

            foreach (var tilePos in tilePositions)
            {
                worldPositions.Add(pathTilemap.GetCellCenterWorld(tilePos));
            }

            pathPositions = CalculateOrderedPath(worldPositions, spawnPoint, endPoint);
        }

        // 경로 계산
        List<Vector3> CalculateOrderedPath(List<Vector3> positions, Vector3 start, Vector3 end)
        {
            List<Vector3> ordered = new List<Vector3>();
            Vector3 current = start;
            var remaining = new List<Vector3>(positions);

            while (remaining.Count > 0)
            {
                Vector3 next = GetClosestPosition(current, remaining);
                ordered.Add(next);
                remaining.Remove(next);
                current = next;
            }

            ordered.Add(end);
            return ordered;
        }

        // 가장 가까운 위치 찾기
        Vector3 GetClosestPosition(Vector3 current, List<Vector3> positions)
        {
            Vector3 closest = positions[0];
            float minDist = Vector3.Distance(current, closest);

            foreach (var pos in positions)
            {
                float dist = Vector3.Distance(current, pos);
                if (dist < minDist)
                {
                    closest = pos;
                    minDist = dist;
                }
            }
            return closest;
        }
    }
}
