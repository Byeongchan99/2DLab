using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetingSystem : MonoBehaviour
{
    public RectTransform aimPoint; // UI 조준점
    public Camera gameCamera;
    public bool autoMode = false; // 오토 모드 활성화 여부
    [SerializeField] private float smoothTime; // SmoothDamp의 부드러운 이동을 위한 시간
    [SerializeField] private float thresholdDistance; // SmoothDamp에서 일정 속도로 전환하는 임계 거리
    [SerializeField] private float constantSpeed; // 일정한 속도로 타겟을 추적할 때의 속도

    private Vector3 currentVelocity = Vector3.zero; // 현재 속도 (SmoothDamp에 필요)

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
                // 타겟과의 거리가 임계값보다 멀 때 SmoothDamp 사용
                aimPoint.position = Vector3.SmoothDamp(aimPoint.position, targetPosition2D, ref currentVelocity, smoothTime);
            }
            else
            {
                // 타겟과 가까울 때는 일정한 속도로 이동
                aimPoint.position = Vector3.MoveTowards(aimPoint.position, targetPosition2D, constantSpeed * Time.deltaTime);
            }
        }
    }
}
