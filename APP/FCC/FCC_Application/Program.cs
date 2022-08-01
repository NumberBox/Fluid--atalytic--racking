using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Presenters;
using FCC_Application.Views;
using FCC_Application.Forms;
namespace FCC_Application
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var appMainPresenter = new AppMainPresenter(new AppMainWindow(), new AuthorizationForm());
            appMainPresenter.Run();
        }
    }
}
