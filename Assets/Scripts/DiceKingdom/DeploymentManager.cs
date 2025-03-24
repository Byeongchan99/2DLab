using UnityEngine;
using UnityEngine.Tilemaps;
using DiceKingdom;

public class DeploymentManager : MonoBehaviour
{
    public static DeploymentManager Instance;

    public Tilemap placementTilemap;      // 게임 월드의 배치 타일맵
    public GameObject towerPrefab;          // UI 타워를 게임 월드에 배치할 때 사용할 프리팹
    public Canvas mainCanvas;               // UI 캔버스

    private IDeployable selectedDeployable;

    private void Awake()
    {
        Instance = this;
    }

    // 타워 선택 시 호출 (UI 타워든 이미 배치된 타워든)
    public void SelectDeployable(IDeployable deployable)
    {
        selectedDeployable = deployable;
        selectedDeployable.OnButtonDown();
    }

    // 배치 확정 시 호출 (world 좌표를 인자로 받음)
    public void DeploySelectedTower(Vector3 worldPos)
    {
        if (selectedDeployable == null)
            return;

        Debug.Log($"드래그 종료 world 좌표: {worldPos}");
        Vector3Int cellPos = placementTilemap.WorldToCell(worldPos);
        Debug.Log($"배치 셀 위치: {cellPos}");
        TileBase placementTile = placementTilemap.GetTile(cellPos);

        if (placementTile != null)
        {
            Debug.Log($"배치 타일: {placementTile.name}");
            Vector3 targetPos = placementTilemap.GetCellCenterWorld(cellPos);
            // UI 타워와 게임 월드 타워를 구분
            if (selectedDeployable is TowerUI)
            {
                // UI 타워인 경우 실제 타워 프리팹을 인스턴스화하고 UI 타워는 제거
                Instantiate(towerPrefab, targetPos, Quaternion.identity);
                Destroy(((MonoBehaviour)selectedDeployable).gameObject);
            }
            else
            {
                // 이미 배치된 타워인 경우 위치 업데이트
                selectedDeployable.OnButtonUp(targetPos);
            }
        }
        else
        {
            Debug.Log("배치 타일 없음. 유효하지 않은 배치 위치.");
            selectedDeployable.OnButtonUp(Vector3.zero);
        }

        selectedDeployable = null;
    }
}
