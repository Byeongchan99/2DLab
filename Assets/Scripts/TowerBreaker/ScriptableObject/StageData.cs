using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public int level; // 레벨 - 층 수
    public NormalEnemyType[] normalEnemyTypes; // 등장할 수 있는 일반 적 타입
    public int normalEnemyCount; // 일반 적 개수
    public NormalEnemyData normalEnemyData; // 등장하는 일반 적 데이터
    public EliteEnemyType[] eliteEnemyTypes; // 등장할 수 있는 엘리트 적 타입
    public int eliteEnemyCount; // 엘리트 적 개수
    public EliteEnemyData eliteEnemyData; // 등장하는 엘리트 적 데이터
    public int interruptCount; // 방해 마법 개수
}
