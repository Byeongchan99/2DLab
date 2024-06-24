using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WindowGame : MonoBehaviour
{
    // ������ API �Լ���
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    public Transform playerTransform; // �÷��̾��� Transform
    public EdgeCollider2D screenCollider; // ȭ���� Edge Collider 2D

    public Text currentSizeText; // ���� â ũ�⸦ ǥ���� Text

    // SetWindowPos �Լ��� uFlags �Ű������� ����� �����
    const uint SWP_NOZORDER = 0x0004; // â�� Z ������ �������� ����

    private IntPtr hWnd; // â�� �ڵ�

    public float screenToWorldScale = 0.01f; // ȭ�� �ȼ��� Unity ���� ������ ��ȯ�ϴ� ����

    public Vector2 minSize = new Vector2(300, 200); // â �ּ� ũ��
    public float expandAmount = 10f; // â Ȯ�� ����
    public float shrinkAmount = 1f; // â ��� ����

    private Vector2 startPos; // ���� ��ġ
    private Vector2 startSize; // ���� ũ��
    private Vector2 currentSize; // ���� â ũ��
    private float updateInterval = 0.01f;  // ������Ʈ ����

    private Coroutine shrinkCoroutine;

    void Start()
    {
        // ���� ������ ���� â�� �ڵ��� ã��
        hWnd = FindWindow("UnityWndClass", "2DLab");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("�ڵ��� ã�� �� �����ϴ�.");
            return;
        }

        // ȭ�� �ػ� ��������
        int screenWidth = GetSystemMetrics(0); // SM_CXSCREEN
        int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN
        Debug.Log("ȭ�� �ػ�: " + screenWidth + "x" + screenHeight);

        // ���� â ��ġ�� ������
        RECT rect;
        if (GetWindowRect(hWnd, out rect))
        {
            startSize = new Vector2(900, 600);
            startPos = new Vector2((screenWidth - startSize.x) / 2, (screenHeight - startSize.y) / 2);
            currentSize = startSize;
            Debug.Log("�ʱ� ��ġ: " + startPos + ", �ʱ� ũ��: " + startSize);
            // �ؽ�Ʈ ǥ��
            UpdateCurrentSizeText();

            InitWindow();
        }
        else
        {
            Debug.LogError("â ��ġ�� ������ �� �����ϴ�.");
        }

        shrinkCoroutine = StartCoroutine(ShrinkWindow());
        // �ݶ��̴� �ʱ�ȭ
        UpdateColliderSize();
    }

    public void StopShrinkWindow()
    {
        if (shrinkCoroutine != null)
        {
            StopCoroutine(shrinkCoroutine);
            shrinkCoroutine = null;
        }
    }

    private IEnumerator ShrinkWindow()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            ShrinkWindowSize();
        }
    }

    private void InitWindow()
    {
        Debug.Log("Init");
        SetWindowPos(hWnd, IntPtr.Zero, Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y), Mathf.RoundToInt(startSize.x), Mathf.RoundToInt(startSize.y), SWP_NOZORDER);
        UpdateColliderSize();
    }

    private void ShrinkWindowSize()
    {
        if (hWnd == IntPtr.Zero)
            return;

        // â ũ�� ���
        float width = currentSize.x - shrinkAmount;
        float height = currentSize.y - shrinkAmount;

        // �ּ� ũ�� ����
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        currentSize.x = width;
        currentSize.y = height;

        // �ؽ�Ʈ ������Ʈ
        UpdateCurrentSizeText();

        SetWindowPos(hWnd, IntPtr.Zero, Mathf.RoundToInt(startPos.x + (startSize.x - width) / 2), Mathf.RoundToInt(startPos.y + (startSize.y - height) / 2), Mathf.RoundToInt(width), Mathf.RoundToInt(height), SWP_NOZORDER);
        UpdateColliderSize();
    }

    // �÷��̾�� �浹 �� â ũ�� ����
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹");
            AdjustWindowSize(collision.collider);
        }
    }

    public void AdjustWindowSize(Collider2D collision)
    {
        if (hWnd == IntPtr.Zero)
            return;

        Vector2 collisionPoint = collision.ClosestPoint(screenCollider.transform.position);
        Vector2 direction = (collisionPoint - (Vector2)screenCollider.transform.position).normalized;

        // ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // �� ���⿡ ���� â�� ũ�� ����
        if (angle >= -45 && angle <= 45) // ������ �浹
        {
            currentSize.x += expandAmount;
            Debug.Log("Right collision");
        }
        else if (angle >= 135 || angle <= -135) // ���� �浹
        {
            currentSize.x += expandAmount;
            Debug.Log("Left collision");
        }
        else if (angle > 45 && angle < 135) // ���� �浹
        {
            currentSize.y += expandAmount;
            Debug.Log("Top collision");
        }
        else if (angle > -135 && angle < -45) // �Ʒ��� �浹
        {
            currentSize.y += expandAmount;
            Debug.Log("Bottom collision");
        }

        // �ּ� ũ�� ����
        currentSize.x = Mathf.Max(currentSize.x, minSize.x);
        currentSize.y = Mathf.Max(currentSize.y, minSize.y);

        // �ؽ�Ʈ ������Ʈ
        UpdateCurrentSizeText();

        // â ũ�� ����
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, Mathf.RoundToInt(currentSize.x), Mathf.RoundToInt(currentSize.y), SWP_NOZORDER);

        // �ݶ��̴� ũ�� ������Ʈ
        UpdateColliderSize();
    }

    // ���� â ũ�� �ؽ�Ʈ ������Ʈ
    private void UpdateCurrentSizeText()
    {
        if (currentSizeText != null)
        {
            currentSizeText.text = $"Current Size: {currentSize.x} x {currentSize.y}";
            Debug.Log($"Current Size: {currentSize.x} x {currentSize.y}");
        }
        else
        {
            Debug.LogError("currentSizeText is not assigned.");
        }
    }
    
    /*
    private void UpdateColliderSize()
    {
        Debug.Log("�ݶ��̴� ������Ʈ");
        if (screenCollider != null)
        {
            // ȭ�� �ȼ� ������ Unity ���� ������ ��ȯ
            Vector2 worldSize = currentSize * screenToWorldScale;

            // Edge Collider 2D�� ����Ʈ ����
            Vector2[] points = new Vector2[5];
            points[0] = new Vector2(-worldSize.x / 2, worldSize.y / 2);
            points[1] = new Vector2(-worldSize.x / 2, -worldSize.y / 2);
            points[2] = new Vector2(worldSize.x / 2, -worldSize.y / 2);
            points[3] = new Vector2(worldSize.x / 2, worldSize.y / 2);
            points[4] = points[0]; // ù ��° ������ �ٽ� �����Ͽ� ���� ������ ����

            screenCollider.points = points;
        }
    }
    */
    
    private void UpdateColliderSize()
    {
        Debug.Log("�ݶ��̴� ������Ʈ");
        if (screenCollider != null)
        {
            screenCollider.transform.localScale = new Vector3(currentSize.x / 100, currentSize.y / 100, 1);
        }
    }
    

    // â�� ��ġ�� ũ��
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
