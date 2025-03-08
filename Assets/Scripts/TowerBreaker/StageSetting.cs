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
        // �� ���� ����
        int normalEnemyCount = data.normalEnemyCount;
        int eliteEnemyCount = data.eliteEnemyCount;
        int interruptCount = data.interruptCount;

        // �Ϲ� �� ����
        for (int i = 0; i < normalEnemyCount; i++)
        {
            GameObject enemy = Instantiate(_normalEnemyPrefabs[Random.Range(0, _normalEnemyPrefabs.Length)]);
            //enemy.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }

        // ����Ʈ �� ����
        for (int i = 0; i < eliteEnemyCount; i++)
        {
            GameObject enemy = Instantiate(_eliteEnemyPrefabs[Random.Range(0, _eliteEnemyPrefabs.Length)]);
            //enemy.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }

        // ���� ���� ����
        for (int i = 0; i < interruptCount; i++)
        {
            // ���� ������ ��� ������ �� �� �� �Ŀ� �����̵��� �����Ѵ�.
            GameObject interrupt = Instantiate(_interruptPrefabs[Random.Range(0, _interruptPrefabs.Length)]);
            //interrupt.transform.position = new Vector3(Random.Range(-5, 5), 0, 0);
        }
    }

    public void SpawnChest()
    {
        GameObject chest = Instantiate(_chestPrefabs[Random.Range(0, _chestPrefabs.Length)]);
    }
}
