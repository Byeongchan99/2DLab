using UnityEngine;
using UnityEngine.Tilemaps;
using DiceKingdom;

public class DeploymentManager : MonoBehaviour
{
    public static DeploymentManager Instance;

    public Tilemap placementTilemap;      // ���� ������ ��ġ Ÿ�ϸ�
    public GameObject towerPrefab;          // UI Ÿ���� ���� ���忡 ��ġ�� �� ����� ������
    public Canvas mainCanvas;               // UI ĵ����

    private IDeployable selectedDeployable;

    private void Awake()
    {
        Instance = this;
    }

    // Ÿ�� ���� �� ȣ�� (UI Ÿ���� �̹� ��ġ�� Ÿ����)
    public void SelectDeployable(IDeployable deployable)
    {
        selectedDeployable = deployable;
        selectedDeployable.OnButtonDown();
    }

    // ��ġ Ȯ�� �� ȣ�� (world ��ǥ�� ���ڷ� ����)
    public void DeploySelectedTower(Vector3 worldPos)
    {
        if (selectedDeployable == null)
            return;

        Debug.Log($"�巡�� ���� world ��ǥ: {worldPos}");
        Vector3Int cellPos = placementTilemap.WorldToCell(worldPos);
        Debug.Log($"��ġ �� ��ġ: {cellPos}");
        TileBase placementTile = placementTilemap.GetTile(cellPos);

        if (placementTile != null)
        {
            Debug.Log($"��ġ Ÿ��: {placementTile.name}");
            Vector3 targetPos = placementTilemap.GetCellCenterWorld(cellPos);
            // UI Ÿ���� ���� ���� Ÿ���� ����
            if (selectedDeployable is TowerUI)
            {
                // UI Ÿ���� ��� ���� Ÿ�� �������� �ν��Ͻ�ȭ�ϰ� UI Ÿ���� ����
                Instantiate(towerPrefab, targetPos, Quaternion.identity);
                Destroy(((MonoBehaviour)selectedDeployable).gameObject);
            }
            else
            {
                // �̹� ��ġ�� Ÿ���� ��� ��ġ ������Ʈ
                selectedDeployable.OnButtonUp(targetPos);
            }
        }
        else
        {
            Debug.Log("��ġ Ÿ�� ����. ��ȿ���� ���� ��ġ ��ġ.");
            selectedDeployable.OnButtonUp(Vector3.zero);
        }

        selectedDeployable = null;
    }
}
