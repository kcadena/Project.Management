using ControlDB.ChatServiceDuplex;
using System.Linq;
using System.Windows;
using mc = MProjectChat;
using System;
using System.Windows.Threading;

namespace MProjectWPF.Controller
{
    class ReceiveChat : ISendChatServiceCallback
    {
        public MainWindow mainW;

        public void CloseApp()
        {   
            mainW.userDuplex = null;
            MessageBox.Show("Sesión Online Cerrada: Usuario Inicio session en otra aplicacion");
        }

        public void ReceiveMessage(Message msg)
        {
            string id_usuario = msg.Sender.UsuDic["id_usuario"];

            mainW.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                if (mainW.lstWinChat.ContainsKey(id_usuario))
                {
                    mc.MainWindow mainChat = mainW.lstWinChat[id_usuario];
                    mainChat.Show();
                    mainChat.addMessage(msg.Content);
                }
                else
                {
                    mc.MainWindow mainChat = new mc.MainWindow(mainW.chatDuplex, msg.Receiver, msg.Sender,mainW.lstWinChat);
                    mainChat.Show();
                    mainW.lstWinChat.Add(msg.Sender.UsuDic["id_usuario"], mainChat);
                    mainChat.addMessage(msg.Content);
                }
            }));
             
        }

        public void SendNames(User[] users)
        {
            mainW.usuPan.loadUsers(users.ToList());  
        }
    }
}
