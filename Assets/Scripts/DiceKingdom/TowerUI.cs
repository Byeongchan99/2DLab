using DiceKingdom;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUI : MonoBehaviour, IDeployable, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 originalAnchoredPosition;  // ���� UI ��ġ (AnchoredPosition)
    private bool isDragging = false;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // IDeployable: Ÿ�� ���� �� ȣ�� (�巡�� ����)
    public void OnButtonDown()
    {
        // ���� UI ��ġ ���
        originalAnchoredPosition = rectTransform.anchoredPosition;
        isDragging = true;
        SetVisualDragging(true);
        Debug.Log("Ÿ�� ���õ�. �巡�� ����.");
    }

    // IDeployable: ��ġ Ȯ�� �� ȣ�� (�巡�� ���� ��)
    // targetPos�� DeploymentManager���� ��ȯ�� ���� ��ǥ�� ���� �ְ�, UI ĵ������ ��ǥ�� ���� ����
    public void OnButtonUp(Vector3 targetPos)
    {
        isDragging = false;
        SetVisualDragging(false);

        // targetPos�� ��ȿ�� ��ġ ��ġ���(��, Vector3.zero�� �ƴ϶��) ��ġ�ϰ�, �ƴϸ� ���� ��ġ�� ����
        if (targetPos != Vector3.zero)
        {
            // ���� UI���� ���� ����� Ÿ���� �̵���Ű�� �����, DeploymentManager���� ó���� �� �ν��Ͻ�ȭ�� ���� ����.
            // ���⼭�� �����ϰ� RectTransform�� position�� �����մϴ�.
            rectTransform.position = targetPos;
            Debug.Log("Ÿ�� ��ġ �Ϸ�: " + targetPos);
        }
        else
        {
            // ��ȿ���� ������ ���� UI ��ġ�� ����
            rectTransform.anchoredPosition = originalAnchoredPosition;
            Debug.Log("��ȿ���� ���� ��ġ ��ġ. ���� ��ġ�� ����.");
        }
    }

    // IPointerDownHandler: Ÿ�� UI Ŭ�� �� DeploymentManager�� ���� ��û
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Ÿ�� UI Ŭ����.");
        DeploymentManager.Instance.SelectDeployable(this);
    }

    // IDragHandler: �巡�� �� UI ����� ��ġ ������Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        Vector2 localPoint;
        // �θ� RectTransform�� �������� ���� ��ǥ�� ���մϴ�.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    // IPointerUpHandler: �巡�� ���� �� DeploymentManager�� ��ġ ��û
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Ÿ�� UI �巡�� ����.");
        // ��ũ�� ��ǥ���� world ��ǥ�� ��ȯ (UI Ÿ���� Screen Space - Overlay��� ����)
        Vector3 screenPos = eventData.position;
        // UI�� ���, ���� z = 0���� ������ �� Camera.main.ScreenToWorldPoint ���
        // ��, ScreenToWorldPoint�� z���� ī�޶�� ���� ������ �Ÿ����� �մϴ�.
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;
        DeploymentManager.Instance.DeploySelectedTower(worldPos);
    }

    // �巡�� �� �ð��� ȿ�� (��: ������)
    private void SetVisualDragging(bool dragging)
    {
        // UI ����� ��� Image ������Ʈ�� ����� ���� �ְ�,
        // SpriteRenderer�� ����ϴ� ��쿡�� �Ʒ�ó�� ������ �� �ֽ��ϴ�.
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
