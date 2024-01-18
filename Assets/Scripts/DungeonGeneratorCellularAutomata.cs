using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorCellularAutomata : MonoBehaviour
{
    // 맵 크기
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    [SerializeField] private string seed; // 난수 생성하기 위한 시드
    [SerializeField] private bool useRandomSeed; // 시드 사용 여부

    [Range(0, 8)]
    [SerializeField] private int referenceWallCount; // 기준 벽 타일 개수
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent; // 맵의 통로와 벽 비율

    private int[,] map, oldMap;
    // 타일 타입
    private const int ROAD = 0; // 통로
    private const int WALL = 1; // 벽

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile roadTile; // 통로 타일
    [SerializeField] private Tile wallTile; // 벽 타일

    public void GenerateDungeon()
    {
        ResetDungeon(); // 던전 초기화
        GenerateMap();
        SmoothMap();
    }

    private void GenerateMap()
    {
        map = new int[mapWidth, mapHeight];
        oldMap = new int[mapWidth, mapHeight];
        MapRandomFill();      
    }

    // 맵을 비율에 따라 벽 혹은 빈 공간으로 랜덤하게 채우는 메서드
    private void MapRandomFill()
    {
        if (useRandomSeed) // 시드 사용 여부
            seed = Time.time.ToString(); // 시드 생성

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); // 의사 난수 생성

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1) 
                    map[x, y] = WALL; // 가장자리는 벽으로 설정
                else 
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; // 비율에 따라 벽 또는 통로로 설정
            }
        }
    }

    // 원래 맵 정보 저장
    private void CreateOldMap() {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                oldMap[x, y] = map[x, y];
            }
        }
    }

    // 맵의 타일들을 순회하며 경계면을 다듬는 메서드
    public void SmoothMap()
    {
        CreateOldMap(); // 원래 맵 정보 저장

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y); // 주변 벽 타일 개수 계산
                if (neighbourWallTiles > referenceWallCount) 
                    map[x, y] = WALL; // 주변 칸 중 벽이 기준 벽 타일 개수를 초과할 경우 현재 타일을 벽으로 설정
                else if (neighbourWallTiles < referenceWallCount) 
                    map[x, y] = ROAD; // 주변 칸 중 벽이 기준 벽 타일 개수 미만일 경우 현재 타일을 통로로 설정
                OnDrawTile(x, y, map[x, y]); // 타일 색상 변경
            }
        }
    }

    // 주변의 벽 타일 개수를 계산하는 메서드
    private int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        // 현재 좌표를 기준으로 주변 8칸 검사
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) 
        { 
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                // 맵 범위 검사
                if (neighbourX >= 0 && neighbourX < mapWidth && neighbourY >= 0 && neighbourY < mapHeight)
                {
                    // 값이 바뀌지 않은 oldMap 정보 사용
                    if (neighbourX != x || neighbourY != y) 
                        wallCount += oldMap[neighbourX, neighbourY];
                }
                else  // 주변 타일이 맵 범위를 벗어날 경우(현재 타일이 가장자리 타일)
                    wallCount++;
            }
        }
        return wallCount;
    }

    // 타일 타입에 따라 타일 생성
    private void OnDrawTile(int x, int y, int tileType)
    {
        Vector3Int pos = new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0); // 중앙 정렬

        if (tileType == ROAD)
            tilemap.SetTile(pos, roadTile);
        else
            tilemap.SetTile(pos, wallTile);
    }

    // 던전 초기화 메서드
    public void ResetDungeon()
    {
        // 타일맵의 모든 타일 제거
        tilemap.ClearAllTiles();       
    }
}
