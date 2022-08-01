using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace FCC_Application.Utills
{
	public class WinApi
	{
		private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
		private static extern bool SetWindowPos(IntPtr hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);
		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetClassName(IntPtr hwnd, StringBuilder lpString, int nMaxCount);
		//[DllImport("user32.dll")]
		//public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
		[DllImport("user32.dll")]
		public static extern void SetFocus(IntPtr hwnd);

		private static List<IntPtr> childWindows;
		private const int WM_ACTIVATE = 0x0006;
		private const int WM_SETFOCUS = 0x0007;
		private const int WM_KILLFOCUS = 0x0008;
		private static readonly IntPtr WA_ACTIVE = new IntPtr(1);
		private static readonly IntPtr WA_INACTIVE = new IntPtr(0);
		

		private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
		{
			childWindows.Add(hWnd);
			EnumChildWindows(hWnd, EnumWindow, IntPtr.Zero);
			return true;
		}
		public static void SetSize(IntPtr hWnd, int cx, int cy)
		{
			SetWindowPos(hWnd, 0, 0, 0, cx, cy, 0);
		}
		public static void ActivateUnityWindow(IntPtr unityHWND)
		{
			SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
			//SendMessage(unityHWND, WM_SETFOCUS, WA_ACTIVE, IntPtr.Zero);
			System.Diagnostics.Debug.WriteLine("A");
		}

		public static void DeactivateUnityWindow(IntPtr unityHWND)
		{
			SendMessage(unityHWND, WM_ACTIVATE, WA_INACTIVE, IntPtr.Zero);
			//SendMessage(unityHWND, WM_KILLFOCUS, WA_INACTIVE, IntPtr.Zero);
			System.Diagnostics.Debug.WriteLine("D");
		}
		public static string GetWindowClass(IntPtr hWnd)
		{
			int length = 256;
			StringBuilder stringBuilder = new StringBuilder(length);
			length = GetClassName(hWnd, stringBuilder, length);

			return stringBuilder.ToString(0, length);
		}

		public static List<IntPtr> GetAllChildHandles(IntPtr handle)
		{
			childWindows = new List<IntPtr>();
			EnumChildWindows(handle, EnumWindow, IntPtr.Zero);
			return childWindows;
		}


	}
}
