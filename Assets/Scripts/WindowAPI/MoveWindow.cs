using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveWindow : MonoBehaviour
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
    static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

    // SetWindowPos �Լ��� uFlags �Ű������� ����� �����
    const uint SWP_NOSIZE = 0x0001; // â�� ũ�⸦ �������� ����
    const uint SWP_NOZORDER = 0x0004; // â�� Z ������ �������� ����
    // RedrawWindow �Լ��� flags �Ű������� ����� �����
    const uint RDW_INVALIDATE = 0x0001; // ������ â ������ ��ȿȭ
    const uint RDW_UPDATENOW = 0x0100; // ��ȿȭ�� ������ ��� �ٽ� �׸����� ��û
    const uint RDW_ALLCHILDREN = 0x0080; // ������ â�� �� ���� â�� ��� �ٽ� �׸����� ��û

    private IntPtr hWnd; // â�� �ڵ�
    public float speed = 2f;  // �̵� �ӵ�
    public Vector2 moveRange = new Vector2(50, 50);  // �̵� ����

    private Vector2 startPos;
    private float elapsedTime = 0f;
    private float updateInterval = 0.5f;  // 0.5�� �������� ������Ʈ
    private float nextUpdateTime = 0f;

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

        // ���� â ��ġ�� ������
        RECT rect;
        if (GetWindowRect(hWnd, out rect))
        {
            startPos = new Vector2(rect.Left, rect.Top);
            Debug.Log("�ʱ� ��ġ: " + startPos);
        }
        else
        {
            Debug.LogError("â ��ġ�� ������ �� �����ϴ�.");
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
            Debug.LogError("â ��ġ�� �����ϴ� �� �����߽��ϴ�.");
        }
        else
        {
            //Debug.Log("â ��ġ ������Ʈ��: " + new Vector2(startPos.x + x, startPos.y + y));
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
