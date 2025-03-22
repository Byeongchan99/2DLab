using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private Tower parentTower;

    void Awake()
    {
        // 부모에 있는 Tower 컴포넌트를 찾습니다.
        parentTower = GetComponentInParent<Tower>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (parentTower != null && other.gameObject.tag == "Enemy")
        {
            parentTower.OnRangeTriggerEnter(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (parentTower != null)
        {
            parentTower.OnRangeTriggerExit(other);
        }
    }
}
