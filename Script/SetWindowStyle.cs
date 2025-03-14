using Godot;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// 设置窗体样式，仅适用于Windows系统
/// </summary>
public partial class SetWindowStyle : Node
{
	#region 系统函数

	[DllImport("user32.dll")]
	static extern IntPtr FindWindow(string ipClassName, string ipWindowName);//用于查找窗口的句柄


	[DllImport("user32.dll")]
	static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);//用于改变指定窗口的属性

	[DllImport("user32.dll")]
	static extern IntPtr GetWindowRect(IntPtr hWnd, out RECT lpRect);//用于获取窗口的坐标

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left;   // 左上角X坐标
		public int Top;    // 左上角Y坐标
		public int Right;  // 右下角X坐标
		public int Bottom; // 右下角Y坐标
	}

	#endregion

	#region 窗口参量，具体内容可在微软官网查询

	private bool hotkeyRegistered = false;//用于记录热键是否已注册

	private const int k_gwlExStyle = -20;//获取窗口扩展样式的索引值

	private const uint k_wsExLayered = 0x00080000;//窗口扩展样式中的分层窗口标志

	private const uint k_wsExTransparent = 0x00000020;//窗口扩展样式中的透明窗口标志，可鼠标穿透

	private const uint k_wsExToolWindow = 0x00000080;//设置窗体不在任务栏和任务管理器出现图标

	private IntPtr m_handle;//窗口句柄
	private IntPtr m_taskBarHandle;//窗口句柄


	#endregion


	public override void _Ready()
	{
		m_taskBarHandle = FindWindow("Shell_TrayWnd", null);

		GetWindowRect(m_taskBarHandle, out RECT rect);
		Main.Instance.taskBarSize.X = rect.Right - rect.Left;
		Main.Instance.taskBarSize.Y = rect.Bottom - rect.Top;
		Main.Instance.taskBarPos.X = rect.Left;
		Main.Instance.taskBarPos.Y = rect.Top;

		//在release和debug情况下都可正常获取窗口句柄
#if DEBUG
		m_handle = FindWindow(null, $"{GetWindow().Title} (DeBug)");
#else
        m_handle = FindWindow(null, GetWindow().Title);
#endif
		SetWindowLong(m_handle, k_gwlExStyle, k_wsExLayered | k_wsExToolWindow);
	}


	/// <summary>
	/// 设置窗口鼠标穿透
	/// </summary>
	public void SetClickThrough(bool click_through)
	{
		if (click_through)
		{
			SetWindowLong(m_handle, k_gwlExStyle, k_wsExLayered | k_wsExTransparent | k_wsExToolWindow);
		}
		else
		{
			SetWindowLong(m_handle, k_gwlExStyle, k_wsExLayered | k_wsExToolWindow);
		}
	}
}
