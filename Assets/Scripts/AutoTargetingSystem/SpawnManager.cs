using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnInterval = 3f; // Ÿ���� ��ȯ�ϴ� �ð� ����
    private float spawnTimer; // ���� ��ȯ������ Ÿ�̸�

    void Update()
    {
        // Ÿ�̸� ������Ʈ
        spawnTimer += Time.deltaTime;

        // ���� �ð��� �����ų� Ȱ��ȭ�� Ÿ���� ������ Ÿ�� ��ȯ
        if (spawnTimer >= spawnInterval || !IsTargetActive())
        {
            SpawnTarget();
            spawnTimer = 0; // Ÿ�̸� �ʱ�ȭ
        }
    }

    void SpawnTarget()
    {
        GameObject target = ObjectPoolManager.Instance.GetTargetFromPool();
        // ��ġ ����
        target.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-3f, 4f), Random.Range(0f, 70f));
        AdjustScaleBasedOnZAxis(target); // Z�࿡ ���� ������ ����
    }

    // Z�࿡ ���� ������ ����
    void AdjustScaleBasedOnZAxis(GameObject target)
    {
        float z = target.transform.position.z;

        if (z > 0 && z < 30)
        {
            target.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else if (z > 30 && z < 50)
        {
            target.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
        else if (z > 50 && z < 60)
        {
            target.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        }
        else if (z > 60 && z < 70)
        {
            target.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        }
    }

    // Ȱ��ȭ�� Ÿ���� �����ϴ��� üũ
    bool IsTargetActive()
    {
        foreach (GameObject target in ObjectPoolManager.Instance.GetActiveTargets())
        {
            if (target.activeInHierarchy)
            {
                return true; // Ȱ��ȭ�� Ÿ���� �ϳ��� ������ true ��ȯ
            }
        }
        return false; // Ȱ��ȭ�� Ÿ���� ������ false ��ȯ
    }
}
