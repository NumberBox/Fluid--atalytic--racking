using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCC_Application.Views
{
    public interface IUnityView : IView
    {
        string GetFileName();
        void SetSize(int width, int height);
        void Open(string file, IntPtr  owner, Action<bool> embeddingCallback);
        void Activate();
        void Deactivate();
        void Close();
    }
}
