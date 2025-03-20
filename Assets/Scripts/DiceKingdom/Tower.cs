using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DiceKingdom
{
    public interface IDeployable
    {
        public void OnButtonDown();
        public void OnButtonUp(Vector3 targetPosition);
    }

    public class Tower : MonoBehaviour, IDeployable, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public int range; // 공격 범위
        public float damage; // 타워 공격력 = 발사체 공격력
        public float attackSpeed; // 초당 공격 횟수
        public GameObject currentTarget; // 현재 타겟
        public float attackCooldown = 0f; // 공격 주기 타이머

        //public CircleCollider2D rangeCollider; // 공격 범위 콜라이더
        public GameObject projectilePrefab; // 발사체 프리팹

        private Vector3 originalPosition;   // 이동 전 원래 위치
        private bool isDragging = false;

        void Awake()
        {
            currentTarget = null;
        }

        void Update()
        {
            // 타겟이 없으면 아무 것도 하지 않음
            if (currentTarget == null)
                return;

            // 공격 주기를 위해 타이머 업데이트
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                FireProjectile();
                attackCooldown = 1f / attackSpeed;
            }
        }

        private void FireProjectile()
        {
            // 발사체 생성 및 타겟 설정 로직 (예시)
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // 발사체 스크립트에서 타겟을 설정할 수 있도록 예시 코드
            TowerProjectile projectileScript = projectile.GetComponent<TowerProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(currentTarget, damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("OnTriggerEnter2D");
            // 새로운 적이 타워의 공격 범위에 들어왔을 때
            if (collision.gameObject.tag == "Enemy" && currentTarget == null)
            {
                Debug.Log("New target acquired: " + collision.gameObject.name);
                currentTarget = collision.gameObject;              
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // 타겟인 적이 타워의 공격 범위를 벗어났을 때
            if (collision.gameObject == currentTarget)
            {
                Debug.Log("Target lost: " + collision.gameObject.name);
                currentTarget = null;
            }
        }

        public void OnButtonDown()
        {
            // 원래 위치 기록 및 드래그 시작 플래그 활성화
            originalPosition = transform.position;
            isDragging = true;
            SetVisualDragging(true);
            Debug.Log("타워 선택됨. 드래그 시작.");
        }

        // IDeployable 구현: 배치 확정 시 (드래그 종료 후)
        public void OnButtonUp(Vector3 targetPos)
        {
            isDragging = false;
            SetVisualDragging(false);

            // 유효한 배치 타일이면 그 위치로, 아니면 원래 위치로 복귀
            if (targetPos != Vector3.zero)
            {
                transform.position = targetPos;
                Debug.Log("타워 배치 완료: " + targetPos);
            }
            else
            {
                transform.position = originalPosition;
                Debug.Log("유효하지 않은 배치 위치. 원래 위치로 복귀.");
            }
        }

        // IPointerDownHandler 구현: 타워를 클릭했을 때 DeploymentManager에 선택 요청
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("타워 클릭됨.");
            DeploymentManager.Instance.SelectDeployable(this);
        }

        // IDragHandler 구현: 드래그 중 포인터를 따라 타워 위치 업데이트
        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            worldPos.z = 0f;  // 2D 게임이므로 z는 0으로 고정
            transform.position = worldPos;
        }

        // IPointerUpHandler 구현: 드래그 종료 시 배치 확정 처리 요청
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("타워 드래그 종료.");
            // 배치 가능한 타일의 유효성을 확인하는 것은 DeploymentManager가 처리합니다.
            Vector3 pointerWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            pointerWorldPos.z = 0f;
            DeploymentManager.Instance.DeploySelectedTower(pointerWorldPos);
        }

        // 드래그 중 타워의 시각적 효과 처리 (예: 반투명 효과)
        private void SetVisualDragging(bool dragging)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = dragging ? new Color(1f, 1f, 1f, 0.5f) : Color.white;
            }
        }
    }
}
