using System.Collections;
using System.Collections.Generic;
using TurretTest;
using UnityEngine;

public abstract class BaseTurret : MonoBehaviour
{
    public TurretSpawner spawner; // TurretSpawner 참조

    public float lifeTime; // 터렛 유지 시간
    public int projectileCount; // 투사체 발사 개수
    public int spawnPointIndex; // 소환 위치
    public Transform firePoint; // 발사 위치
    public Transform targetPosition; // 목표 위치

    protected float attackSpeed; // 공격 속도
    protected float timeSinceLastShot = 0f; // 마지막 발사 이후 경과 시간

    public GameObject[] projectilePrefabs; // 발사할 투사체 프리팹 리스트
    protected GameObject currentProjectilePrefabs; // 현재 발사할 투사체 프리팹

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

    // 초기화
    protected virtual void InitTurret()
    {
        attackSpeed = lifeTime / projectileCount;
        currentProjectilePrefabs = projectilePrefabs[1];
        targetPosition = PlayerStat.Instance.transform;
    }

    protected abstract void Shoot();

    protected void DisableTurret()
    {
        // 터렛 비활성화
        gameObject.SetActive(false);
        // 소환 위치 반환
        if (spawner != null)
        {
            spawner.SetPositionAvailable(spawnPointIndex);
        }
    }

    public abstract void RotateTurret(); // 터렛의 포를 회전하는 메서드
}
