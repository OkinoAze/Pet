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
	private static extern IntPtr FindWindow(string ipClassName, string ipWindowName);

	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

	#endregion

	#region 窗口参量，具体内容可在微软官网查询
	private bool hotkeyRegistered = false;
	private const int k_gwlExStyle = -20;
	private const uint k_wsExLayered = 0x00080000;
	private const uint k_wsExTransparent = 0x00000020;      //设置窗体透明，可鼠标穿透
	private const uint k_wsExToolWindow = 0x00000080;       //设置窗体不在任务栏和任务管理器出现图标
	private IntPtr m_handle;                                //窗口句柄

	#endregion


	public override void _Ready()
	{
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
