using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveWindow : MonoBehaviour
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
    static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

    // SetWindowPos 함수의 uFlags 매개변수에 사용할 상수들
    const uint SWP_NOSIZE = 0x0001; // 창의 크기를 변경하지 않음
    const uint SWP_NOZORDER = 0x0004; // 창의 Z 순서를 변경하지 않음
    // RedrawWindow 함수의 flags 매개변수에 사용할 상수들
    const uint RDW_INVALIDATE = 0x0001; // 지정된 창 영역을 무효화
    const uint RDW_UPDATENOW = 0x0100; // 무효화된 영역을 즉시 다시 그리도록 요청
    const uint RDW_ALLCHILDREN = 0x0080; // 지정된 창과 그 하위 창을 모두 다시 그리도록 요청

    private IntPtr hWnd; // 창의 핸들
    public float speed = 2f;  // 이동 속도
    public Vector2 moveRange = new Vector2(50, 50);  // 이동 범위

    private Vector2 startPos;
    private float elapsedTime = 0f;
    private float updateInterval = 0.5f;  // 0.5초 간격으로 업데이트
    private float nextUpdateTime = 0f;

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

        // 현재 창 위치를 가져옴
        RECT rect;
        if (GetWindowRect(hWnd, out rect))
        {
            startPos = new Vector2(rect.Left, rect.Top);
            Debug.Log("초기 위치: " + startPos);
        }
        else
        {
            Debug.LogError("창 위치를 가져올 수 없습니다.");
        }
    }

    void Update()
    {
        if (hWnd == IntPtr.Zero)
            return;

        elapsedTime += Time.deltaTime * speed;
        float x = Mathf.Sin(elapsedTime) * moveRange.x;
        float y = Mathf.Cos(elapsedTime) * moveRange.y;

        if (!SetWindowPos(hWnd, IntPtr.Zero, (int)(startPos.x + x), (int)(startPos.y + y), 0, 0, SWP_NOSIZE | SWP_NOZORDER))
        {
            Debug.LogError("창 위치를 설정하는 데 실패했습니다.");
        }
        else
        {
            //Debug.Log("창 위치 업데이트됨: " + new Vector2(startPos.x + x, startPos.y + y));
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
