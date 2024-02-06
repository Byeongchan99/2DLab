using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetingSystem : MonoBehaviour
{
    public RectTransform aimPoint; // UI 조준점
    public Camera gameCamera;

    void Update()
    {
        AutoAimAtNearestTarget();
    }

    void AutoAimAtNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            Vector3 screenPosition = gameCamera.WorldToScreenPoint(enemy.transform.position);

            // 화면상에서의 거리 계산
            float distance = (screenPosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)).magnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            // 선택된 몬스터로 조준점 이동
            Vector3 targetScreenPosition = gameCamera.WorldToScreenPoint(nearestEnemy.position);
            aimPoint.position = targetScreenPosition;
        }
    }
}
