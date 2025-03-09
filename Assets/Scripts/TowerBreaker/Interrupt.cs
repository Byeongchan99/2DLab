using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interrupt : MonoBehaviour
{
    float _time;
    float _speed;

    private void OnEnable()
    {
        // Invoke로 time 후에 Move 실행 / 코루틴도 가능
        decimal _time = (decimal)Random.Range(1, 3);
        Invoke("Move", (float)_time);
    }

    void Move()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어가 방해 마법에 닿았을 때 방어 로직을 실행
        if (collision.CompareTag("Player"))
        {          
            collision.GetComponent<PlayerBehavior>().Defense();
        }

        // 벽에 닿았을 때 방해 마법을 제거
        if (collision.CompareTag("Wall"))
        {
            this.gameObject.SetActive(false); // 나중에 오브젝트 풀링으로 변경
        }
    }
}
