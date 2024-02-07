using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject targetPrefab; // Target ������
    private List<GameObject> targetsPool = new List<GameObject>(); // ������Ʈ Ǯ
    public int poolSize = 20; // Ǯ ������
    public Transform targetPool; // ������Ʈ Ǯ�� ������ �θ� ������Ʈ

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(targetPrefab, targetPool);
            obj.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
            targetsPool.Add(obj);
        }
    }

    public GameObject GetTargetFromPool()
    {
        foreach (GameObject obj in targetsPool)
        {
            if (!obj.activeInHierarchy) // ��Ȱ��ȭ ������ ������Ʈ ã��
            {
                obj.SetActive(true); // Ȱ��ȭ
                return obj;
            }
        }

        // ��� ������Ʈ�� ��� ���̸� �߰� ���� (������)
        GameObject newObj = Instantiate(targetPrefab, targetPool);
        newObj.SetActive(true);
        targetsPool.Add(newObj);
        return newObj;
    }

    public List<GameObject> GetActiveTargets()
    {
        List<GameObject> activeTargets = new List<GameObject>();
        foreach (GameObject obj in targetsPool)
        {
            if (obj.activeInHierarchy)
            {
                activeTargets.Add(obj);
            }
        }
        return activeTargets;
    }

    public void ReturnTargetToPool(GameObject target)
    {
        target.SetActive(false); // ������Ʈ ��Ȱ��ȭ
    }
}
