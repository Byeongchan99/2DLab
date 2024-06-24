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
    public EdgeCollider2D screenCollider; // ȭ�� ũ�⸸ŭ�� Edge Collider 2D
    public Text currentSizeText; // ���� â ũ�⸦ ǥ���� Text

    // SetWindowPos �Լ��� uFlags �Ű������� ����� �����
    const uint SWP_NOSIZE = 0x0001; // â�� ũ�⸦ �������� ����
    const uint SWP_NOZORDER = 0x0004; // â�� Z ������ �������� ����

    private IntPtr hWnd; // â�� �ڵ�

    public Vector2 minSize = new Vector2(360, 200); // â �ּ� ũ��
    public float expandAmountX = 9f; // â ũ�⸦ Ȯ���ϴ� ��
    public float expandAmountY = 5f; // â ũ�⸦ Ȯ���ϴ� ��
    public float shrinkSpeedX = 9f; // â ��� �ӵ�
    public float shrinkSpeedY = 5f; // â ��� �ӵ�

    private Vector2 startPos; // ���� ��ġ
    private Vector2 startSize; // ���� ũ��
    private Vector2 currentSize; // ���� â ũ��
    private float updateInterval = 0.01f;  // ������Ʈ ����

    private Coroutine shrinkCoroutine;

    void Start()
    {
        /*
        startSize = new Vector2(900, 500);
        currentSize = startSize;
        UpdateColliderSize();
        */

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
            startSize = new Vector2(900, 500);
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
        SetWindowPos(hWnd, IntPtr.Zero, (int)startPos.x, (int)startPos.y, (int)startSize.x, (int)startSize.y, SWP_NOZORDER);
        UpdateColliderSize();
    }

    private void ShrinkWindowSize()
    {
        if (hWnd == IntPtr.Zero)
            return;

        // â ũ�� ���
        float width = currentSize.x - shrinkSpeedX * updateInterval;
        float height = currentSize.y - shrinkSpeedY * updateInterval;

        // �ּ� ũ�� ����
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        currentSize.x = width;
        currentSize.y = height;

        // �ؽ�Ʈ ������Ʈ
        UpdateCurrentSizeText();

        SetWindowPos(hWnd, IntPtr.Zero, (int)(startPos.x + (startSize.x - width) / 2), (int)(startPos.y + (startSize.y - height) / 2), (int)width, (int)height, SWP_NOZORDER);
        UpdateColliderSize();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹");
            AdjustWindowSize(collision.collider);
        }
    }

    private void AdjustWindowSize(Collider2D collision)
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
            currentSize.x += expandAmountX;
            Debug.Log("Right collision");
        }
        else if (angle >= 135 || angle <= -135) // ���� �浹
        {
            currentSize.x += expandAmountX;
            Debug.Log("Left collision");
        }
        else if (angle > 45 && angle < 135) // ���� �浹
        {
            currentSize.y += expandAmountY;
            Debug.Log("Top collision");
        }
        else if (angle > -135 && angle < -45) // �Ʒ��� �浹
        {
            currentSize.y += expandAmountY;
            Debug.Log("Bottom collision");
        }

        // �ּ� ũ�� ����
        currentSize.x = Mathf.Max(currentSize.x, minSize.x);
        currentSize.y = Mathf.Max(currentSize.y, minSize.y);

        // �ؽ�Ʈ ������Ʈ
        UpdateCurrentSizeText();

        // â ũ�� ����
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, (int)currentSize.x, (int)currentSize.y, SWP_NOZORDER);

        // �ݶ��̴� ũ�� ������Ʈ
        UpdateColliderSize();
    }


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

    private void UpdateColliderSize()
    {
        Debug.Log("�ݶ��̴� ������Ʈ");
        if (screenCollider != null)
        {
            // �ݶ��̴��� ũ�⸦ â ũ�⿡ ���߾� ������ ����
            screenCollider.transform.localScale = new Vector3(currentSize.x / startSize.x, currentSize.y / startSize.y, 1);
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
