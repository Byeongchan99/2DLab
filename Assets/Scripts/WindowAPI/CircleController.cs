using System.Collections;
using System.Collections.Generic;
using TurretTest;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public Rigidbody2D rb;

    private Vector2 inputVec;
    private float moveSpeed = 200f;

    // �б� ���� ������Ƽ
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
        // �Է��� Update���� ó��
        // ���߿� ����Ƽ�� new Input system���� ���� ����
        inputVec = Vector2.zero;
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // ���� ��� �̵��� FixedUpdate���� ó��
        Move();
    }

    void Move()
    {
        // velocity�� �̿��� �̵�
        rb.velocity = InputVec.normalized * moveSpeed * Time.fixedDeltaTime;
    }
}
