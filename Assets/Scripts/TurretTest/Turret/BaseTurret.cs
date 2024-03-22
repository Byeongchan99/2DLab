using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TurretTest;
using UnityEngine;

public abstract class BaseTurret : MonoBehaviour
{
    /****************************************************************************
                                 protected Fields
    ****************************************************************************/
    /// <summary> ���� �ӵ� = �ͷ� ���� �ð� / ����ü �߻� ���� </summary>
    protected float attackSpeed;
    /// <summary> ������ �߻� ���� ��� �ð� </summary>
    protected float timeSinceLastShot = 0f;
    /// <summary> ��ȯ ��ġ �ε��� </summary>
    [SerializeField] protected int spawnPointIndex;
    /// <summary> ����ü�� �߻�Ǵ� ��ġ(����ü�� �����Ǵ� ��ġ) </summary>
    protected Transform firePoint;
    /// <summary> ��ǥ ��ġ </summary>
    [SerializeField] protected Transform targetPosition;

    /// <summary> �߻��� ����ü ������ ����Ʈ </summary>
    [SerializeField] protected GameObject[] projectilePrefabs;
    /// <summary> ���� �߻��� ����ü ������ </summary>
    [SerializeField] protected GameObject currentProjectilePrefabs;

    /****************************************************************************
                                   public Fields
    ****************************************************************************/
    /// <summary> TurretSpawner ���� </summary>
    public TurretSpawner spawner;
    /// <summary> �ͷ� ���� �ð� </summary>
    public float lifeTime;
    /// <summary> ����ü �߻� ���� </summary>
    public int projectileCount;

    /****************************************************************************
                                    Unity Callbacks
    ****************************************************************************/
    void Awake()
    {
        InitTurret(); // �ʱ�ȭ
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

    /****************************************************************************
                                 private Methods
    ****************************************************************************/
    /// <summary> ����ü�� �߻� �������� Ȯ�� </summary>
    protected bool ShouldShoot()
    {
        return timeSinceLastShot >= attackSpeed;
    }

    /// <summary> �ͷ� �ʱ�ȭ </summary>
    protected virtual void InitTurret()
    {
        attackSpeed = lifeTime / projectileCount;
        currentProjectilePrefabs = projectilePrefabs[0];
        targetPosition = PlayerStat.Instance.transform;
    }

    /// <summary> ����ü �߻� </summary>
    protected abstract void Shoot();

    /// <summary> �ͷ� ��Ȱ��ȭ </summary>
    // ���߿� ������Ʈ Ǯ������ ����
    protected void DisableTurret()
    {
        gameObject.SetActive(false);
        // ��ȯ ��ġ ��ȯ
        if (spawner != null)
        {
            spawner.SetPositionAvailable(spawnPointIndex);
        }
    }

    /****************************************************************************
                                 public Methods
    ****************************************************************************/
    /// <summary> �߻��� ����ü ���� </summary>
    public virtual void ChangeProjectile(int projectileIndex)
    {
        Debug.Log("ChangeProjectile");
        currentProjectilePrefabs = projectilePrefabs[projectileIndex];
    }

    /// <summary> �ͷ��� ���� ȸ�� </summary>
    public abstract void RotateTurret();
}
