using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FCC_Application.Views;
using FCC_Application.Models;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace FCC_Application.Presenters
{
    public class EmbeddedPresenter : IPresenter
    {
        private IUnityView unityView;
        private IUserView userFormView;
        private IAppMainWindowView appMainWindowView;
        private TcpClient socketConnection;
        private Thread clientReceiveThread;
        private bool IsRunning = false;
        public EmbeddedPresenter(IAppMainWindowView appMainWindowView, IUserView userFormView, IUnityView unityView)
        {
            this.userFormView = userFormView;
            this.unityView = unityView;
            this.appMainWindowView = appMainWindowView;

            this.userFormView.OnDeleteObject += UserFormView_OnDeleteObject;
            this.userFormView.OnRelocateObject += UserFormView_OnRelocateObject;
            this.userFormView.HandInput += UserFormView_HandInput;
            this.userFormView.CaptureInput += UserFormView_CaptureInput;
            this.userFormView.OnClosingEvent += UserFormView_OnClosingEvent;
            this.userFormView.OnLoadedEvent += UserFormView_OnLoadedEvent;
            this.userFormView.OnResizeEvent += UserFormView_OnResizeEvent;
            this.userFormView.OnActivate += UserFormView_OnActivate;
            this.userFormView.OnDeactivated += UserFormView_OnDeactivated;
            this.userFormView.OnLoadObject += UserFormView_OnLoadObject; 
            this.appMainWindowView.OnActivate += UserFormView_OnActivate;
            this.appMainWindowView.OnDeactivated += UserFormView_OnDeactivated;
            this.userFormView.OnSelectObject += UserFormView_OnSelectObject;
        }

        private void UserFormView_OnRelocateObject(int index)
        {
            SendMessageToServer("r " + index);
        }

        private void UserFormView_OnDeleteObject(int index)
        {
            SendMessageToServer("d " + index);
        }

        private void UserFormView_CaptureInput()
        {
            SendMessageToServer("w");
        }

        private void UserFormView_HandInput()
        {

        }

        private void UserFormView_OnSelectObject(int index)
        {
            SendMessageToServer("s " + index);
        }

        private void UserFormView_OnLoadObject(string message)
        {
            SendMessageToServer("p " + message);
        }

        private void ListenForData(int port)
        {
           
                socketConnection = new TcpClient("localhost", port);
                byte[] bytes = new byte[1024];
                while (socketConnection.Connected)
                {
                    // Get a stream object for reading 				
                    using (NetworkStream stream = socketConnection.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary.
                        try
                        {
                            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                            {
                                var incommingData = new byte[length];
                                Array.Copy(bytes, 0, incommingData, 0, length);
                                // Convert byte array to string message. 						
                                string serverMessage = Encoding.ASCII.GetString(incommingData);

                                userFormView.ParseMessage(serverMessage);
                            }
                        }
                        catch { 
                        }
                    }
                }
            
        }

        public void SendMessageToServer(string message)
        {
            if (socketConnection == null || !IsRunning)
            {
                MessageBox.Show("Поключение к Unity не завершено. Повторите попытку через несколько секунд.");
                return;
            }
            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = socketConnection.GetStream();
                if (stream.CanWrite)
                {
                    // Convert string message to byte array.                 
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    //MessageBox.Show("Client sent his message - should be received by server");
                }
            }
            catch (SocketException socketException)
            {
                MessageBox.Show("Socket exception: " + socketException);
            }
        }

        private void UserFormView_OnDeactivated()
        {
            unityView.Deactivate();
        }

        private void UserFormView_OnActivate()
        {
            unityView.Activate();
        }

        private void UserFormView_OnResizeEvent(int width, int height)
        {
            unityView.SetSize(width, height);
        }

        private void UserFormView_OnLoadedEvent(string scene,int port)
        {
            unityView.Open(scene, userFormView.GetUnityParentHandle(), (success) =>
             {
                 if (!success)
                 {
                     userFormView.ShowEmbeddingFalliedMessage(unityView.GetFileName());
                 }
                 else
                 {
                     Thread.Sleep(3000);
                     while (!IsRunning)
                    {
                        try
                        {
                           clientReceiveThread = new Thread(() => ListenForData(port));
                           clientReceiveThread.IsBackground = true;
                           clientReceiveThread.Start();
                           IsRunning = true;
                        }
                        catch
                        {
                            IsRunning = false;
                        }
                        Thread.Sleep(1000);
                    }
                 }
             });
        }

     private void UserFormView_OnClosingEvent()
        {
            socketConnection?.Close();
            userFormView.OnClosingEvent -= UserFormView_OnClosingEvent;
            userFormView.OnLoadedEvent -= UserFormView_OnLoadedEvent;
            userFormView.OnResizeEvent -= UserFormView_OnResizeEvent;
            unityView?.Close();
        }
    }
}
