using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Forms;
using FCC_Application.Views;

namespace FCC_Application
{
    public partial class AppMainWindow : Form, IAppMainWindowView
    {
        public AppMainWindow()
        {
            InitializeComponent();
        }

        public event Action OnLoadedEvent;
        public event Action OnClosingEvent;
        public event Action OnActivate;
        public event Action OnDeactivated;

        public Form currientChildForm;

        public void OpenChildWindow(Form childForm)
        {
            ChildFormSpace.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            ChildFormSpace.Controls.Add(childForm);
            ChildFormSpace.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            childForm.Focus();
            this.currientChildForm = childForm;
            UserService.Visible = true;
            //FileMenu.Visible = true;
            //WindowService.Visible = true;
        }

        private void AppMainWindow_Load(object sender, EventArgs e)
        {
            OnLoadedEvent?.Invoke();
            UserService.Visible = false;
        }

        private void AppMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnClosingEvent?.Invoke();
        }

        public Form GetForm()
        {
            return this;
        }

        public void CloseChildWindow()
        {
            currientChildForm?.Close();
        }

        private void AppMainWindow_Activated(object sender, EventArgs e)
        {
            OnActivate?.Invoke();
        }

        private void AppMainWindow_Deactivate(object sender, EventArgs e)
        {
            OnDeactivated?.Invoke();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            CloseChildWindow();
            
            OnLoadedEvent?.Invoke();
            UserService.Visible = false;
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

    }
}
