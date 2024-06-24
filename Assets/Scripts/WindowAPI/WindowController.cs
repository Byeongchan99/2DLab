using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    // ������ API �Լ���
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

    // SetWindowPos �Լ��� uFlags �Ű������� ����� �����
    const uint SWP_NOSIZE = 0x0001; // â�� ũ�⸦ �������� ����
    const uint SWP_NOZORDER = 0x0004; // â�� Z ������ �������� ����

    private IntPtr hWnd; // â�� �ڵ�

    private Vector2 startPos; // ���� ��ġ
    public Vector2 moveRange = new Vector2(50, 50);  // �̵� ����
    int moveSpeed = 5; // �̵� �ӵ�

    private Vector2 startSize; // ���� ũ��
    public Vector2 sizeRange = new Vector2(960, 540); // â ũ�� ����
    public Vector2 minSize = new Vector2(200, 200); // â �ּ� ũ��

    private float elapsedTime = 0f;
    private float updateInterval = 0.01f;  // ������Ʈ ����

    private Coroutine moveCoroutine;
    private Coroutine resizeCoroutine;

    void Start()
    {
        /*
        // ���� ����Ƽ â�� �ڵ��� ã��
        IntPtr unityEditorHWnd = FindWindow("UnityContainerWndClass", "Game");
        if (unityEditorHWnd == IntPtr.Zero)
        {
            Debug.LogError("Unity Editor �ڵ��� ã�� �� �����ϴ�.");
            return;
        }

        hWnd = FindWindowEx(unityEditorHWnd, IntPtr.Zero, "UnityGUIViewWndClass", "UnityEditor.GameView");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("�ڵ��� ã�� �� �����ϴ�.");
            return;
        }
        else
        {
            Debug.Log("�ڵ� ã��: " + hWnd);
        }
        */

        /*
        // ���� ����Ƽ â�� �ڵ��� ã��
        hWnd = FindWindow("UnityContainerWndClass", "2DLab - WindowAPI - Windows, Mac, Linux - Unity 2022.3.13f1 <DX11>");
        if (hWnd == IntPtr.Zero)
        {
            Debug.LogError("�ڵ��� ã�� �� �����ϴ�.");
            return;
        }
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
            //startPos = new Vector2(rect.Left, rect.Top);
            //startSize = new Vector2(rect.Right - rect.Left, rect.Bottom - rect.Top);
            startSize = new Vector2(400, 300);
            startPos = new Vector2((screenWidth - startSize.x) / 2, (screenHeight - startSize.y) / 2);
            Debug.Log("�ʱ� ��ġ: " + startPos + ", �ʱ� ũ��: " + startSize);

            InitWindow();
        }
        else
        {
            Debug.LogError("â ��ġ�� ������ �� �����ϴ�.");
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
            Debug.LogError("â ��ġ�� �����ϴ� �� �����߽��ϴ�.");
        }
        else
        {
            //Debug.Log("â ��ġ ������Ʈ��: " + new Vector2(startPos.x + x, startPos.y + y));
        }
    }

    private void ResizeWindow()
    {
        if (hWnd == IntPtr.Zero)
            return;

        elapsedTime += Time.deltaTime;

        // â ũ�� ����
        float width = startSize.x + Mathf.Sin(elapsedTime) * sizeRange.x;
        float height = startSize.y + Mathf.Cos(elapsedTime) * sizeRange.y;

        // �ּ� ũ�� ����
        width = Mathf.Max(width, minSize.x);
        height = Mathf.Max(height, minSize.y);

        if (!SetWindowPos(hWnd, IntPtr.Zero, (int)startPos.x, (int)startPos.y, (int)width, (int)height, SWP_NOZORDER))
        {
            Debug.LogError("â ũ�⸦ �����ϴ� �� �����߽��ϴ�.");
        }
        else
        {
            //Debug.Log("â ũ�� ������Ʈ��: " + new Vector2(width, height));
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
