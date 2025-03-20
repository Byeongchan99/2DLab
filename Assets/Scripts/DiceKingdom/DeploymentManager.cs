using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeploymentManager : MonoBehaviour
{
    public static DeploymentManager Instance;

    // ��ġ ������ Ÿ�ϸ�
    public Tilemap placementTilemap;

    // ���� ���õ� IDeployable (��: Tower)
    private IDeployable selectedDeployable;

    private void Awake()
    {
        Instance = this;
    }

    // Ÿ���� ������ �� ȣ��
    public void SelectDeployable(IDeployable deployable)
    {
        selectedDeployable = deployable;
        selectedDeployable.OnButtonDown();
    }

    // Ÿ�� ��ġ Ȯ�� �� ȣ�� (��: ���콺 ��ư�� ����)
    public void DeploySelectedTower(Vector3 pointerWorldPos)
    {
        if (selectedDeployable == null)
            return;

        // ��ġ Ÿ�� ����
        Vector3Int cellPos = placementTilemap.WorldToCell(pointerWorldPos);
        TileBase placementTile = placementTilemap.GetTile(cellPos);

        if (placementTile != null)
        {
            Debug.Log($"��ġ Ÿ��: {placementTile.name}");
            // ��ġ Ÿ���� ������ �ʿ�� ó��
            if (placementTile is CustomTile customPlacementTile)
            {
                LogCustomTileInfo(customPlacementTile);
            }

            // ��ġ ������ Ÿ���̹Ƿ� �ش� ���� �߽� ��ġ�� ��ġ ��ġ�� ���
            Vector3 targetPos = placementTilemap.GetCellCenterWorld(cellPos);
            selectedDeployable.OnButtonUp(targetPos);
        }
        else
        {
            Debug.Log("��ġ Ÿ�� ����. ��ġ�� ��ȿ���� �ʽ��ϴ�.");
            // ��ȿ���� ���� ��ġ�� ���� ��ġ�� �����ϵ��� ó���ϰų� ���� �޽��� ���
            selectedDeployable.OnButtonUp(Vector3.zero);
        }

        selectedDeployable = null;
    }

    private void LogCustomTileInfo(CustomTile tile)
    {
        Debug.Log($"Ÿ�� ����: {tile.tileData.tileType}");
        if (tile.tileData.effects != null)
        {
            foreach (var effect in tile.tileData.effects)
            {
                Debug.Log($"Ÿ�� ȿ��: {effect.name}");
            }
        }
    }
}
