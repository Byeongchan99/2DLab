using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    PlayerStat _playerStat;
    bool _startDash;

    private void Awake()
    {
        if (_playerStat == null)
        {
            _playerStat = GetComponent<PlayerStat>();
        }
    }

    /// <summary>
    /// 적의 바로 앞까지 이동한다.
    /// </summary>
    public void Dash()
    {
        if (_startDash)
        {
            StartCoroutine(DashCoroutine());
        }
        else
        {
            Debug.Log("이미 대쉬 중입니다.");
        }
    }

    IEnumerator DashCoroutine()
    {
        _startDash = true;
        // 이동 로직
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 적과 충돌한다면 대쉬 로직 종료
        if (collision.CompareTag("Enemy"))
        {
            StopCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// 방어 로직은 플레이어의 현재 위치에 따라 달라진다.
    /// 플레이어 맵 중앙 기준 왼쪽에 있을 때는 밀려나는 모션, 오른쪽에 있을 때는 백텀블링을 한다.
    /// </summary>
    public void Defense()
    {
        // 우선 탑 중앙의 위치를 0이라고 가정
        if (transform.position.x < 0) // 왼쪽에 있을 때
        {
            StartCoroutine(PushBack());
        }
        else // 오른쪽에 있을 때
        {
            StartCoroutine(BackTumbling());
        }
    }

    IEnumerator PushBack()
    {
        Debug.Log("밀려나는 모션");
        yield break;
    }

    IEnumerator BackTumbling()
    {
        Debug.Log("백텀블링");
        yield break;
    }

    /// <summary>
    /// 공격 로직은 무기의 종류에 따라 달라진다.
    /// </summary>
    public void Attack()
    {
        switch(_playerStat.CurrentWeapon.attackMethod)
        {
            case AttackMethod.Normal:
                StartCoroutine(NormalAttack());
                break;
            case AttackMethod.Piercing:
                StartCoroutine(PiercingAttack());
                break;
            case AttackMethod.Double:
                StartCoroutine(DoubleAttack());
                break;
        }
    }

    // 일반 공격
    IEnumerator NormalAttack()
    {
        Debug.Log("일반 공격");
        yield break;
    }

    // 관통 공격
    IEnumerator PiercingAttack()
    {
        Debug.Log("관통 공격");
        yield break;
    }

    // 더블 공격
    IEnumerator DoubleAttack()
    {
        Debug.Log("더블 공격");
        yield break;
    }
}
