using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveTarget : MonoBehaviour
{
    public float moveSpeed; // ������Ʈ�� �̵� �ӵ�
    private Vector3 moveDirection; // ������Ʈ�� �̵� ����
    public float changeDirectionTime; // ���� ���� �ð� ����
    private float timer; // Ÿ�̸�

    // ȭ�� ����
    private float minX = -8f;
    private float maxX = 8f;
    private float minY = -3f;
    private float maxY = 4f;

    void OnEnable()
    {
        AdjustScaleBasedOnZAxis();
        ChangeDirection();
    }

    void Update()
    {
        Move();
    }

    // ���� �ð����� ������ �����ϸ� �̵�
    void Move()
    {
        // ���� ��ġ ������Ʈ
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ȭ�� ������ ����� �ʵ��� ����
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            0);

        // ���� ���� Ÿ�̸�
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    // Z�࿡ ���� ������ ����
    void AdjustScaleBasedOnZAxis()
    {
        float z = transform.position.z;

        if (z >= 0 && z <= 30)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else if (z > 30 && z <= 50)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 1f);
        }
        else if (z > 50 && z <= 60)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        }
        else if (z > 60 && z <= 70)
        {
            transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        }
    }

    // �������� �����̴� ���� ����
    void ChangeDirection()
    {
        int direction = Random.Range(0, 4); // 0: ��, 1: �Ʒ�, 2: ����, 3: ������
        switch (direction)
        {
            case 0:
                moveDirection = Vector3.up;
                break;
            case 1:
                moveDirection = Vector3.down;
                break;
            case 2:
                moveDirection = Vector3.left;
                break;
            case 3:
                moveDirection = Vector3.right;
                break;
        }
    }
}
