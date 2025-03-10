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
        // �� ���� ����
        int normalEnemyCount = data.normalEnemyCount;
        int eliteEnemyCount = data.eliteEnemyCount;
        int interruptCount = data.interruptCount;

        // �Ϲ� �� ����
        for (int i = 0; i < normalEnemyCount; i++)
        {
            var enemy = _enemyPool.GetRandomNormal(data); // �Ϲ� �� Ǯ���� �������� ���� �ϳ� �����´�.          
            //enemy.gameObject.transform.position = new Vector3(Random.Range(-5, 5), 0, 0); // ���� ��ġ�� �����Ѵ�.
            enemy.gameObject.SetActive(true);
        }

        // ����Ʈ �� ����
        for (int i = 0; i < eliteEnemyCount; i++)
        {
            var enemy = _enemyPool.GetRandomElite(data); // ����Ʈ �� Ǯ���� �������� ���� �ϳ� �����´�.           
            //enemy.gameObject.transform.position = new Vector3(Random.Range(-5, 5), 0, 0); // ���� ��ġ�� �����Ѵ�.
            enemy.gameObject.SetActive(true);
        }

        // ���� ���� ����
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
