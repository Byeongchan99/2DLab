using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [Header("일반 적 풀 설정")]
    [SerializeField] private List<BaseEnemy> normalList;
    int normalCount = 10;
    public GameObject normalEnemyContainer;

    [Header("엘리트 적 풀 설정")]
    [SerializeField] private List<BaseEnemy> eliteList;
    int eliteCount = 3;
    public GameObject eliteEnemyContainer;

    // "어떤 프리팹" -> "해당 프리팹에 대한 Queue"
    private Dictionary<BaseEnemy, Queue<BaseEnemy>> normalPools
        = new Dictionary<BaseEnemy, Queue<BaseEnemy>>();

    private Dictionary<BaseEnemy, Queue<BaseEnemy>> elitePools
        = new Dictionary<BaseEnemy, Queue<BaseEnemy>>();

    private void Awake()
    {
        // 1) 일반 적 초기화
        foreach (var prefab in normalList)
        {
            // 프리팹별로 Queue 생성
            var queue = new Queue<BaseEnemy>();
            for (int i = 0; i < normalCount; i++)
            {
                var enemy = InstantiateNewEnemy(prefab, normalEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            normalPools.Add(prefab, queue);
        }

        // 2) 엘리트 적 초기화
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
    /// 일반 적 풀에서 "무작위" 프리팹으로 적을 하나 가져온다
    /// </summary>
    public BaseEnemy GetRandomNormal()
    {
        if (normalPools.Count == 0)
        {
            Debug.LogWarning("[EnemyPool] normalPools is empty!");
            return null;
        }

        // 1) normalPools.Keys를 배열이나 리스트로 꺼냄
        var prefabKeys = new List<BaseEnemy>(normalPools.Keys);

        // 2) 무작위 프리팹 선택
        int randomIndex = Random.Range(0, prefabKeys.Count);
        var chosenPrefab = prefabKeys[randomIndex];

        // 3) 해당 프리팹의 Queue에서 하나 Dequeue
        var queue = normalPools[chosenPrefab];
        if (queue.Count == 0)
        {
            // 비었으면 새로 하나 생성
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
    /// 엘리트 적 풀에서 "무작위" 프리팹으로 적을 하나 가져온다
    /// </summary>
    public BaseEnemy GetRandomElite()
    {
        if (elitePools.Count == 0)
        {
            Debug.LogWarning("[EnemyPool] elitePools is empty!");
            return null;
        }

        // 1) elitePools.Keys를 리스트로 가져오기
        var prefabKeys = new List<BaseEnemy>(elitePools.Keys);

        // 2) 무작위 프리팹 선택
        int randomIndex = Random.Range(0, prefabKeys.Count);
        var chosenPrefab = prefabKeys[randomIndex];

        // 3) Queue에서 하나 뽑아 활성화
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
    /// 적이 사망/비활성화될 때, 다시 풀로 반환
    /// 프리팹 참조를 알아야 함 -> BaseEnemy 스크립트 안에서 본인이 가진 "originPrefab" 등을 기억시킬 수도 있음.
    /// </summary>
    public void ReturnToNormalPool(BaseEnemy prefabReference, BaseEnemy enemy)
    {
        enemy.gameObject.SetActive(false);

        // 혹시 해당 prefabReference가 없으면, 그냥 ignore
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
    /// 단순히 Instantiate하면서 parent를 자기(transform)로 설정
    /// </summary>
    private BaseEnemy InstantiateNewEnemy(BaseEnemy prefab, GameObject container)
    {
        var newEnemy = Instantiate(prefab, transform, container);
        // 여기서 newEnemy에게 "내가 어떤 prefabReference에서 만들어졌는지" 기록시킬 수도 있음
        return newEnemy;
    }
}
