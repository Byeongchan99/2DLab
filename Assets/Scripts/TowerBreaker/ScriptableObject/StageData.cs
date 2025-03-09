using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public int level; // 레벨 - 층 수
    public int normalEnemyCount; // 일반 적 개수
    public float normalEnemyHealth; // 일반 적 체력
    public float normalEnemySpeed; // 일반 적 속도
    public int eliteEnemyCount; // 엘리트 적 개수
    public float eliteEnemyHealth; // 엘리트 적 체력
    public float eliteEnemySpeed; // 엘리트 적 속도
    public int interruptCount; // 방해 마법 개수
}
