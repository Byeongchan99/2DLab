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
        if (transform.position.magnitude > 20.0f) // ���÷� 20�� ���
        {
            DestroyProjectile();
        }
    }

    protected void DestroyProjectile()
    {
        // �߰����� ��Ȱ��ȭ ������ �ʿ��ϸ� ���⿡ �ۼ�
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾�� �浹���� ���� ����
            DestroyProjectile();
        }
    }
}
