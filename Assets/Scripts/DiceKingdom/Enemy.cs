using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DiceKingdom
{
    public class Enemy : MonoBehaviour
    {
        public float baseSpeed = 3f;
        public float currentSpeed;
        public float health = 100f;

        public Tilemap pathTilemap;

        List<Vector3> path;
        private Vector3Int currentCell;
        int currentPathIndex;

        private List<TileEffect> activeEffects = new List<TileEffect>();

        private void Awake()
        {
            pathTilemap = GameObject.Find("Path Tilemap").GetComponent<Tilemap>();
        }

        private void Start()
        {
            currentSpeed = baseSpeed;
        }

        private void Update()
        {
            Vector3Int cellPos = pathTilemap.WorldToCell(transform.position);

            // ���� �ٲ�� ���� ȿ�� ���� �� �� ȿ�� ����
            if (cellPos != currentCell)
            {
                // ���� ȿ�� ����
                foreach (var effect in activeEffects)
                {
                    effect.OnExit(gameObject);
                }
                activeEffects.Clear();
                currentCell = cellPos;

                // �� ���� ȿ�� ��������
                CustomTile tile = pathTilemap.GetTile<CustomTile>(cellPos);
                if (tile != null && tile.tileData.effects != null)
                {
                    foreach (TileEffect effect in tile.tileData.effects)
                    {
                        effect.OnEnter(gameObject);
                        activeEffects.Add(effect);
                    }
                }
            }
            else
            {
                // ���� ���� �ӹ����� ���� ȿ�� ������Ʈ
                foreach (var effect in activeEffects)
                {
                    effect.OnStay(gameObject, Time.deltaTime);
                }
            }
        }

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
                    transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
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
            health -= amount;
            Debug.Log("Health: " + health);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void ApplySlow(float multiplier)
        {           
            currentSpeed = baseSpeed * multiplier;
            Debug.Log("Applying slow to enemy: " + currentSpeed);
        }

        public void RemoveSlow()
        {
            currentSpeed = baseSpeed;
            Debug.Log("Slow removed. Speed reset to: " + currentSpeed);
        }
    }
}
