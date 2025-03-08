using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetting : MonoBehaviour
{
    [Header("Reference")]
    StageManager _stageManager;
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
        // 利 积己 肺流
        int normalEnemyCount = data.normalEnemyCount;
        int eliteEnemyCount = data.eliteEnemyCount;
        int interruptCount = data.interruptCount;

        // 老馆 利 积己
        for (int i = 0; i < normalEnemyCount; i++)
        {
            GameObject enemy = Instantiate(_normalEnemyPrefabs[Random.Range(0, _normalEnemyPrefabs.Length)]);
            //enemy.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }

        // 郡府飘 利 积己
        for (int i = 0; i < eliteEnemyCount; i++)
        {
            GameObject enemy = Instantiate(_eliteEnemyPrefabs[Random.Range(0, _eliteEnemyPrefabs.Length)]);
            //enemy.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }

        // 规秦 付过 积己
        for (int i = 0; i < interruptCount; i++)
        {
            // 规秦 付过狼 版快 积己等 饶 割 檬 饶俊 框流捞档废 备泅茄促.
            GameObject interrupt = Instantiate(_interruptPrefabs[Random.Range(0, _interruptPrefabs.Length)]);
            //interrupt.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }
    }

    public void SpawnChest()
    {
        GameObject chest = Instantiate(_chestPrefabs[Random.Range(0, _chestPrefabs.Length)]);
    }
}
