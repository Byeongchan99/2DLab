using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    void Awake()
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
        currentProjectilePrefabs = projectilePrefabs[0];
        targetPosition = PlayerStat.Instance.transform;
    }

    protected abstract void Shoot();

    // �ͷ� ��Ȱ��ȭ
    protected void DisableTurret()
    {
        gameObject.SetActive(false);
        // ��ȯ ��ġ ��ȯ
        if (spawner != null)
        {
            spawner.SetPositionAvailable(spawnPointIndex);
        }
    }

    // �߻��� ����ü�� ��ü
    public virtual void ChangeProjectile(int projectileIndex)
    {
        Debug.Log("ChangeProjectile");
        currentProjectilePrefabs = projectilePrefabs[projectileIndex];
    }

    public abstract void RotateTurret(); // �ͷ��� ���� ȸ���ϴ� �޼���
}
