using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WindowGame : MonoBehaviour
{
    // 윈도우 API 함수들
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    public Transform playerTransform; // 플레이어의 Transform
    public EdgeCollider2D screenCollider; // 화면 크기만큼의 Edge Collider 2D
    public Text currentSizeText; // 현재 창 크기를 표시할 Text

    // SetWindowPos 함수의 uFlags 매개변수에 사용할 상수들
    const uint SWP_NOSIZE = 0x0001; // 창의 크기를 변경하지 않음
    const uint SWP_NOZORDER = 0x0004; // 창의 Z 순서를 변경하지 않음

    private IntPtr hWnd; // 창의 핸들

    public Vector2 minSize = new Vector2(360, 200); // 창 최소 크기
    public float expandAmountX = 9f; // 창 크기를 확장하는 양
    public float expandAmountY = 5f; // 창 크기를 확장하는 양
    public float shrinkSpeedX = 9f; // 창 축소 속도
    public float shrinkSpeedY = 5f; // 창 축소 속도

    private Vector2 startPos; // 시작 위치
    private Vector2 startSize; // 시작 크기
    private Vector2 currentSize; // 현재 창 크기
    private float updateInterval = 0.01f;  // 업데이트 간격

    private Coroutine shrinkCoroutine;

    void Start()
    {
        /*
        startSize = new Vector2(900, 500);
        currentSize = startSize;
        UpdateColliderSize();
        */

        // 현재 빌드한 게임 창의 핸들을 찾음
        hWnd = FindWindow("UnityWndClass", "2DLab");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("핸들을 찾을 수 없습니다.");
            return;
        }

        // 화면 해상도 가져오기
        int screenWidth = GetSystemMetrics(0); // SM_CXSCREEN
        int screenHeight = GetSystemMetrics(1); // SM_CYSCREEN
        Debug.Log("화면 해상도: " + screenWidth + "x" + screenHeight);

        // 현재 창 위치를 가져옴
        RECT rect;
        if (GetWindowRect(hWnd, out rect))
        {
            startSize = new Vector2(900, 500);
            startPos = new Vector2((screenWidth - startSize.x) / 2, (screenHeight - startSize.y) / 2);
            currentSize = startSize;
            Debug.Log("초기 위치: " + startPos + ", 초기 크기: " + startSize);
            // 텍스트 표시
            UpdateCurrentSizeText();

            InitWindow();
        }
        else
        {
            Debug.LogError("창 위치를 가져올 수 없습니다.");
        }

        shrinkCoroutine = StartCoroutine(ShrinkWindow());

        // 콜라이더 초기화
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

        // 창 크기 축소
        float width = currentSize.x - shrinkSpeedX * updateInterval;
        float height = currentSize.y - shrinkSpeedY * updateInterval;

        // 최소 크기 적용
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        currentSize.x = width;
        currentSize.y = height;

        // 텍스트 업데이트
        UpdateCurrentSizeText();

        SetWindowPos(hWnd, IntPtr.Zero, (int)(startPos.x + (startSize.x - width) / 2), (int)(startPos.y + (startSize.y - height) / 2), (int)width, (int)height, SWP_NOZORDER);
        UpdateColliderSize();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌");
            AdjustWindowSize(collision.collider);
        }
    }

    private void AdjustWindowSize(Collider2D collision)
    {
        if (hWnd == IntPtr.Zero)
            return;

        Vector2 collisionPoint = collision.ClosestPoint(screenCollider.transform.position);
        Vector2 direction = (collisionPoint - (Vector2)screenCollider.transform.position).normalized;

        // 기울기 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 각 방향에 따라 창의 크기 조정
        if (angle >= -45 && angle <= 45) // 오른쪽 충돌
        {
            currentSize.x += expandAmountX;
            Debug.Log("Right collision");
        }
        else if (angle >= 135 || angle <= -135) // 왼쪽 충돌
        {
            currentSize.x += expandAmountX;
            Debug.Log("Left collision");
        }
        else if (angle > 45 && angle < 135) // 위쪽 충돌
        {
            currentSize.y += expandAmountY;
            Debug.Log("Top collision");
        }
        else if (angle > -135 && angle < -45) // 아래쪽 충돌
        {
            currentSize.y += expandAmountY;
            Debug.Log("Bottom collision");
        }

        // 최소 크기 적용
        currentSize.x = Mathf.Max(currentSize.x, minSize.x);
        currentSize.y = Mathf.Max(currentSize.y, minSize.y);

        // 텍스트 업데이트
        UpdateCurrentSizeText();

        // 창 크기 조정
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, (int)currentSize.x, (int)currentSize.y, SWP_NOZORDER);

        // 콜라이더 크기 업데이트
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
        Debug.Log("콜라이더 업데이트");
        if (screenCollider != null)
        {
            // 콜라이더의 크기를 창 크기에 맞추어 스케일 조정
            screenCollider.transform.localScale = new Vector3(currentSize.x / startSize.x, currentSize.y / startSize.y, 1);
        }
    }

    // 창의 위치와 크기
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
