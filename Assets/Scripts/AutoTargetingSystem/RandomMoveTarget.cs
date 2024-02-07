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
        ChangeDirection(false);
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

        float currentZ = transform.position.z;

        // 활동 범위를 벗어나면 방향 즉시 변경
        if (transform.position.x <= minX || transform.position.x >= maxX ||
            transform.position.y <= minY || transform.position.y >= maxY)
        {
            ChangeDirection(true); // 벽에 닿았을 때 방향 변경
        }

        // 방향 변경 타이머
        timer += Time.deltaTime;
        if (timer > changeDirectionTime)
        {
            ChangeDirection(false);
            timer = 0;
        }
    }

    // Z축에 따른 활동 범위 조절
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

    // 랜덤으로 움직이는 방향 결정
    void ChangeDirection(bool changeDirection)
    {
        if (changeDirection)
        {
            moveDirection = -moveDirection; // 반대 방향으로 변경
            return;
        }

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
