using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveTarget : MonoBehaviour
{
    public float moveSpeed; // 오브젝트의 이동 속도
    private Vector3 moveDirection; // 오브젝트의 이동 방향
    public float changeDirectionTime; // 방향 변경 시간 간격
    private float timer; // 타이머

    // 화면 범위
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

    // 일정 시간마다 방향을 변경하며 이동
    void Move()
    {
        // 현재 위치 업데이트
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 화면 범위를 벗어나지 않도록 제한
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            0);

        // 방향 변경 타이머
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            ChangeDirection();
            timer = 0;
        }
    }

    // Z축에 따라 스케일 조정
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

    // 랜덤으로 움직이는 방향 결정
    void ChangeDirection()
    {
        int direction = Random.Range(0, 4); // 0: 위, 1: 아래, 2: 왼쪽, 3: 오른쪽
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
