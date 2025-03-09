using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("�Ϲ� �� Ǯ ����")]
    [SerializeField] private List<BaseEnemy> normalList;
    int normalCount = 10;
    public GameObject normalEnemyContainer;

    [Header("����Ʈ �� Ǯ ����")]
    [SerializeField] private List<BaseEnemy> eliteList;
    int eliteCount = 3;
    public GameObject eliteEnemyContainer;

    // "� ������" -> "�ش� �����տ� ���� Queue"
    private Dictionary<BaseEnemy, Queue<BaseEnemy>> normalPools
        = new Dictionary<BaseEnemy, Queue<BaseEnemy>>();

    private Dictionary<BaseEnemy, Queue<BaseEnemy>> elitePools
        = new Dictionary<BaseEnemy, Queue<BaseEnemy>>();

    private void Awake()
    {
        // 1) �Ϲ� �� �ʱ�ȭ
        foreach (var prefab in normalList)
        {
            // �����պ��� Queue ����
            var queue = new Queue<BaseEnemy>();
            for (int i = 0; i < normalCount; i++)
            {
                var enemy = InstantiateNewEnemy(prefab, normalEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            normalPools.Add(prefab, queue);
        }

        // 2) ����Ʈ �� �ʱ�ȭ
        foreach (var prefab in eliteList)
        {
            var queue = new Queue<BaseEnemy>();
            for (int i = 0; i < eliteCount; i++)
            {
                var enemy = InstantiateNewEnemy(prefab, eliteEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            elitePools.Add(prefab, queue);
        }
    }

    /// <summary>
    /// �Ϲ� �� Ǯ���� "������" ���������� ���� �ϳ� �����´�
    /// </summary>
    public BaseEnemy GetRandomNormal()
    {
        if (normalPools.Count == 0)
        {
            Debug.LogWarning("[EnemyPool] normalPools is empty!");
            return null;
        }

        // 1) normalPools.Keys�� �迭�̳� ����Ʈ�� ����
        var prefabKeys = new List<BaseEnemy>(normalPools.Keys);

        // 2) ������ ������ ����
        int randomIndex = Random.Range(0, prefabKeys.Count);
        var chosenPrefab = prefabKeys[randomIndex];

        // 3) �ش� �������� Queue���� �ϳ� Dequeue
        var queue = normalPools[chosenPrefab];
        if (queue.Count == 0)
        {
            // ������� ���� �ϳ� ����
            return InstantiateNewEnemy(chosenPrefab, normalEnemyContainer);
        }
        else
        {
            var enemy = queue.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
    }

    /// <summary>
    /// ����Ʈ �� Ǯ���� "������" ���������� ���� �ϳ� �����´�
    /// </summary>
    public BaseEnemy GetRandomElite()
    {
        if (elitePools.Count == 0)
        {
            Debug.LogWarning("[EnemyPool] elitePools is empty!");
            return null;
        }

        // 1) elitePools.Keys�� ����Ʈ�� ��������
        var prefabKeys = new List<BaseEnemy>(elitePools.Keys);

        // 2) ������ ������ ����
        int randomIndex = Random.Range(0, prefabKeys.Count);
        var chosenPrefab = prefabKeys[randomIndex];

        // 3) Queue���� �ϳ� �̾� Ȱ��ȭ
        var queue = elitePools[chosenPrefab];
        if (queue.Count == 0)
        {
            return InstantiateNewEnemy(chosenPrefab, eliteEnemyContainer);
        }
        else
        {
            var enemy = queue.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }
    }

    /// <summary>
    /// ���� ���/��Ȱ��ȭ�� ��, �ٽ� Ǯ�� ��ȯ
    /// ������ ������ �˾ƾ� �� -> BaseEnemy ��ũ��Ʈ �ȿ��� ������ ���� "originPrefab" ���� ����ų ���� ����.
    /// </summary>
    public void ReturnToNormalPool(BaseEnemy prefabReference, BaseEnemy enemy)
    {
        enemy.gameObject.SetActive(false);

        // Ȥ�� �ش� prefabReference�� ������, �׳� ignore
        if (normalPools.ContainsKey(prefabReference))
        {
            normalPools[prefabReference].Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning($"[EnemyPool] {prefabReference} not found in normalPools. Destroying object.");
            Destroy(enemy.gameObject);
        }
    }

    public void ReturnToElitePool(BaseEnemy prefabReference, BaseEnemy enemy)
    {
        enemy.gameObject.SetActive(false);

        if (elitePools.ContainsKey(prefabReference))
        {
            elitePools[prefabReference].Enqueue(enemy);
        }
        else
        {
            Debug.LogWarning($"[EnemyPool] {prefabReference} not found in elitePools. Destroying object.");
            Destroy(enemy.gameObject);
        }
    }

    /// <summary>
    /// �ܼ��� Instantiate�ϸ鼭 parent�� �ڱ�(transform)�� ����
    /// </summary>
    private BaseEnemy InstantiateNewEnemy(BaseEnemy prefab, GameObject container)
    {
        var newEnemy = Instantiate(prefab, transform, container);
        // ���⼭ newEnemy���� "���� � prefabReference���� �����������" ��Ͻ�ų ���� ����
        return newEnemy;
    }
}
