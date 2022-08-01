using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Views;

namespace FCC_Application.Presenters
{
    public class AppMainPresenter : IPresenter
    {
        private IAppMainWindowView appMainWindowView;
        private IAuthorizationView authorizationView;

        public AppMainPresenter(IAppMainWindowView appMainWindowView,IAuthorizationView authorizationView) {
            this.appMainWindowView = appMainWindowView;
            this.authorizationView = authorizationView;

            this.appMainWindowView.OnClosingEvent += AppMainWindowView_OnClosingEvent;
            this.appMainWindowView.OnLoadedEvent += AppMainWindowView_OnLoadedEvent;
            this.authorizationView.GotAccess += AuthorizationView_GotAccess;
        }

        private void AuthorizationView_GotAccess()
        {
            appMainWindowView.OpenChildWindow(authorizationView.AccessIsAllowed(appMainWindowView));
        }

        

        private void AppMainWindowView_OnLoadedEvent()
        {
            appMainWindowView.OpenChildWindow(authorizationView.GetForm());
        }
        

        private void AppMainWindowView_OnClosingEvent()
        {
            appMainWindowView.CloseChildWindow();
        }

        public void Run() {
            Application.Run(appMainWindowView.GetForm());
        }
    }
}
