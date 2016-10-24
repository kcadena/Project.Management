using ControlDB.ChatService;
using System.Linq;
using System.Windows;

namespace MProjectWPF.Controller
{
    class ReceiveChat : ISendChatServiceCallback
    {
        public MainWindow mainW;

        public void CloseApp()
        {   
            mainW.user = null;
            MessageBox.Show("Sesión Online Cerrada: Usuario Inicio session en otra aplicacion");
        }

        public void SendNames(User[] users)
        {
            mainW.usuPan.loadUsers(users.ToList());  
        }
    }
}
