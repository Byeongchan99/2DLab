using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RandomMoveTarget : MonoBehaviour
{
    public float moveSpeed; // 오브젝트의 이동 속도
    private Vector3 moveDirection; // 오브젝트의 이동 방향
    public float changeDirectionTime; // 방향 변경 시간 간격
    private float timer; // 타이머
    public int health; // 체력

    // 화면 범위
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

    // 일정 시간마다 방향을 변경하며 이동
    void Move()
    {
        // 현재 위치 업데이트
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

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

    // 랜덤으로 움직이는 방향 결정
    // 범위를 벗어난 경우에 대한 방향 변경 로직
    void ChangeDirection(bool forceChange)
    {
        if (forceChange)
        {
            // 벽에 닿았을 때의 방향 변경 로직
            if (transform.position.x <= minX)
            {
                moveDirection = Vector3.right; // 왼쪽 벽에 부딪힌 경우 오른쪽으로 이동
            }
            else if (transform.position.x >= maxX)
            {
                moveDirection = Vector3.left; // 오른쪽 벽에 부딪힌 경우 왼쪽으로 이동
            }
            if (transform.position.y <= minY)
            {
                moveDirection = Vector3.up; // 하단 벽에 부딪힌 경우 위로 이동
            }
            else if (transform.position.y >= maxY)
            {
                moveDirection = Vector3.down; // 상단 벽에 부딪힌 경우 아래로 이동
            }
            return;
        }

        // 정기적인 방향 변경 로직은 동일하게 유지
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
