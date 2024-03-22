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

        /// <summary> 초기화 </summary>
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

        /// <summary> 속도 변경 </summary>
        public void ChangeSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        /// <summary> 이동 </summary>
        protected virtual void Move()
        {
            rb.velocity = moveDirection * speed;
        }

        /// <summary> 이벤트 리스너 추가 </summary>
        protected void CheckOutOfBounds()
        {
            // 원점으로부터 20 이상 떨어지면 삭제
            if (transform.position.magnitude > 30.0f)
            {
                DestroyProjectile();
            }
        }

        /// <summary> 발사체 파괴 </summary>
        protected void DestroyProjectile()
        {
            // 나중에 오브젝트 풀링으로 변경
            Destroy(gameObject);
        }

        /// <summary> 충돌 검사 </summary>
        void OnTriggerEnter2D(Collider2D collision)
        {
            // 플레이어와 충돌했을 때
            if (collision.CompareTag("Player"))
            {                
                DestroyProjectile();
            }
            // 나중에 EMP와 충돌했을 때 추가
        }
    }
}
