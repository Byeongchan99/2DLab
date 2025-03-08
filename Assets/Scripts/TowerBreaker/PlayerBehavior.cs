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
    /// ���� �ٷ� �ձ��� �̵��Ѵ�.
    /// </summary>
    public void Dash()
    {
        if (_startDash)
        {
            StartCoroutine(DashCoroutine());
        }
        else
        {
            Debug.Log("�̹� �뽬 ���Դϴ�.");
        }
    }

    IEnumerator DashCoroutine()
    {
        _startDash = true;
        // �̵� ����
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ���� �浹�Ѵٸ� �뽬 ���� ����
        if (collision.CompareTag("Enemy"))
        {
            StopCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// ��� ������ �÷��̾��� ���� ��ġ�� ���� �޶�����.
    /// �÷��̾� �� �߾� ���� ���ʿ� ���� ���� �з����� ���, �����ʿ� ���� ���� ���Һ��� �Ѵ�.
    /// </summary>
    public void Defense()
    {
        // �켱 ž �߾��� ��ġ�� 0�̶�� ����
        if (transform.position.x < 0) // ���ʿ� ���� ��
        {
            StartCoroutine(PushBack());
        }
        else // �����ʿ� ���� ��
        {
            StartCoroutine(BackTumbling());
        }
    }

    IEnumerator PushBack()
    {
        Debug.Log("�з����� ���");
        yield break;
    }

    IEnumerator BackTumbling()
    {
        Debug.Log("���Һ�");
        yield break;
    }

    /// <summary>
    /// ���� ������ ������ ������ ���� �޶�����.
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

    // �Ϲ� ����
    IEnumerator NormalAttack()
    {
        Debug.Log("�Ϲ� ����");
        yield break;
    }

    // ���� ����
    IEnumerator PiercingAttack()
    {
        Debug.Log("���� ����");
        yield break;
    }

    // ���� ����
    IEnumerator DoubleAttack()
    {
        Debug.Log("���� ����");
        yield break;
    }
}
