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
    /// <summary> 공격 속도 = 터렛 유지 시간 / 투사체 발사 개수 </summary>
    protected float attackSpeed;
    /// <summary> 마지막 발사 이후 경과 시간 </summary>
    protected float timeSinceLastShot = 0f;
    /// <summary> 소환 위치 인덱스 </summary>
    [SerializeField] protected int spawnPointIndex;
    /// <summary> 투사체가 발사되는 위치(투사체가 생성되는 위치) </summary>
    protected Transform firePoint;
    /// <summary> 목표 위치 </summary>
    [SerializeField] protected Transform targetPosition;

    /// <summary> 발사할 투사체 프리팹 리스트 </summary>
    [SerializeField] protected GameObject[] projectilePrefabs;
    /// <summary> 현재 발사할 투사체 프리팹 </summary>
    [SerializeField] protected GameObject currentProjectilePrefabs;

    /****************************************************************************
                                   public Fields
    ****************************************************************************/
    /// <summary> TurretSpawner 참조 </summary>
    public TurretSpawner spawner;
    /// <summary> 터렛 유지 시간 </summary>
    public float lifeTime;
    /// <summary> 투사체 발사 개수 </summary>
    public int projectileCount;

    /****************************************************************************
                                    Unity Callbacks
    ****************************************************************************/
    void Awake()
    {
        InitTurret(); // 초기화
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
    /// <summary> 투사체를 발사 가능한지 확인 </summary>
    protected bool ShouldShoot()
    {
        return timeSinceLastShot >= attackSpeed;
    }

    /// <summary> 터렛 초기화 </summary>
    protected virtual void InitTurret()
    {
        attackSpeed = lifeTime / projectileCount;
        currentProjectilePrefabs = projectilePrefabs[0];
        targetPosition = PlayerStat.Instance.transform;
    }

    /// <summary> 투사체 발사 </summary>
    protected abstract void Shoot();

    /// <summary> 터렛 비활성화 </summary>
    // 나중에 오브젝트 풀링으로 변경
    protected void DisableTurret()
    {
        gameObject.SetActive(false);
        // 소환 위치 반환
        if (spawner != null)
        {
            spawner.SetPositionAvailable(spawnPointIndex);
        }
    }

    /****************************************************************************
                                 public Methods
    ****************************************************************************/
    /// <summary> 발사할 투사체 변경 </summary>
    public virtual void ChangeProjectile(int projectileIndex)
    {
        Debug.Log("ChangeProjectile");
        currentProjectilePrefabs = projectilePrefabs[projectileIndex];
    }

    /// <summary> 터렛의 포를 회전 </summary>
    public abstract void RotateTurret();
}
