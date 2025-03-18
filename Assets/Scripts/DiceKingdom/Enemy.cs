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

            // 셀이 바뀌면 기존 효과 종료 후 새 효과 적용
            if (cellPos != currentCell)
            {
                // 기존 효과 종료
                foreach (var effect in activeEffects)
                {
                    effect.OnExit(gameObject);
                }
                activeEffects.Clear();
                currentCell = cellPos;

                // 새 셀의 효과 가져오기
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
                // 같은 셀에 머무르는 동안 효과 업데이트
                foreach (var effect in activeEffects)
                {
                    effect.OnStay(gameObject, Time.deltaTime);
                }
            }
        }

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
                    transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
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
