using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Utills;
namespace FCC_Application.Views
{
    public class UnityView : IUnityView
    {
        private const string UNITY_WINDOW_CLASS_NAME = "UnityWndClass";
        private const int UNITY_WINDOW_SEARCH_TIMEOUT = 5;

        private Process process;
        public IntPtr handle = IntPtr.Zero;
        string file;
        public void Close()
        {
            try
            {
                process?.Kill();
                process = null;
            }
            catch { }
        }
        private bool StartProcess(IntPtr owner)
        {
            if (File.Exists(GetFileName()))
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo(GetFileName(), "-parentHWND " + owner.ToInt32().ToString());
                    process = Process.Start(info);

                    return true;
                }
                catch { }
            }

            return false;

        }
        private bool WaitWindow(IntPtr owner)
        {
            IntPtr unityWindow = IntPtr.Zero;
            DateTime start = DateTime.Now;
            try
            {
                List<IntPtr> childWindows;
                while (unityWindow == IntPtr.Zero)
                {
                    childWindows = WinApi.GetAllChildHandles(owner);
                    unityWindow = childWindows.FirstOrDefault(w => WinApi.GetWindowClass(w) == UNITY_WINDOW_CLASS_NAME);
                    if ((DateTime.Now - start).TotalSeconds >= UNITY_WINDOW_SEARCH_TIMEOUT)
                    {
                        break;
                    }
                }
            }
            catch { }

            bool success = unityWindow != IntPtr.Zero;
            if (success)
            {
                handle = unityWindow;
                Activate();
            }
            return success;
        }
        public string GetFileName()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            //path = path.Substring(0, path.LastIndexOf("FCC")+3) + file;
            path = path.Substring(0, path.LastIndexOf("FCC")) + file;
            return path;

        }

        public void Open(string file,IntPtr owner, Action<bool> embeddingCallback)
        {
            this.file = file;
            OpenAsync(owner, embeddingCallback);
        }
        private async void OpenAsync(IntPtr owner, Action<bool> embeddingCallback)
        {
            if (StartProcess(owner))
            {
                await Task.Run(() => embeddingCallback(WaitWindow(owner)));
            }
            else
            {
                embeddingCallback(false);
            }
        }

        public void SetSize(int width, int height)
        {
            if (handle != IntPtr.Zero)
            {
                WinApi.SetSize(handle, width, height);
            }
        }

        public void Activate()
        {
            if (handle != IntPtr.Zero)
            {
                WinApi.ActivateUnityWindow(handle);
                //WinApi.SetFocus(handle);
            }
        }

        public void Deactivate()
        {
            if (handle != IntPtr.Zero)
            {
                WinApi.DeactivateUnityWindow(handle);
            }
        }
    }
}
