using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    public Camera gameCamera; // ���ӿ��� ����ϴ� ī�޶�
    public AutoTargetingSystem autoTargetingSystem; // AutoTargetingSystem ��ũ��Ʈ ����
    public float shootInterval = 0.3f; // ��� ���� �ð� ���� (�� ����)

    private float lastShootTime; // ���������� ����� �ð�

    void Update()
    {
        if (autoTargetingSystem.autoMode)
        {
            // AutoMode�� Ȱ��ȭ�ǰ�, ������ ��� �ð����κ��� ���� �ð��� ����� ��쿡�� ���
            if (Time.time >= lastShootTime + shootInterval)
            {
                AutoShootAtAimPoint();
                lastShootTime = Time.time; // ������ ��� �ð� ������Ʈ
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootAtMousePosition();
            }
        }
    }

    void AutoShootAtAimPoint()
    {
        Vector3 aimPointPos = gameCamera.ScreenToWorldPoint(new Vector3(autoTargetingSystem.aimPoint.position.x, autoTargetingSystem.aimPoint.position.y, gameCamera.nearClipPlane));
        Vector2 aimPointPos2D = new Vector2(aimPointPos.x, aimPointPos.y);

        RaycastHit2D hit = Physics2D.Raycast(aimPointPos2D, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            RandomMoveTarget randomMoveTarget = hit.collider.GetComponent<RandomMoveTarget>();
            randomMoveTarget.health--; // Ÿ�� ü�� ����
        }
    }

    void ShootAtMousePosition()
    {
        // ���콺 �������� ��ũ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameCamera.nearClipPlane));
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        // ���콺 ��ġ���� ����ĳ��Ʈ ����
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        // ����ĳ��Ʈ�� Ÿ���� ����Ǹ�
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            RandomMoveTarget randomMoveTarget = hit.collider.GetComponent<RandomMoveTarget>();
            randomMoveTarget.health--; // Ÿ�� ü�� ����
        }
    }
}
