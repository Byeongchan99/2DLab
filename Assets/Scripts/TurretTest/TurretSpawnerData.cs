using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurretTest
{
    [CreateAssetMenu(fileName = "TurretSpawnerData", menuName = "ScriptableObjects/TurretSpawnerData", order = 1)]
    public class TurretSpawnerData : ScriptableObject
    {
        [System.Serializable]
        public struct TurretData
        {
            public int spawnLevel; // ��ȯ ����
            public float spawnCooldownPercent; // ��ȯ ��Ÿ�� �ۼ�Ʈ
        }

        public List<TurretData> turretDatas; // ��Ȳ�� ���� �ͷ� ������ ������ ����Ʈ
    }
}
