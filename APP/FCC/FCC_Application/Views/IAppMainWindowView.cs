
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCC_Application.Views
{
    public interface IAppMainWindowView : IView
    {
        event Action OnLoadedEvent;
        event Action OnClosingEvent;
        event Action OnActivate;
        event Action OnDeactivated;

        void OpenChildWindow(Form ChildForm);
        void CloseChildWindow();
        Form GetForm();
    }
}
