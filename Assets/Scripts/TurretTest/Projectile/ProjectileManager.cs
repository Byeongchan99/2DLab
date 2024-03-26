using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class ProjectileManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public GameObject splitBulletPrefab;
        // �ٸ� �߻�ü �����յ� �߰�

        public Transform projectileContainer;

        private void Start()
        {
            ProjectilePoolManager.Instance.CreatePool(bulletPrefab.GetComponent<Bullet>(), 20, projectileContainer);
            ProjectilePoolManager.Instance.CreatePool(splitBulletPrefab.GetComponent<SplitBullet>(), 20, projectileContainer);
            // �ٸ� �߻�ü Ǯ�� ����
        }

        public void ShootBullet(Vector2 position, Quaternion rotation)
        {
            BaseProjectile bullet = ProjectilePoolManager.Instance.Get("Bullet");
            if (bullet != null)
            {
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                // �߰� ����...
            }
        }

        public void ShootSplitBullet(Vector2 position, Quaternion rotation)
        {
            BaseProjectile splitBullet = ProjectilePoolManager.Instance.Get("SplitBullet");
            if (splitBullet != null)
            {
                splitBullet.transform.position = position;
                splitBullet.transform.rotation = rotation;
                // �߰� ����...
            }
        }
    }
}
