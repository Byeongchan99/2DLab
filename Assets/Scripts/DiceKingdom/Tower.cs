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
        public int range; // ���� ����
        public float damage; // Ÿ�� ���ݷ� = �߻�ü ���ݷ�
        public float attackSpeed; // �ʴ� ���� Ƚ��
        public GameObject currentTarget; // ���� Ÿ��
        public float attackCooldown = 0f; // ���� �ֱ� Ÿ�̸�

        //public CircleCollider2D rangeCollider; // ���� ���� �ݶ��̴�
        public GameObject projectilePrefab; // �߻�ü ������

        private Vector3 originalPosition;   // �̵� �� ���� ��ġ
        private bool isDragging = false;

        void Awake()
        {
            currentTarget = null;
        }

        void Update()
        {
            // Ÿ���� ������ �ƹ� �͵� ���� ����
            if (currentTarget == null)
                return;

            // ���� �ֱ⸦ ���� Ÿ�̸� ������Ʈ
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f)
            {
                FireProjectile();
                attackCooldown = 1f / attackSpeed;
            }
        }

        private void FireProjectile()
        {
            // �߻�ü ���� �� Ÿ�� ���� ���� (����)
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // �߻�ü ��ũ��Ʈ���� Ÿ���� ������ �� �ֵ��� ���� �ڵ�
            TowerProjectile projectileScript = projectile.GetComponent<TowerProjectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(currentTarget, damage);
            }
        }

        // Tower.cs ����
        public void OnRangeTriggerEnter(Collider2D other)
        {
            Debug.Log("���� Ʈ���� ����: " + other.gameObject.name);
            // �� ���� ���� ó��...
            currentTarget = other.gameObject;
        }

        public void OnRangeTriggerExit(Collider2D other)
        {
            Debug.Log("���� Ʈ���� ��Ż: " + other.gameObject.name);
            // �� ��Ż ó��...
            if (other.gameObject == currentTarget)
            {
                Debug.Log("Target lost: " + other.gameObject.name);
                currentTarget = null;
            }
        }

        public void OnButtonDown()
        {
            // ���� ��ġ ��� �� �巡�� ���� �÷��� Ȱ��ȭ
            originalPosition = transform.position;
            isDragging = true;
            SetVisualDragging(true);
            Debug.Log("Ÿ�� ���õ�. �巡�� ����.");
        }

        // IDeployable ����: ��ġ Ȯ�� �� (�巡�� ���� ��)
        public void OnButtonUp(Vector3 targetPos)
        {
            isDragging = false;
            SetVisualDragging(false);

            // ��ȿ�� ��ġ Ÿ���̸� �� ��ġ��, �ƴϸ� ���� ��ġ�� ����
            if (targetPos != Vector3.zero)
            {
                transform.position = targetPos;
                Debug.Log("Ÿ�� ��ġ �Ϸ�: " + targetPos);
            }
            else
            {
                transform.position = originalPosition;
                Debug.Log("��ȿ���� ���� ��ġ ��ġ. ���� ��ġ�� ����.");
            }
        }

        // IPointerDownHandler ����: Ÿ���� Ŭ������ �� DeploymentManager�� ���� ��û
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Ÿ�� Ŭ����.");
            DeploymentManager.Instance.SelectDeployable(this);
        }

        // IDragHandler ����: �巡�� �� �����͸� ���� Ÿ�� ��ġ ������Ʈ
        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging)
                return;
            // �巡�� ��, Tower�� �̹� ȭ����� ��ġ�� ���� ��ǥ�� ��ȯ�Ͽ� �̵��ϵ��� �ߴٰ� ����
            Vector3 screenPos = eventData.position;
            // ���� ���� ���� Ÿ���� �׻� ī�޶��� z �����¸� ����Ͽ� ��ȯ
            screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            transform.position = worldPos;
        }

        // IPointerUpHandler ����: �巡�� ���� �� ��ġ Ȯ�� ó�� ��û
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Ÿ�� �巡�� ����.");
            DeploymentManager.Instance.DeploySelectedTower(transform.position);
        }

        // �巡�� �� Ÿ���� �ð��� ȿ�� ó�� - ������ ȿ��
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
