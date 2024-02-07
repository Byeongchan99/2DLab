using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RandomMoveTarget : MonoBehaviour
{
    public float moveSpeed; // ������Ʈ�� �̵� �ӵ�
    private Vector3 moveDirection; // ������Ʈ�� �̵� ����
    public float changeDirectionTime; // ���� ���� �ð� ����
    private float timer; // Ÿ�̸�
    public int health; // ü��

    // ȭ�� ����
    [SerializeField]private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 4f;

    void OnEnable()
    {
        health = 3;
        ChangeDirection(false);
    }

    void Update()
    {
        Move();

        if (health <= 0)
        {
            ObjectPoolManager.Instance.ReturnTargetToPool(gameObject);
        }
    }

    // ���� �ð����� ������ �����ϸ� �̵�
    void Move()
    {
        // ���� ��ġ ������Ʈ
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

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
    public void AdjustMovementRangeBasedOnZ(float depth)
    {
        if (depth > 0 && depth < 30)
        {
            minX = -8f;
            maxX = 8f;
            minY = -3f;
            maxY = 4f;
        }
        else if (depth > 30 && depth < 50)
        {
            minX = -8f;
            maxX = 8f;
            minY = -2.8f;
            maxY = 4f;
        }
        else if (depth > 50 && depth < 60)
        {
            minX = -8f;
            maxX = 8f;
            minY = -2.5f;
            maxY = 4f;
        }
        else if (depth > 60 && depth < 70)
        {
            minX = -8f;
            maxX = 8f;
            minY = -2f;
            maxY = 4f;
        }
    }

    // �������� �����̴� ���� ����
    // ������ ��� ��쿡 ���� ���� ���� ����
    void ChangeDirection(bool forceChange)
    {
        if (forceChange)
        {
            // ���� ����� ���� ���� ���� ����
            if (transform.position.x <= minX)
            {
                moveDirection = Vector3.right; // ���� ���� �ε��� ��� ���������� �̵�
            }
            else if (transform.position.x >= maxX)
            {
                moveDirection = Vector3.left; // ������ ���� �ε��� ��� �������� �̵�
            }
            if (transform.position.y <= minY)
            {
                moveDirection = Vector3.up; // �ϴ� ���� �ε��� ��� ���� �̵�
            }
            else if (transform.position.y >= maxY)
            {
                moveDirection = Vector3.down; // ��� ���� �ε��� ��� �Ʒ��� �̵�
            }
            return;
        }

        // �������� ���� ���� ������ �����ϰ� ����
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
