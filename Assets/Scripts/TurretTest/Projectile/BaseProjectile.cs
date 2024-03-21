using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    protected void Update()
    {
        CheckOutOfBounds();
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
        Move();
    }

    protected virtual void Move()
    {
        rb.velocity = transform.right * speed;
    }

    protected void CheckOutOfBounds()
    {
        if (transform.position.magnitude > 20.0f) // 예시로 20을 사용
        {
            DestroyProjectile();
        }
    }

    protected void DestroyProjectile()
    {
        // 추가적인 비활성화 로직이 필요하면 여기에 작성
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어와 충돌했을 때의 로직
            DestroyProjectile();
        }
    }
}
