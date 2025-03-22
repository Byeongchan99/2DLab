using DiceKingdom;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUI : MonoBehaviour, IDeployable, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 originalAnchoredPosition;  // 원래 UI 위치 (AnchoredPosition)
    private bool isDragging = false;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // IDeployable: 타워 선택 시 호출 (드래그 시작)
    public void OnButtonDown()
    {
        // 현재 UI 위치 기록
        originalAnchoredPosition = rectTransform.anchoredPosition;
        isDragging = true;
        SetVisualDragging(true);
        Debug.Log("타워 선택됨. 드래그 시작.");
    }

    // IDeployable: 배치 확정 시 호출 (드래그 종료 후)
    // targetPos는 DeploymentManager에서 변환한 월드 좌표일 수도 있고, UI 캔버스의 좌표일 수도 있음
    public void OnButtonUp(Vector3 targetPos)
    {
        isDragging = false;
        SetVisualDragging(false);

        // targetPos가 유효한 배치 위치라면(예, Vector3.zero가 아니라면) 배치하고, 아니면 원래 위치로 복귀
        if (targetPos != Vector3.zero)
        {
            // 만약 UI에서 게임 월드로 타워를 이동시키는 경우라면, DeploymentManager에서 처리한 후 인스턴스화할 수도 있음.
            // 여기서는 간단하게 RectTransform의 position을 변경합니다.
            rectTransform.position = targetPos;
            Debug.Log("타워 배치 완료: " + targetPos);
        }
        else
        {
            // 유효하지 않으면 원래 UI 위치로 복귀
            rectTransform.anchoredPosition = originalAnchoredPosition;
            Debug.Log("유효하지 않은 배치 위치. 원래 위치로 복귀.");
        }
    }

    // IPointerDownHandler: 타워 UI 클릭 시 DeploymentManager에 선택 요청
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("타워 UI 클릭됨.");
        DeploymentManager.Instance.SelectDeployable(this);
    }

    // IDragHandler: 드래그 중 UI 요소의 위치 업데이트
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        Vector2 localPoint;
        // 부모 RectTransform을 기준으로 로컬 좌표를 구합니다.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    // IPointerUpHandler: 드래그 종료 시 DeploymentManager에 배치 요청
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("타워 UI 드래그 종료.");
        // 스크린 좌표에서 world 좌표로 변환 (UI 타워는 Screen Space - Overlay라고 가정)
        Vector3 screenPos = eventData.position;
        // UI의 경우, 보통 z = 0으로 설정한 후 Camera.main.ScreenToWorldPoint 사용
        // 단, ScreenToWorldPoint의 z값은 카메라와 월드 사이의 거리여야 합니다.
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        DeploymentManager.Instance.DeploySelectedTower(worldPos);
    }

    // 드래그 중 시각적 효과 (예: 반투명)
    private void SetVisualDragging(bool dragging)
    {
        // UI 요소인 경우 Image 컴포넌트를 사용할 수도 있고,
        // SpriteRenderer를 사용하는 경우에도 아래처럼 적용할 수 있습니다.
        var image = GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.color = dragging ? new Color(1f, 1f, 1f, 0.5f) : Color.white;
        }
        else
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = dragging ? new Color(1f, 1f, 1f, 0.5f) : Color.white;
            }
        }
    }
}
