using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class BaseProjectile : MonoBehaviour
    {
        public float speed = 5.0f;
        private Rigidbody2D rb;

        /// <summary> �ʱ�ȭ </summary>
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            Move();
        }

        protected void Update()
        {
            CheckOutOfBounds();
        }

        /// <summary> �ӵ� ���� </summary>
        public void ChangeSpeed(float newSpeed)
        {
            speed = newSpeed;
            Move();
        }

        /// <summary> �̵� </summary>
        protected virtual void Move()
        {
            rb.velocity = transform.right * speed;
        }

        /// <summary> �̺�Ʈ ������ �߰� </summary>
        protected void CheckOutOfBounds()
        {
            // �������κ��� 20 �̻� �������� ����
            if (transform.position.magnitude > 20.0f)
            {
                DestroyProjectile();
            }
        }

        /// <summary> �߻�ü �ı� </summary>
        protected void DestroyProjectile()
        {
            // ���߿� ������Ʈ Ǯ������ ����
            Destroy(gameObject);
        }

        /// <summary> �浹 �˻� </summary>
        void OnTriggerEnter2D(Collider2D collision)
        {
            // �÷��̾�� �浹���� ��
            if (collision.CompareTag("Player"))
            {                
                DestroyProjectile();
            }
            // ���߿� EMP�� �浹���� �� �߰�
        }
    }
}
