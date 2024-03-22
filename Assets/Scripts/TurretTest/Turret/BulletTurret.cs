using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TurretTest
{
    public class BulletTurret : BaseTurret
    {
        //float rotationSpeed = 2f; // �ͷ��� ȸ���ϴ� �ӵ�
        [SerializeField] Transform rotatePoint; // ȸ����ų �ڽ� ������Ʈ

        /// <summary> �߻� </summary>
        protected override void Shoot()
        {
            RotateTurret();
        }

        /// <summary> �ͷ� ȸ�� </summary>
        protected override void RotateTurret()
        {
            if (targetPosition != null && rotatePoint != null)
            {
                // �÷��̾ ���� ���� ���� ���
                Vector2 direction = targetPosition.position - rotatePoint.position;
                // atan2�� ����Ͽ� �������� ���� ������ ����� ����, ��(degree)�� ��ȯ
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Z�� ȸ��
                Vector3 rotation = new Vector3(0, 0, angle);
                // ȸ�� �ִϸ��̼��� �Ϸ�� �� ShootProjectile �޼��� ȣ��
                rotatePoint.DOLocalRotate(rotation, 0.5f).SetEase(Ease.OutSine).OnComplete(ShootProjectile);
            }
        }

        /// <summary> �߻�ü ���� </summary>
        private void ShootProjectile()
        {
            // �÷��̾��� ��ġ�� ���� �Ѿ� ����
            Instantiate(currentProjectilePrefabs, firePoint.position, firePoint.rotation);
        }
    }
}
