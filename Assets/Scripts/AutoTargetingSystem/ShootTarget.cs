using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    public Camera gameCamera; // ���ӿ��� ����ϴ� ī�޶�

    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            ShootAtMousePosition();
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
            ObjectPoolManager.Instance.ReturnTargetToPool(hit.collider.gameObject); // ������Ʈ Ǯ�� ��ȯ
        }
    }
}
