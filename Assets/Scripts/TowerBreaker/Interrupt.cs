using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupt : MonoBehaviour
{
    float _time;
    float _speed;

    private void OnEnable()
    {
        // Invoke�� time �Ŀ� Move ���� / �ڷ�ƾ�� ����
        decimal _time = (decimal)Random.Range(1, 3);
        Invoke("Move", (float)_time);
    }

    void Move()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ ���� ������ ����� �� ��� ������ ����
        if (collision.CompareTag("Player"))
        {          
            collision.GetComponent<PlayerBehavior>().Defense();
        }

        // ���� ����� �� ���� ������ ����
        if (collision.CompareTag("Wall"))
        {
            this.gameObject.SetActive(false); // ���߿� ������Ʈ Ǯ������ ����
        }
    }
}
