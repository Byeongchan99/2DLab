using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    public class BaseProjectile : MonoBehaviour
    {
        protected Rigidbody2D rb;

        public float speed = 5.0f;
        public Vector2 moveDirection;

        /// <summary> �ʱ�ȭ </summary>
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
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
        }

        /// <summary> �̵� </summary>
        protected virtual void Move()
        {
            rb.velocity = moveDirection * speed;
        }

        /// <summary> �̺�Ʈ ������ �߰� </summary>
        protected void CheckOutOfBounds()
        {
            // �������κ��� 20 �̻� �������� ����
            if (transform.position.magnitude > 30.0f)
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
