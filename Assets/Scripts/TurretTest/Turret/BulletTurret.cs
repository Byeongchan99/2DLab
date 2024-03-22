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
                Vector2 direction = targetPosition.position - rotatePoint.position;
                // atan2를 사용하여 라디안으로 방향 각도를 계산한 다음, 도(degree)로 변환
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Z축 회전
                Vector3 rotation = new Vector3(0, 0, angle);
                // 회전 애니메이션이 완료된 후 ShootProjectile 메서드 호출
                rotatePoint.DOLocalRotate(rotation, 0.5f).SetEase(Ease.OutSine).OnComplete(ShootProjectile);
            }
        }

        /// <summary> 발사체 생성 </summary>
        private void ShootProjectile()
        {
            // 플레이어의 위치를 향해 총알 생성
            Instantiate(currentProjectilePrefabs, firePoint.position, firePoint.rotation);
        }
    }
}
