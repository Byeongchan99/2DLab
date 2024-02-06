using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTargetingSystem : MonoBehaviour
{
    public RectTransform aimPoint; // UI ������
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

            // ȭ��󿡼��� �Ÿ� ���
            float distance = (screenPosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)).magnitude;

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            // ���õ� ���ͷ� ������ �̵�
            Vector3 targetScreenPosition = gameCamera.WorldToScreenPoint(nearestEnemy.position);
            aimPoint.position = targetScreenPosition;
        }
    }
}
