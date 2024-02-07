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
        ChangeDirection(false);
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

        float currentZ = transform.position.z;

        // Ȱ�� ������ ����� ���� ��� ����
        if (transform.position.x <= minX || transform.position.x >= maxX ||
            transform.position.y <= minY || transform.position.y >= maxY)
        {
            ChangeDirection(true); // ���� ����� �� ���� ����
        }

        // ���� ���� Ÿ�̸�
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            ChangeDirection(false);
            timer = 0;
        }
    }

    // Z�࿡ ���� Ȱ�� ���� ����
    void AdjustMovementRangeBasedOnZ(int depth)
    {
        switch (depth)
        {
            case 1:
                minX = -8f;
                maxX = 8f;
                minY = -3f;
                maxY = 4f;
                break;
            case 2:
                minX = -8f;
                maxX = 8f;
                minY = -2.8f;
                maxY = 4f;
                break;
            case 3:
                minX = -8f;
                maxX = 8f;
                minY = -2.5f;
                maxY = 4f;
                break;
            case 4:
                minX = -8f;
                maxX = 8f;
                minY = -2f;
                maxY = 4f;
                break;
        }
    }

    // �������� �����̴� ���� ����
    void ChangeDirection(bool changeDirection)
    {
        if (changeDirection)
        {
            moveDirection = -moveDirection; // �ݴ� �������� ����
            return;
        }

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
