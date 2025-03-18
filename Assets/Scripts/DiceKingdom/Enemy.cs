using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceKingdom
{
    public class Enemy : MonoBehaviour
    {
        public float speed = 3f;

        List<Vector3> path;
        int currentPathIndex;

        // 몬스터가 생성될 때 경로 설정
        public void SetPath(List<Vector3> pathPositions)
        {
            path = pathPositions;
            currentPathIndex = 0;
            transform.position = path[currentPathIndex]; // SpawnPoint에 즉시 배치
            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            while (currentPathIndex < path.Count)
            {
                Vector3 target = path[currentPathIndex];
                while (Vector3.Distance(transform.position, target) > 0.05f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    yield return null;
                }
                currentPathIndex++;
            }

            // 경로 끝에 도달했을 때 로직 (EndPoint 도착)
            OnReachedEndPoint();
        }

        void OnReachedEndPoint()
        {
            // 몬스터가 경로 끝에 도착했을 때 처리 (데미지 등)
            Debug.Log("몬스터가 목적지에 도착했습니다!");
            Destroy(gameObject);
        }

        public void TakeDamage(float amount)
        {
            Debug.Log("Taking damage: " + amount);
        }

        public void ApplySlow(float amount)
        {
            Debug.Log("Applying slow to enemy: " + amount);
        }
    }
}
