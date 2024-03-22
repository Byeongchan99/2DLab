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

            // ���� �Ѿ��� ������ �������� �п�
            Quaternion currentRotation = transform.rotation;
            Instantiate(projectilePrefab, transform.position, currentRotation * Quaternion.Euler(0, 0, 45));
            Instantiate(projectilePrefab, transform.position, currentRotation);
            Instantiate(projectilePrefab, transform.position, currentRotation * Quaternion.Euler(0, 0, -45));

            DestroyProjectile();
        }
    }
}
