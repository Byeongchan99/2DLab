using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeploymentManager : MonoBehaviour
{
    public static DeploymentManager Instance;

    // 배치 가능한 타일맵
    public Tilemap placementTilemap;

    // 현재 선택된 IDeployable (예: Tower)
    private IDeployable selectedDeployable;

    private void Awake()
    {
        Instance = this;
    }

    // 타워를 선택할 때 호출
    public void SelectDeployable(IDeployable deployable)
    {
        selectedDeployable = deployable;
        selectedDeployable.OnButtonDown();
    }

    // 타워 배치 확정 시 호출 (예: 마우스 버튼을 떼면)
    public void DeploySelectedTower(Vector3 pointerWorldPos)
    {
        if (selectedDeployable == null)
            return;

        // 배치 타일 검증
        Vector3Int cellPos = placementTilemap.WorldToCell(pointerWorldPos);
        TileBase placementTile = placementTilemap.GetTile(cellPos);

        if (placementTile != null)
        {
            Debug.Log($"배치 타일: {placementTile.name}");
            // 배치 타일의 정보를 필요시 처리
            if (placementTile is CustomTile customPlacementTile)
            {
                LogCustomTileInfo(customPlacementTile);
            }

            // 배치 가능한 타일이므로 해당 셀의 중심 위치를 배치 위치로 사용
            Vector3 targetPos = placementTilemap.GetCellCenterWorld(cellPos);
            selectedDeployable.OnButtonUp(targetPos);
        }
        else
        {
            Debug.Log("배치 타일 없음. 배치가 유효하지 않습니다.");
            // 유효하지 않은 위치면 원래 위치로 복귀하도록 처리하거나 오류 메시지 출력
            selectedDeployable.OnButtonUp(Vector3.zero);
        }

        selectedDeployable = null;
    }

    private void LogCustomTileInfo(CustomTile tile)
    {
        Debug.Log($"타일 종류: {tile.tileData.tileType}");
        if (tile.tileData.effects != null)
        {
            foreach (var effect in tile.tileData.effects)
            {
                Debug.Log($"타일 효과: {effect.name}");
            }
        }
    }
}
