using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorCellularAutomata : MonoBehaviour
{
    // �� ũ��
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    [SerializeField] private string seed; // ���� �����ϱ� ���� �õ�
    [SerializeField] private bool useRandomSeed; // �õ� ��� ����

    [Range(0, 8)]
    [SerializeField] private int referenceWallCount; // ���� �� Ÿ�� ����
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent; // ���� ��ο� �� ����

    private int[,] map, oldMap;
    // Ÿ�� Ÿ��
    private const int ROAD = 0; // ���
    private const int WALL = 1; // ��

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile roadTile; // ��� Ÿ��
    [SerializeField] private Tile wallTile; // �� Ÿ��

    public void GenerateDungeon()
    {
        ResetDungeon(); // ���� �ʱ�ȭ
        GenerateMap();
        SmoothMap();
    }

    private void GenerateMap()
    {
        map = new int[mapWidth, mapHeight];
        oldMap = new int[mapWidth, mapHeight];
        MapRandomFill();      
    }

    // ���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��� �޼���
    private void MapRandomFill()
    {
        if (useRandomSeed) // �õ� ��� ����
            seed = Time.time.ToString(); // �õ� ����

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); // �ǻ� ���� ����

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1) 
                    map[x, y] = WALL; // �����ڸ��� ������ ����
                else 
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; // ������ ���� �� �Ǵ� ��η� ����
            }
        }
    }

    // ���� �� ���� ����
    private void CreateOldMap() {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                oldMap[x, y] = map[x, y];
            }
        }
    }

    // ���� Ÿ�ϵ��� ��ȸ�ϸ� ������ �ٵ�� �޼���
    public void SmoothMap()
    {
        CreateOldMap(); // ���� �� ���� ����

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y); // �ֺ� �� Ÿ�� ���� ���
                if (neighbourWallTiles > referenceWallCount) 
                    map[x, y] = WALL; // �ֺ� ĭ �� ���� ���� �� Ÿ�� ������ �ʰ��� ��� ���� Ÿ���� ������ ����
                else if (neighbourWallTiles < referenceWallCount) 
                    map[x, y] = ROAD; // �ֺ� ĭ �� ���� ���� �� Ÿ�� ���� �̸��� ��� ���� Ÿ���� ��η� ����
                OnDrawTile(x, y, map[x, y]); // Ÿ�� ���� ����
            }
        }
    }

    // �ֺ��� �� Ÿ�� ������ ����ϴ� �޼���
    private int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        // ���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) 
        { 
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                // �� ���� �˻�
                if (neighbourX >= 0 && neighbourX < mapWidth && neighbourY >= 0 && neighbourY < mapHeight)
                {
                    // ���� �ٲ��� ���� oldMap ���� ���
                    if (neighbourX != x || neighbourY != y) 
                        wallCount += oldMap[neighbourX, neighbourY];
                }
                else  // �ֺ� Ÿ���� �� ������ ��� ���(���� Ÿ���� �����ڸ� Ÿ��)
                    wallCount++;
            }
        }
        return wallCount;
    }

    // Ÿ�� Ÿ�Կ� ���� Ÿ�� ����
    private void OnDrawTile(int x, int y, int tileType)
    {
        Vector3Int pos = new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0); // �߾� ����

        if (tileType == ROAD)
            tilemap.SetTile(pos, roadTile);
        else
            tilemap.SetTile(pos, wallTile);
    }

    // ���� �ʱ�ȭ �޼���
    public void ResetDungeon()
    {
        // Ÿ�ϸ��� ��� Ÿ�� ����
        tilemap.ClearAllTiles();       
    }
}
