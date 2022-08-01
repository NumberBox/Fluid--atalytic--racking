using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCC_Application.Views
{
    public interface IUserView : IView
    {
        event Action<int, int> OnResizeEvent;
        event Action<string,int> OnLoadedEvent;
        event Action OnClosingEvent;
        event Action OnActivate;
        event Action OnDeactivated;
        event Action HandInput;
        event Action CaptureInput;
        event Action<string> OnLoadObject;
        event Action<int> OnSelectObject;
        event Action<int> OnDeleteObject;
        event Action<int> OnRelocateObject;

        IntPtr GetUnityParentHandle();
        void ShowEmbeddingFalliedMessage(string fileName);
        void ParseMessage(string message);
        Form GetForm();
    }
}
