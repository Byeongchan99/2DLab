using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    // 윈도우 API 함수들
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    // SetWindowPos 함수의 uFlags 매개변수에 사용할 상수들
    const uint SWP_NOSIZE = 0x0001; // 창의 크기를 변경하지 않음
    const uint SWP_NOZORDER = 0x0004; // 창의 Z 순서를 변경하지 않음

    private IntPtr hWnd; // 창의 핸들

    private Vector2 startPos; // 시작 위치
    public Vector2 moveRange = new Vector2(50, 50);  // 이동 범위
    int moveSpeed = 5; // 이동 속도

    private Vector2 startSize; // 시작 크기
    public Vector2 sizeRange = new Vector2(960, 540); // 창 크기 범위
    public Vector2 minSize = new Vector2(200, 200); // 창 최소 크기

    private float elapsedTime = 0f;
    private float updateInterval = 0.01f;  // 업데이트 간격

    private Coroutine moveCoroutine;
    private Coroutine resizeCoroutine;

    void Start()
    {
        /*
        // 현재 유니티 창의 핸들을 찾음
        IntPtr unityEditorHWnd = FindWindow("UnityContainerWndClass", "Game");
        if (unityEditorHWnd == IntPtr.Zero)
        {
            Debug.LogError("Unity Editor 핸들을 찾을 수 없습니다.");
            return;
        }

        hWnd = FindWindowEx(unityEditorHWnd, IntPtr.Zero, "UnityGUIViewWndClass", "UnityEditor.GameView");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("핸들을 찾을 수 없습니다.");
            return;
        }
        else
        {
            Debug.Log("핸들 찾음: " + hWnd);
        }
        */

        /*
        // 현재 유니티 창의 핸들을 찾음
        hWnd = FindWindow("UnityContainerWndClass", "2DLab - WindowAPI - Windows, Mac, Linux - Unity 2022.3.13f1 <DX11>");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("핸들을 찾을 수 없습니다.");
            return;
        }
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
            //startPos = new Vector2(rect.Left, rect.Top);
            //startSize = new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
            startSize = new Vector2(400, 300);
            startPos = new Vector2((screenWidth - startSize.x) / 2, (screenHeight - startSize.y) / 2);
            Debug.Log("초기 위치: " + startPos + ", 초기 크기: " + startSize);

            InitWindow();
        }
        else
        {
            Debug.LogError("창 위치를 가져올 수 없습니다.");
        }
    }

    public void StartMoveWindow()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveWindowCoroutine());
    }

    public void StopMoveWindow()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        InitWindow();
    }

    public void StartResizeWindow()
    {
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }
        resizeCoroutine = StartCoroutine(ResizeWindowCoroutine());
    }

    public void StopResizeWindow()
    {
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
            resizeCoroutine = null;
        }
        InitWindow();
    }

    private IEnumerator MoveWindowCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(updateInterval);
            MoveWindow();
        }
    }

    private IEnumerator ResizeWindowCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            ResizeWindow();
        }
    }

    private void InitWindow()
    {
        SetWindowPos(hWnd, IntPtr.Zero, (int)startPos.x, (int)startPos.y, (int)startSize.x, (int)startSize.y, SWP_NOZORDER);
    }

    private void MoveWindow()
    {
        if (hWnd == IntPtr.Zero)
            return;

        elapsedTime += Time.deltaTime;
        float x = Mathf.Sin(elapsedTime) * moveRange.x * moveSpeed;
        float y = Mathf.Cos(elapsedTime) * moveRange.y * moveSpeed;

        if (!SetWindowPos(hWnd, IntPtr.Zero, (int)(startPos.x + x), (int)(startPos.y + y), (int)startSize.x, (int)startSize.y, SWP_NOZORDER))
        {
            Debug.LogError("창 위치를 설정하는 데 실패했습니다.");
        }
        else
        {
            //Debug.Log("창 위치 업데이트됨: " + new Vector2(startPos.x + x, startPos.y + y));
        }
    }

    private void ResizeWindow()
    {
        if (hWnd == IntPtr.Zero)
            return;

        elapsedTime += Time.deltaTime;

        // 창 크기 변경
        float width = startSize.x + Mathf.Sin(elapsedTime) * sizeRange.x;
        float height = startSize.y + Mathf.Cos(elapsedTime) * sizeRange.y;

        // 최소 크기 적용
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        if (!SetWindowPos(hWnd, IntPtr.Zero, (int)startPos.x, (int)startPos.y, (int)width, (int)height, SWP_NOZORDER))
        {
            Debug.LogError("창 크기를 설정하는 데 실패했습니다.");
        }
        else
        {
            //Debug.Log("창 크기 업데이트됨: " + new Vector2(width, height));
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
