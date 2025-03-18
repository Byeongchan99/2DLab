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

        // ���Ͱ� ������ �� ��� ����
        public void SetPath(List<Vector3> pathPositions)
        {
            path = pathPositions;
            currentPathIndex = 0;
            transform.position = path[currentPathIndex]; // SpawnPoint�� ��� ��ġ
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

            // ��� ���� �������� �� ���� (EndPoint ����)
            OnReachedEndPoint();
        }

        void OnReachedEndPoint()
        {
            // ���Ͱ� ��� ���� �������� �� ó�� (������ ��)
            Debug.Log("���Ͱ� �������� �����߽��ϴ�!");
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
