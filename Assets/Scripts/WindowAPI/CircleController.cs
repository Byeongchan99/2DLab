using System.Collections;
using System.Collections.Generic;
using TurretTest;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 inputVec;
    private float moveSpeed = 200f;

    // 읽기 전용 프로퍼티
    public Vector2 InputVec
    {
        get { return inputVec; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력을 Update에서 처리
        // 나중에 유니티의 new Input system으로 변경 예정
        inputVec = Vector2.zero;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // 물리 기반 이동을 FixedUpdate에서 처리
        Move();
    }

    void Move()
    {
        // velocity를 이용한 이동
        rb.velocity = InputVec.normalized * moveSpeed * Time.fixedDeltaTime;
    }
}
