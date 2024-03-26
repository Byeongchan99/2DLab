using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TurretTest
{
    public class BulletTurret : BaseTurret
    {
        //float rotationSpeed = 2f; // 터렛이 회전하는 속도
        [SerializeField] Transform rotatePoint; // 회전시킬 자식 오브젝트
        [SerializeField] Vector2 direction;
        float angle;

        /// <summary> 발사 </summary>
        protected override void Shoot()
        {
            RotateTurret();
        }

        /// <summary> 터렛 회전 </summary>
        protected override void RotateTurret()
        {
            if (targetPosition != null && rotatePoint != null)
            {
                // 플레이어를 향한 방향 벡터 계산
                direction = targetPosition.position - rotatePoint.position;
                // atan2를 사용하여 라디안으로 방향 각도를 계산한 다음, 도(degree)로 변환
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // 스프라이트가 뒤집혀 있으면 각도 조정
                if (transform.localScale.x < 0)
                {
                    angle = 180f - angle; // 뒤집힌 스프라이트에 대해 각도를 조정
                }
                // Z축 회전
                Vector3 rotation = new Vector3(0, 0, angle);
                // 회전 애니메이션이 완료된 후 ShootProjectile 메서드 호출
                rotatePoint.DOLocalRotate(rotation, 0.5f).SetEase(Ease.OutSine).OnComplete(ShootProjectile);
            }
        }

        /// <summary> 발사체 생성 </summary>
        private void ShootProjectile()
        {
            if (currentProjectilePrefabs == null || firePoint == null)
            {
                Debug.LogError("Projectile prefab or fire point is not set.");
                return;
            }

            // direction 벡터를 반시계 방향으로 90도 회전
            //Vector2 shootingDirection = new Vector2(-direction.y, direction.x);
            // direction 벡터를 바탕으로 Quaternion 생성
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            // 오브젝트 풀에서 총알 가져오기
            BaseProjectile projectile = ProjectilePoolManager.Instance.Get(currentProjectilePrefabs.name);

            if (projectile != null)
            {
                // 총알 위치와 회전 설정
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = rotation;
                projectile.SetDirection(direction);
                projectile.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Failed to get projectile from pool.");
            }



            /*
            BaseProjectile projectile = projectileObject.GetComponent<BaseProjectile>();

            if (projectile != null)
            {
                // firePoint에서 targetPosition으로 향하는 방향 벡터 계산
                Vector2 direction = (targetPosition.position - firePoint.position).normalized;
                // 투사체 이동 방향 설정
                projectile.moveDirection = direction;
            }
            else
            {
                Debug.LogError("Spawned projectile does not have a BaseProjectile component.");
            }
            */
        }

    }
}
