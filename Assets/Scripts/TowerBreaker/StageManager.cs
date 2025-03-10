using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("Reference")]
    StageSetting _stageSetting;

    List<StageData> _stageDataList; // 스테이지 데이터 리스트

    // 현재 스테이지 정보
    StageData _currentStageData;
    int _totalEnemyCount; // 총 적 수
    bool _hasChest; // 보물 상자가 있는지

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
    /// 스테이지 데이터 기반으로 스테이지 설정
    /// </summary>
    /// <param name="data"></param>
    public void SetStage(StageData data)
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
    /// 스테이지 레벨 기반으로 스테이지 초기화
    /// </summary> 
    public void InitStage(int level)
    {
        StageData data = GetStageData(level);
        if (data != null)
        {
            SetStage(data);
            // 캐릭터를 시작 위치로 이동시키는 로직
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
        if (_totalEnemyCount == 0)
        {
            // 보물상자 콜라이더 활성화
        }
    }

    /// <summary>
    /// 보물상자가 파괴되었을 때 호출
    /// </summary>
    public void ChestDestroyed()
    {
        _hasChest = false;
        // 아이템 획득 로직
        EndStage();
    }

    /// <summary>
    /// 스테이지 종료 후 일시정지 버튼을 제외한 화면에서 입력이 감지되었을 때 호출
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
        yield return new WaitForSeconds(1f);

        // 다음 스테이지 세팅
        StageData nextStageDate = GetStageData(_currentStageData.level + 1);
        SetStage(nextStageDate);

        // 플레이어를 다음 층 스테이지로 이동 및 카메라 이동

        yield break;
    }

    /// <summary>
    /// level에 해당하는 StageData 반환
    /// </summary>
    StageData GetStageData(int level)
    {
        return _stageDataList.Find(x => x.level == level);
    }
}
