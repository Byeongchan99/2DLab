using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Reference")]
    StageSetting _stageSetting;

    List<StageData> _stageDataList; // �������� ������ ����Ʈ

    // ���� �������� ����
    StageData _currentStageData;
    int _totalEnemyCount; // �� �� ��
    bool _hasChest; // ���� ���ڰ� �ִ���

    private void Awake()
    {
        if (_stageSetting == null)
        {
            _stageSetting = GetComponent<StageSetting>();
        }
    }

    /// <summary>
    /// �������� ������ ������� �������� ����
    /// </summary>
    /// <param name="data"></param>
    public void StartStage(StageData data)
    {
        // �� ����
        _stageSetting.SpawnEnemy(data);
        // ���� �������� ���� ����
        _currentStageData.level = data.level;
        _currentStageData.normalEnemyCount = data.normalEnemyCount;
        _currentStageData.eliteEnemyCount = data.eliteEnemyCount;
        _currentStageData.interruptCount = data.interruptCount;

        // �������� ����(50% Ȯ��)
        if (Random.Range(0, 2) == 0)
        {
            _stageSetting.SpawnChest();
            _hasChest = true;
        }
        else
        {
            _hasChest = false;
        }
    }

    /// <summary>
    /// �������� ���� ������� �������� ����
    /// </summary> 
    public void StartStage(int level)
    {
        StageData data = _stageDataList.Find(x => x.level == level);
        if (data != null)
        {
            StartStage(data);
        }
        else
        {
            Debug.LogError("�ش� ������ StageData�� �����ϴ�.");
        }
    }

    /// <summary>
    /// ���� �������� �� ȣ��
    /// </summary>
    public void EnemyDestroyed()
    {
        _totalEnemyCount--;
    }

    /// <summary>
    /// �������ڰ� �ı��Ǿ��� �� ȣ��
    /// </summary>
    public void ChestDestroyed()
    {
        _hasChest = false;
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    public void EndStage()
    {
        // ��� ���� �������ڰ� ���ŵǾ��� �� ����
        if (_totalEnemyCount == 0 && _hasChest == false)
        {
            // �������� ���� �ڷ�ƾ ����
            StartCoroutine(EndStageRoutine());
        }
        else
        {
            Debug.LogError("��� ���� �������ڰ� ���ŵ��� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator EndStageRoutine()
    {
        // �÷��̾ �̹� �� ������ �̵� ��

        // ī�޶� �̵� + �÷��̾ ���� �� ���� �������� �̵�
        yield break;
    }
}
