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

        public Vector3 spawnPoint;
        public Vector3 endPoint;

        void StartRound()
        {
            pathManager.InitializePath(spawnPoint, endPoint);
            SpawnMonster();
        }

        void SpawnMonster()
        {
            GameObject monsterObj = Instantiate(monsterPrefab);
            Enemy monster = monsterObj.GetComponent<Enemy>();
            monster.SetPath(pathManager.pathPositions);
        }
    }
}
