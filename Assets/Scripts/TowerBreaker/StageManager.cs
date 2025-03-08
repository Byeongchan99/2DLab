using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Reference")]
    StageSetting _stageSetting;

    List<StageData> _stageDataList; // 스테이지 데이터 리스트

    // 현재 스테이지 정보
    StageData _currentStageData;
    int _totalEnemyCount; // 총 적 수
    bool _hasChest; // 보물 상자가 있는지

    private void Awake()
    {
        if (_stageSetting == null)
        {
            _stageSetting = GetComponent<StageSetting>();
        }
    }

    /// <summary>
    /// 스테이지 데이터 기반으로 스테이지 시작
    /// </summary>
    /// <param name="data"></param>
    public void StartStage(StageData data)
    {
        // 적 세팅
        _stageSetting.SpawnEnemy(data);
        // 현재 스테이지 정보 설정
        _currentStageData.level = data.level;
        _currentStageData.normalEnemyCount = data.normalEnemyCount;
        _currentStageData.eliteEnemyCount = data.eliteEnemyCount;
        _currentStageData.interruptCount = data.interruptCount;

        // 보물상자 생성(50% 확률)
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
    /// 스테이지 레벨 기반으로 스테이지 시작
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
            Debug.LogError("해당 레벨의 StageData가 없습니다.");
        }
    }

    /// <summary>
    /// 적이 쓰러졌을 때 호출
    /// </summary>
    public void EnemyDestroyed()
    {
        _totalEnemyCount--;
    }

    /// <summary>
    /// 보물상자가 파괴되었을 때 호출
    /// </summary>
    public void ChestDestroyed()
    {
        _hasChest = false;
    }

    /// <summary>
    /// 스테이지 종료
    /// </summary>
    public void EndStage()
    {
        // 모든 적과 보물상자가 제거되었을 때 실행
        if (_totalEnemyCount == 0 && _hasChest == false)
        {
            // 스테이지 종료 코루틴 실행
            StartCoroutine(EndStageRoutine());
        }
        else
        {
            Debug.LogError("모든 적과 보물상자가 제거되지 않았습니다.");
        }
    }

    IEnumerator EndStageRoutine()
    {
        // 플레이어를 이번 층 끝으로 이동 후

        // 카메라 이동 + 플레이어를 다음 층 시작 지점으로 이동
        yield break;
    }
}
