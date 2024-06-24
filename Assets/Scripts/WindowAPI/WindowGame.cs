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
    public EdgeCollider2D screenCollider; // 화면의 Edge Collider 2D

    public Text currentSizeText; // 현재 창 크기를 표시할 Text

    // SetWindowPos 함수의 uFlags 매개변수에 사용할 상수들
    const uint SWP_NOZORDER = 0x0004; // 창의 Z 순서를 변경하지 않음

    private IntPtr hWnd; // 창의 핸들

    public float screenToWorldScale = 0.01f; // 화면 픽셀을 Unity 월드 단위로 변환하는 비율

    public Vector2 minSize = new Vector2(300, 200); // 창 최소 크기
    public float expandAmount = 10f; // 창 확장 정도
    public float shrinkAmount = 1f; // 창 축소 정도

    private Vector2 startPos; // 시작 위치
    private Vector2 startSize; // 시작 크기
    private Vector2 currentSize; // 현재 창 크기
    private float updateInterval = 0.01f;  // 업데이트 간격

    private Coroutine shrinkCoroutine;

    void Start()
    {
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
            startSize = new Vector2(900, 600);
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
        SetWindowPos(hWnd, IntPtr.Zero, Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y), Mathf.RoundToInt(startSize.x), Mathf.RoundToInt(startSize.y), SWP_NOZORDER);
        UpdateColliderSize();
    }

    private void ShrinkWindowSize()
    {
        if (hWnd == IntPtr.Zero)
            return;

        // 창 크기 축소
        float width = currentSize.x - shrinkAmount;
        float height = currentSize.y - shrinkAmount;

        // 최소 크기 적용
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        currentSize.x = width;
        currentSize.y = height;

        // 텍스트 업데이트
        UpdateCurrentSizeText();

        SetWindowPos(hWnd, IntPtr.Zero, Mathf.RoundToInt(startPos.x + (startSize.x - width) / 2), Mathf.RoundToInt(startPos.y + (startSize.y - height) / 2), Mathf.RoundToInt(width), Mathf.RoundToInt(height), SWP_NOZORDER);
        UpdateColliderSize();
    }

    // 플레이어와 충돌 시 창 크기 조정
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌");
            AdjustWindowSize(collision.collider);
        }
    }

    public void AdjustWindowSize(Collider2D collision)
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
            currentSize.x += expandAmount;
            Debug.Log("Right collision");
        }
        else if (angle >= 135 || angle <= -135) // 왼쪽 충돌
        {
            currentSize.x += expandAmount;
            Debug.Log("Left collision");
        }
        else if (angle > 45 && angle < 135) // 위쪽 충돌
        {
            currentSize.y += expandAmount;
            Debug.Log("Top collision");
        }
        else if (angle > -135 && angle < -45) // 아래쪽 충돌
        {
            currentSize.y += expandAmount;
            Debug.Log("Bottom collision");
        }

        // 최소 크기 적용
        currentSize.x = Mathf.Max(currentSize.x, minSize.x);
        currentSize.y = Mathf.Max(currentSize.y, minSize.y);

        // 텍스트 업데이트
        UpdateCurrentSizeText();

        // 창 크기 조정
        SetWindowPos(hWnd, IntPtr.Zero, 0, 0, Mathf.RoundToInt(currentSize.x), Mathf.RoundToInt(currentSize.y), SWP_NOZORDER);

        // 콜라이더 크기 업데이트
        UpdateColliderSize();
    }

    // 현재 창 크기 텍스트 업데이트
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
        Debug.Log("콜라이더 업데이트");
        if (screenCollider != null)
        {
            // 화면 픽셀 단위를 Unity 월드 단위로 변환
            Vector2 worldSize = currentSize * screenToWorldScale;

            // Edge Collider 2D의 포인트 설정
            Vector2[] points = new Vector2[5];
            points[0] = new Vector2(-worldSize.x / 2, worldSize.y / 2);
            points[1] = new Vector2(-worldSize.x / 2, -worldSize.y / 2);
            points[2] = new Vector2(worldSize.x / 2, -worldSize.y / 2);
            points[3] = new Vector2(worldSize.x / 2, worldSize.y / 2);
            points[4] = points[0]; // 첫 번째 점으로 다시 연결하여 폐쇄된 루프를 형성

            screenCollider.points = points;
        }
    }
    */
    
    private void UpdateColliderSize()
    {
        Debug.Log("콜라이더 업데이트");
        if (screenCollider != null)
        {
            screenCollider.transform.localScale = new Vector3(currentSize.x / 100, currentSize.y / 100, 1);
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
