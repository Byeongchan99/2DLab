using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public int level; // ���� - �� ��
    public NormalEnemyType[] normalEnemyTypes; // ������ �� �ִ� �Ϲ� �� Ÿ��
    public int normalEnemyCount; // �Ϲ� �� ����
    public NormalEnemyData normalEnemyData; // �����ϴ� �Ϲ� �� ������
    public EliteEnemyType[] eliteEnemyTypes; // ������ �� �ִ� ����Ʈ �� Ÿ��
    public int eliteEnemyCount; // ����Ʈ �� ����
    public EliteEnemyData eliteEnemyData; // �����ϴ� ����Ʈ �� ������
    public int interruptCount; // ���� ���� ����
}
