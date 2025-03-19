using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    public interface IDeployable
    {
        public void OnButtonDown();
        public void OnButtonUp();
    }

    public class Tower : MonoBehaviour, IDeployable
    {
        public int range; // 공격 범위
        public float damage; // 타워 공격력 = 발사체 공격력
        public float attackSpeed; // 초당 공격 횟수
        public GameObject currentTarget; // 현재 타겟
        public float attackCooldown = 0f; // 공격 주기 타이머

        //public CircleCollider2D rangeCollider; // 공격 범위 콜라이더
        public GameObject projectilePrefab; // 발사체 프리팹

        void Awake()
        {
            currentTarget = null;
        }

        void Update()
        {
            // 타겟이 없으면 아무 것도 하지 않음
            if (currentTarget == null)
                return;

            // 공격 주기를 위해 타이머 업데이트
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                FireProjectile();
                attackCooldown = 1f / attackSpeed;
            }
        }

        private void FireProjectile()
        {
            // 발사체 생성 및 타겟 설정 로직 (예시)
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 발사체 스크립트에서 타겟을 설정할 수 있도록 예시 코드
            TowerProjectile projectileScript = projectile.GetComponent<TowerProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(currentTarget, damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("OnTriggerEnter2D");
            // 새로운 적이 타워의 공격 범위에 들어왔을 때
            if (collision.gameObject.tag == "Enemy" && currentTarget == null)
            {
                Debug.Log("New target acquired: " + collision.gameObject.name);
                currentTarget = collision.gameObject;              
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // 타겟인 적이 타워의 공격 범위를 벗어났을 때
            if (collision.gameObject == currentTarget)
            {
                Debug.Log("Target lost: " + collision.gameObject.name);
                currentTarget = null;
            }
        }

        public void OnButtonDown()
        {
            Debug.Log("OnButtonDown");
        }

        public void OnButtonUp()
        {
            Debug.Log("OnButtonUp");
        }
    }
}
