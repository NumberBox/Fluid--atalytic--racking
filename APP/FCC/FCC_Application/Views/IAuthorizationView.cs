using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FCC_Application.Views
{
    public interface IAuthorizationView : IView
    {
        
        Form GetForm();
        event Action GotAccess;
        Form AccessIsAllowed(IAppMainWindowView appMainWindowView);
    }
}
