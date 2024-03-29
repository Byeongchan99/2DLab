using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class ProjectileManager : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public GameObject splitBulletPrefab;
        // 다른 발사체 프리팹도 추가

        public Transform projectileContainer;

        private void Start()
        {
            ProjectilePoolManager.Instance.CreatePool(bulletPrefab.GetComponent<Bullet>(), 20, projectileContainer);
            ProjectilePoolManager.Instance.CreatePool(splitBulletPrefab.GetComponent<SplitBullet>(), 20, projectileContainer);
            // 다른 발사체 풀도 생성
        }
    }
}
