using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class ProjectileDecorator : Bullet
    {
        [SerializeField] float splitTime = 1f;
        [SerializeField] GameObject projectilePrefab;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(SplitProjectile());
        }

        IEnumerator SplitProjectile()
        {
            yield return new WaitForSeconds(splitTime);

            // 현재 총알의 방향을 기준으로 분열
            Quaternion currentRotation = transform.rotation;
            Instantiate(projectilePrefab, transform.position, currentRotation * Quaternion.Euler(0, 0, 45));
            Instantiate(projectilePrefab, transform.position, currentRotation);
            Instantiate(projectilePrefab, transform.position, currentRotation * Quaternion.Euler(0, 0, -45));

            DestroyProjectile();
        }
    }
}
