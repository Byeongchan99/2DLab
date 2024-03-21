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
            public int spawnLevel; // 소환 레벨
            public float spawnCooldownPercent; // 소환 쿨타임 퍼센트
        }

        public List<TurretData> turretDatas; // 상황에 따른 터렛 종류별 데이터 리스트
    }
}
