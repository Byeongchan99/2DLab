using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    public Camera gameCamera; // 게임에서 사용하는 카메라

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            ShootAtMousePosition();
        }
    }

    void ShootAtMousePosition()
    {
        // 마우스 포지션을 스크린 좌표에서 월드 좌표로 변환
        Vector3 mousePos = gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, gameCamera.nearClipPlane));
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        // 마우스 위치에서 레이캐스트 실행
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        // 레이캐스트에 타겟이 검출되면
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            ObjectPoolManager.Instance.ReturnTargetToPool(hit.collider.gameObject); // 오브젝트 풀로 반환
        }
    }
}
