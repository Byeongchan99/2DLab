using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetting : MonoBehaviour
{
    [Header("Reference")]
    StageManager _stageManager;
    EnemyPool _enemyPool;
    GameObject[] _normalEnemyPrefabs;
    GameObject[] _eliteEnemyPrefabs;
    GameObject[] _interruptPrefabs;
    GameObject[] _chestPrefabs;

    private void Awake()
    {
        if (_stageManager == null)
        {
            _stageManager = GetComponent<StageManager>();
        }
    }

    public void SpawnEnemy(StageData data)
    {
        // 적 생성 로직
        int normalEnemyCount = data.normalEnemyCount;
        int eliteEnemyCount = data.eliteEnemyCount;
        int interruptCount = data.interruptCount;

        // 일반 적 생성
        for (int i = 0; i < normalEnemyCount; i++)
        {
            var enemy = _enemyPool.GetRandomNormal(data); // 일반 적 풀에서 무작위로 적을 하나 가져온다.          
            //enemy.gameObject.transform.position = new Vector3(Random.Range(-5, 5), 0, 0); // 적의 위치를 설정한다.
            enemy.gameObject.SetActive(true);
        }

        // 엘리트 적 생성
        for (int i = 0; i < eliteEnemyCount; i++)
        {
            var enemy = _enemyPool.GetRandomElite(data); // 엘리트 적 풀에서 무작위로 적을 하나 가져온다.           
            //enemy.gameObject.transform.position = new Vector3(Random.Range(-5, 5), 0, 0); // 적의 위치를 설정한다.
            enemy.gameObject.SetActive(true);
        }

        // 방해 마법 생성
        for (int i = 0; i < interruptCount; i++)
        {
            
        }
    }

    public void SpawnChest()
    {
        GameObject chest = Instantiate(_chestPrefabs[Random.Range(0, _chestPrefabs.Length)]);
    }

    public void ActiveChest()
    {

    }
}
