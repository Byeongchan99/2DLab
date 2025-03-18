using DiceKingdom;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DiceKingdom
{
    public class StageManager : MonoBehaviour
    {
        public PathManager pathManager;
        public GameObject monsterPrefab;

        public Transform spawnPoint;
        public Transform endPoint;

        private void Awake()
        {
            StartRound();
        }

        void StartRound()
        {
            pathManager.InitializePath(spawnPoint.position, endPoint.position);
            SpawnMonster();
        }

        public void SpawnMonster()
        {
            GameObject monsterObj = Instantiate(monsterPrefab);
            Enemy monster = monsterObj.GetComponent<Enemy>();
            monster.SetPath(pathManager.pathPositions);
        }
    }
}
