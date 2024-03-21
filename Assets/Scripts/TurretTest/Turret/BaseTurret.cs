using System.Collections;
using System.Collections.Generic;
using TurretTest;
using UnityEngine;

public abstract class BaseTurret : MonoBehaviour
{
    public TurretSpawner spawner; // TurretSpawner ����

    public float lifeTime; // �ͷ� ���� �ð�
    public int projectileCount; // ����ü �߻� ����
    public int spawnPointIndex; // ��ȯ ��ġ
    public Transform firePoint; // �߻� ��ġ
    public Transform targetPosition; // ��ǥ ��ġ

    protected float attackSpeed; // ���� �ӵ�
    protected float timeSinceLastShot = 0f; // ������ �߻� ���� ��� �ð�

    public GameObject[] projectilePrefabs; // �߻��� ����ü ������ ����Ʈ
    protected GameObject currentProjectilePrefabs; // ���� �߻��� ����ü ������

    void Start()
    {
        InitTurret();
    }

    void Update()
    {
        if (ShouldShoot())
        {
            Shoot();
            timeSinceLastShot = 0f;
        }

        timeSinceLastShot += Time.deltaTime;

        if (lifeTime <= 0)
        {
            DisableTurret();
        }

        lifeTime -= Time.deltaTime;
    }

    protected bool ShouldShoot()
    {
        return timeSinceLastShot >= attackSpeed;
    }

    // �ʱ�ȭ
    protected virtual void InitTurret()
    {
        attackSpeed = lifeTime / projectileCount;
        currentProjectilePrefabs = projectilePrefabs[1];
        targetPosition = PlayerStat.Instance.transform;
    }

    protected abstract void Shoot();

    protected void DisableTurret()
    {
        // �ͷ� ��Ȱ��ȭ
        gameObject.SetActive(false);
        // ��ȯ ��ġ ��ȯ
        if (spawner != null)
        {
            spawner.SetPositionAvailable(spawnPointIndex);
        }
    }

    public abstract void RotateTurret(); // �ͷ��� ���� ȸ���ϴ� �޼���
}
