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
        public int range; // ���� ����
        public float damage; // Ÿ�� ���ݷ� = �߻�ü ���ݷ�
        public float attackSpeed; // �ʴ� ���� Ƚ��
        public GameObject currentTarget; // ���� Ÿ��
        public float attackCooldown = 0f; // ���� �ֱ� Ÿ�̸�

        //public CircleCollider2D rangeCollider; // ���� ���� �ݶ��̴�
        public GameObject projectilePrefab; // �߻�ü ������

        void Awake()
        {
            currentTarget = null;
        }

        void Update()
        {
            // Ÿ���� ������ �ƹ� �͵� ���� ����
            if (currentTarget == null)
                return;

            // ���� �ֱ⸦ ���� Ÿ�̸� ������Ʈ
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                FireProjectile();
                attackCooldown = 1f / attackSpeed;
            }
        }

        private void FireProjectile()
        {
            // �߻�ü ���� �� Ÿ�� ���� ���� (����)
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // �߻�ü ��ũ��Ʈ���� Ÿ���� ������ �� �ֵ��� ���� �ڵ�
            TowerProjectile projectileScript = projectile.GetComponent<TowerProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(currentTarget, damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("OnTriggerEnter2D");
            // ���ο� ���� Ÿ���� ���� ������ ������ ��
            if (collision.gameObject.tag == "Enemy" && currentTarget == null)
            {
                Debug.Log("New target acquired: " + collision.gameObject.name);
                currentTarget = collision.gameObject;              
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // Ÿ���� ���� Ÿ���� ���� ������ ����� ��
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
