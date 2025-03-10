using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [Header("일반 적 풀 설정")]
    [SerializeField] private List<BaseEnemy> normalList;
    int normalCount = 10;
    public Transform normalEnemyContainer;

    [Header("엘리트 적 풀 설정")]
    [SerializeField] private List<EliteEnemy> eliteList;
    int eliteCount = 3;
    public Transform eliteEnemyContainer;

    // "어떤 프리팹" -> "해당 프리팹에 대한 Queue"
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
        // 1) 일반 적 풀 초기화
        foreach (var prefab in normalList)
        {
            // 프리팹별로 Queue 생성
            var queue = new Queue<BaseEnemy>();
            for (int i = 0; i < normalCount; i++)
            {
                var enemy = Instantiate(prefab, normalEnemyContainer);
                enemy.gameObject.SetActive(false);
                queue.Enqueue(enemy);
            }
            normalPools.Add(prefab.normalEnemyType, queue);
        }

        // 2) 엘리트 적 풀 초기화
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
    /// 스테이지 정보에 있는 "등장 가능 일반 적 타입들" 중 무작위로 1개 뽑아 소환
    /// </summary>
    public BaseEnemy GetRandomNormal(StageData data)
    {
        var possibleTypes = data.normalEnemyTypes;   // 등장 가능 일반 적 타입 배열
        if (possibleTypes == null || possibleTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyPool] 이번 스테이지에 등장할 수 있는 적이 없습니다.");
            return null;
        }

        // 1) 무작위 타입 하나 골라
        int randomIndex = Random.Range(0, possibleTypes.Length);
        NormalEnemyType chosenType = possibleTypes[randomIndex];

        // 2) 그 타입에 해당하는 Queue가 있는지 확인
        if (!normalPools.ContainsKey(chosenType))
        {
            Debug.LogError($"[EnemyPool] normalPools에 {chosenType} 타입이 없습니다!");
            return null;
        }

        // 3) Queue에서 하나 꺼내기(비어 있으면 새로 Instantiate)
        Queue<BaseEnemy> queue = normalPools[chosenType];
        BaseEnemy enemy = null;

        if (queue.Count == 0)
        {
            // 이 경우는 초기 풀링 개수보다 더 많이 소환되었을 때 발생
            // 원한다면 여기서 새로 Instantiate 가능
            var prefab = normalList.Find(p => p.normalEnemyType == chosenType);
            if (prefab == null)
            {
                Debug.LogError($"[EnemyPool] normalList에 {chosenType} 프리팹이 없습니다!");
                return null;
            }
            enemy = Instantiate(prefab, normalEnemyContainer);
            enemy.normalEnemyType = chosenType;
        }
        else
        {
            enemy = queue.Dequeue();
        }

        // 4) StageData의 normalEnemyData를 참조해서 스탯 세팅
        if (data.normalEnemyData != null)
        {
            enemy.SetStat(data.normalEnemyData);
        }

        // 5) 활성화
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    /// <summary>
    /// 스테이지 정보에 있는 "등장 가능 엘리트 적 타입들" 중 무작위로 1개 뽑아 소환
    /// </summary>
    public EliteEnemy GetRandomElite(StageData data)
    {
        var possibleTypes = data.eliteEnemyTypes;
        if (possibleTypes == null || possibleTypes.Length == 0)
        {
            Debug.LogWarning("[EnemyPool] 이번 스테이지에 등장할 수 있는 적이 없습니다.");
            return null;
        }

        int randomIndex = Random.Range(0, possibleTypes.Length);
        EliteEnemyType chosenType = possibleTypes[randomIndex];

        if (!elitePools.ContainsKey(chosenType))
        {
            Debug.LogError($"[EnemyPool] elitePools에 {chosenType} 타입이 없습니다!");
            return null;
        }

        Queue<EliteEnemy> queue = elitePools[chosenType];
        EliteEnemy enemy = null;

        if (queue.Count == 0)
        {
            var prefab = eliteList.Find(p => p.eliteEnemyType == chosenType);
            if (prefab == null)
            {
                Debug.LogError($"[EnemyPool] eliteList에 {chosenType} 프리팹이 없습니다!");
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
    /// 일반 적을 다시 풀로 반환
    /// </summary>
    public void ReturnToNormalPool(BaseEnemy enemy)
    {
        // 1) 비활성화
        enemy.gameObject.SetActive(false);

        // 2) enemy.normalEnemyType 기반으로 Queue에 다시 Enqueue
        NormalEnemyType type = enemy.normalEnemyType;
        if (!normalPools.ContainsKey(type))
        {
            Debug.LogWarning($"[EnemyPool] {type}가 normalPools에 없음. 그냥 Destroy합니다.");
            Destroy(enemy.gameObject);
            return;
        }
        normalPools[type].Enqueue(enemy);
    }

    /// <summary>
    /// 엘리트 적을 다시 풀로 반환
    /// </summary>
    public void ReturnToElitePool(EliteEnemy enemy)
    {
        enemy.gameObject.SetActive(false);

        EliteEnemyType type = enemy.eliteEnemyType;
        if (!elitePools.ContainsKey(type))
        {
            Debug.LogWarning($"[EnemyPool] {type}가 elitePools에 없음. 그냥 Destroy합니다.");
            Destroy(enemy.gameObject);
            return;
        }
        elitePools[type].Enqueue(enemy);
    }
}
