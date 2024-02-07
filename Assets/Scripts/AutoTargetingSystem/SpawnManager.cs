using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnInterval = 3f; // 타겟을 소환하는 시간 간격
    private float spawnTimer; // 다음 소환까지의 타이머

    void Update()
    {
        // 타이머 업데이트
        spawnTimer += Time.deltaTime;

        // 일정 시간이 지나거나 활성화된 타겟이 없으면 타겟 소환
        if (spawnTimer >= spawnInterval || !IsTargetActive())
        {
            SpawnTarget();
            spawnTimer = 0; // 타이머 초기화
        }
    }

    void SpawnTarget()
    {
        GameObject target = ObjectPoolManager.Instance.GetTargetFromPool();
        // 위치 설정
        target.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-3f, 4f), Random.Range(0f, 70f));
        AdjustScaleBasedOnZAxis(target); // Z축에 따라 스케일 조정
    }

    // Z축에 따라 스케일 조정
    void AdjustScaleBasedOnZAxis(GameObject target)
    {
        float z = target.transform.position.z;

        if (z > 0 && z < 30)
        {
            target.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else if (z > 30 && z < 50)
        {
            target.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
        else if (z > 50 && z < 60)
        {
            target.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        }
        else if (z > 60 && z < 70)
        {
            target.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        }
    }

    // 활성화된 타겟이 존재하는지 체크
    bool IsTargetActive()
    {
        foreach (GameObject target in ObjectPoolManager.Instance.GetActiveTargets())
        {
            if (target.activeInHierarchy)
            {
                return true; // 활성화된 타겟이 하나라도 있으면 true 반환
            }
        }
        return false; // 활성화된 타겟이 없으면 false 반환
    }
}
