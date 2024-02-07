using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetingSystem : MonoBehaviour
{
    public RectTransform aimPoint; // UI ������
    public Camera gameCamera;
    public bool autoMode = false; // ���� ��� Ȱ��ȭ ����
    [SerializeField] private float smoothTime; // SmoothDamp�� �ε巯�� �̵��� ���� �ð�
    [SerializeField] private float thresholdDistance; // SmoothDamp���� ���� �ӵ��� ��ȯ�ϴ� �Ӱ� �Ÿ�
    [SerializeField] private float constantSpeed; // ������ �ӵ��� Ÿ���� ������ ���� �ӵ�

    private Vector3 currentVelocity = Vector3.zero; // ���� �ӵ� (SmoothDamp�� �ʿ�)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            autoMode = !autoMode;
        }

        if (autoMode)
        {
            AutoAimAtNearestTarget();
        }
        else
        {
            MoveAimPointToMousePosition();
        }
    }

    void MoveAimPointToMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        aimPoint.position = mousePosition;
    }

    void AutoAimAtNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPosition = gameCamera.WorldToScreenPoint(enemy.transform.position);
            float distance = (screenPosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)).magnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 targetScreenPosition = gameCamera.WorldToScreenPoint(nearestEnemy.position);
            Vector3 targetPosition2D = new Vector3(targetScreenPosition.x, targetScreenPosition.y, 0);
            Vector3 aimPointPosition2D = new Vector3(aimPoint.position.x, aimPoint.position.y, 0);

            float distanceToTarget = Vector2.Distance(targetPosition2D, aimPointPosition2D);

            if (distanceToTarget > thresholdDistance)
            {
                // Ÿ�ٰ��� �Ÿ��� �Ӱ谪���� �� �� SmoothDamp ���
                aimPoint.position = Vector3.SmoothDamp(aimPoint.position, targetPosition2D, ref currentVelocity, smoothTime);
            }
            else
            {
                // Ÿ�ٰ� ����� ���� ������ �ӵ��� �̵�
                aimPoint.position = Vector3.MoveTowards(aimPoint.position, targetPosition2D, constantSpeed * Time.deltaTime);
            }
        }
    }
}
