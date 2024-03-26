using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class SplitBullet : Bullet
    {
        [SerializeField] float splitTime = 1f;
        [SerializeField] string bulletPoolName;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Split());
        }

        IEnumerator Split()
        {
            yield return new WaitForSeconds(splitTime);

            // 현재 총알의 방향을 기준으로 분열
            Quaternion currentRotation = transform.rotation;

            // 오브젝트 풀에서 분열 총알 가져오기
            BaseProjectile bullet1 = ProjectilePoolManager.Instance.Get(bulletPoolName);
            BaseProjectile bullet2 = ProjectilePoolManager.Instance.Get(bulletPoolName);
            BaseProjectile bullet3 = ProjectilePoolManager.Instance.Get(bulletPoolName);

            if (bullet1 != null && bullet2 != null && bullet3 != null)
            {
                bullet1.transform.position = transform.position;
                bullet1.transform.rotation = currentRotation * Quaternion.Euler(0, 0, 45);
                bullet1.gameObject.SetActive(true);

                bullet2.transform.position = transform.position;
                bullet2.transform.rotation = currentRotation;
                bullet2.gameObject.SetActive(true);

                bullet3.transform.position = transform.position;
                bullet3.transform.rotation = currentRotation * Quaternion.Euler(0, 0, -45);
                bullet3.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Failed to get split bullets from pool.");
            }

            DestroyProjectile();
        }
    }
}
