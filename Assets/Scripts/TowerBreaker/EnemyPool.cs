using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("�Ϲ� �� Ǯ ����")]
    [SerializeField] private List<BaseEnemy> normalList;
    int normalCount = 10;
    public Transform normalEnemyContainer;

    [Header("����Ʈ �� Ǯ ����")]
    [SerializeField] private List<EliteEnemy> eliteList;
    int eliteCount = 3;
    public Transform eliteEnemyContainer;

    // "� ������" -> "�ش� �����տ� ���� Queue"
    private Dictionary<NormalEnemyType, Queue<BaseEnemy>> normalPools
        = new Dictionary<NormalEnemyType, Queue<BaseEnemy>>();

    private Dictionary<EliteEnemyType, Queue<EliteEnemy>> elitePools
        = new Dictionary<EliteEnemyType, Queue<EliteEnemy>>();

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

        Init();
    }

    private void Init()
    {
        // 1) �Ϲ� �� Ǯ �ʱ�ȭ
        foreach (var prefab in normalList)
        {
            // �����պ��� Queue ����
            var queue = new Queue<BaseEnemy>();
            for (int i = 0; i < normalCount; i++)
            {
                var enemy = Instantiate(prefab, normalEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            normalPools.Add(prefab.normalEnemyType, queue);
        }

        // 2) ����Ʈ �� Ǯ �ʱ�ȭ
        foreach (var prefab in eliteList)
        {
            var queue = new Queue<EliteEnemy>();
            for (int i = 0; i < eliteCount; i++)
            {
                var enemy = Instantiate(prefab, eliteEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            elitePools.Add(prefab.eliteEnemyType, queue);
        }
    }

    /// <summary>
    /// �������� ������ �ִ� "���� ���� �Ϲ� �� Ÿ�Ե�" �� �������� 1�� �̾� ��ȯ
    /// </summary>
    public BaseEnemy GetRandomNormal(StageData data)
    {
        var possibleTypes = data.normalEnemyTypes;   // ���� ���� �Ϲ� �� Ÿ�� �迭
        if (possibleTypes == null || possibleTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyPool] �̹� ���������� ������ �� �ִ� ���� �����ϴ�.");
            return null;
        }

        // 1) ������ Ÿ�� �ϳ� ���
        int randomIndex = Random.Range(0, possibleTypes.Length);
        NormalEnemyType chosenType = possibleTypes[randomIndex];

        // 2) �� Ÿ�Կ� �ش��ϴ� Queue�� �ִ��� Ȯ��
        if (!normalPools.ContainsKey(chosenType))
        {
            Debug.LogError($"[EnemyPool] normalPools�� {chosenType} Ÿ���� �����ϴ�!");
            return null;
        }

        // 3) Queue���� �ϳ� ������(��� ������ ���� Instantiate)
        Queue<BaseEnemy> queue = normalPools[chosenType];
        BaseEnemy enemy = null;

        if (queue.Count == 0)
        {
            // �� ���� �ʱ� Ǯ�� �������� �� ���� ��ȯ�Ǿ��� �� �߻�
            // ���Ѵٸ� ���⼭ ���� Instantiate ����
            var prefab = normalList.Find(p => p.normalEnemyType == chosenType);
            if (prefab == null)
            {
                Debug.LogError($"[EnemyPool] normalList�� {chosenType} �������� �����ϴ�!");
                return null;
            }
            enemy = Instantiate(prefab, normalEnemyContainer);
            enemy.normalEnemyType = chosenType;
        }
        else
        {
            enemy = queue.Dequeue();
        }

        // 4) StageData�� normalEnemyData�� �����ؼ� ���� ����
        if (data.normalEnemyData != null)
        {
            enemy.SetStat(data.normalEnemyData);
        }

        // 5) Ȱ��ȭ
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    /// <summary>
    /// �������� ������ �ִ� "���� ���� ����Ʈ �� Ÿ�Ե�" �� �������� 1�� �̾� ��ȯ
    /// </summary>
    public EliteEnemy GetRandomElite(StageData data)
    {
        var possibleTypes = data.eliteEnemyTypes;
        if (possibleTypes == null || possibleTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyPool] �̹� ���������� ������ �� �ִ� ���� �����ϴ�.");
            return null;
        }

        int randomIndex = Random.Range(0, possibleTypes.Length);
        EliteEnemyType chosenType = possibleTypes[randomIndex];

        if (!elitePools.ContainsKey(chosenType))
        {
            Debug.LogError($"[EnemyPool] elitePools�� {chosenType} Ÿ���� �����ϴ�!");
            return null;
        }

        Queue<EliteEnemy> queue = elitePools[chosenType];
        EliteEnemy enemy = null;

        if (queue.Count == 0)
        {
            var prefab = eliteList.Find(p => p.eliteEnemyType == chosenType);
            if (prefab == null)
            {
                Debug.LogError($"[EnemyPool] eliteList�� {chosenType} �������� �����ϴ�!");
                return null;
            }
            enemy = Instantiate(prefab, eliteEnemyContainer);
            enemy.eliteEnemyType = chosenType;
        }
        else
        {
            enemy = queue.Dequeue();
        }

        if (data.eliteEnemyData != null)
        {
            enemy.SetStat(data.eliteEnemyData);
        }

        enemy.gameObject.SetActive(true);
        return enemy;
    }

    /// <summary>
    /// �Ϲ� ���� �ٽ� Ǯ�� ��ȯ
    /// </summary>
    public void ReturnToNormalPool(BaseEnemy enemy)
    {
        // 1) ��Ȱ��ȭ
        enemy.gameObject.SetActive(false);

        // 2) enemy.normalEnemyType ������� Queue�� �ٽ� Enqueue
        NormalEnemyType type = enemy.normalEnemyType;
        if (!normalPools.ContainsKey(type))
        {
            Debug.LogWarning($"[EnemyPool] {type}�� normalPools�� ����. �׳� Destroy�մϴ�.");
            Destroy(enemy.gameObject);
            return;
        }
        normalPools[type].Enqueue(enemy);
    }

    /// <summary>
    /// ����Ʈ ���� �ٽ� Ǯ�� ��ȯ
    /// </summary>
    public void ReturnToElitePool(EliteEnemy enemy)
    {
        enemy.gameObject.SetActive(false);

        EliteEnemyType type = enemy.eliteEnemyType;
        if (!elitePools.ContainsKey(type))
        {
            Debug.LogWarning($"[EnemyPool] {type}�� elitePools�� ����. �׳� Destroy�մϴ�.");
            Destroy(enemy.gameObject);
            return;
        }
        elitePools[type].Enqueue(enemy);
    }
}
