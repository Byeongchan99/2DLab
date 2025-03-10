using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Reference")]
    StageSetting _stageSetting;

    List<StageData> _stageDataList; // �������� ������ ����Ʈ

    // ���� �������� ����
    StageData _currentStageData;
    int _totalEnemyCount; // �� �� ��
    bool _hasChest; // ���� ���ڰ� �ִ���

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_stageSetting == null)
        {
            _stageSetting = GetComponent<StageSetting>();
        }
    }

    /// <summary>
    /// �������� ������ ������� �������� ����
    /// </summary>
    /// <param name="data"></param>
    public void SetStage(StageData data)
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
    /// �������� ���� ������� �������� �ʱ�ȭ
    /// </summary> 
    public void InitStage(int level)
    {
        StageData data = GetStageData(level);
        if (data != null)
        {
            SetStage(data);
            // ĳ���͸� ���� ��ġ�� �̵���Ű�� ����
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
        if (_totalEnemyCount == 0)
        {
            // �������� �ݶ��̴� Ȱ��ȭ
        }
    }

    /// <summary>
    /// �������ڰ� �ı��Ǿ��� �� ȣ��
    /// </summary>
    public void ChestDestroyed()
    {
        _hasChest = false;
        // ������ ȹ�� ����
        EndStage();
    }

    /// <summary>
    /// �������� ���� �� �Ͻ����� ��ư�� ������ ȭ�鿡�� �Է��� �����Ǿ��� �� ȣ��
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
        yield return new WaitForSeconds(1f);

        // ���� �������� ����
        StageData nextStageDate = GetStageData(_currentStageData.level + 1);
        SetStage(nextStageDate);

        // �÷��̾ ���� �� ���������� �̵� �� ī�޶� �̵�

        yield break;
    }

    /// <summary>
    /// level�� �ش��ϴ� StageData ��ȯ
    /// </summary>
    StageData GetStageData(int level)
    {
        return _stageDataList.Find(x => x.level == level);
    }
}
