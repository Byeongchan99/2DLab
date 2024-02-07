using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public GameObject targetPrefab; // Target ������
    private List<GameObject> targetsPool = new List<GameObject>(); // ������Ʈ Ǯ
    public int poolSize = 20; // Ǯ ������

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
        GameObject newObj = Instantiate(targetPrefab);
        newObj.SetActive(true);
        targetsPool.Add(newObj);
        return newObj;
    }

    public void ReturnTargetToPool(GameObject target)
    {
        target.SetActive(false); // ������Ʈ ��Ȱ��ȭ
    }
}
