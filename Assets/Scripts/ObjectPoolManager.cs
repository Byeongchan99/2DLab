using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject targetPrefab; // Target 프리팹
    private List<GameObject> targetsPool = new List<GameObject>(); // 오브젝트 풀
    public int poolSize = 20; // 풀 사이즈

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(targetPrefab);
            obj.SetActive(false); // 초기에는 비활성화
            targetsPool.Add(obj);
        }
    }

    public GameObject GetTargetFromPool()
    {
        foreach (GameObject obj in targetsPool)
        {
            if (!obj.activeInHierarchy) // 비활성화 상태인 오브젝트 찾기
            {
                obj.SetActive(true); // 활성화
                return obj;
            }
        }

        // 모든 오브젝트가 사용 중이면 추가 생성 (선택적)
        GameObject newObj = Instantiate(targetPrefab);
        newObj.SetActive(true);
        targetsPool.Add(newObj);
        return newObj;
    }

    public void ReturnTargetToPool(GameObject target)
    {
        target.SetActive(false); // 오브젝트 비활성화
    }
}
