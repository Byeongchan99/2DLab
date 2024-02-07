using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTarget : MonoBehaviour
{
    public Camera gameCamera; // 게임에서 사용하는 카메라
    public AutoTargetingSystem autoTargetingSystem; // AutoTargetingSystem 스크립트 참조
    public float shootInterval = 0.3f; // 사격 간의 시간 간격 (초 단위)

    private float lastShootTime; // 마지막으로 사격한 시간

    void Update()
    {
        if (autoTargetingSystem.autoMode)
        {
            // AutoMode가 활성화되고, 마지막 사격 시간으로부터 일정 시간이 경과한 경우에만 사격
            if (Time.time >= lastShootTime + shootInterval)
            {
                AutoShootAtAimPoint();
                lastShootTime = Time.time; // 마지막 사격 시간 업데이트
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
            randomMoveTarget.health--; // 타겟 체력 감소
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
            RandomMoveTarget randomMoveTarget = hit.collider.GetComponent<RandomMoveTarget>();
            randomMoveTarget.health--; // 타겟 체력 감소
        }
    }
}
