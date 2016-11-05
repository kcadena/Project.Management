using System;
using System.Windows;
using ControlDB.ChatService;

namespace MProjectWPF.UsersControls.ChatControls
{
    /// <summary>
    /// Lógica de interacción para ChatPanel.xaml
    /// </summary>
    public partial class ChatPanel : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        private User usu;
        private string id_usuarioDestination;
        private string nameDestination;
        string nameOrigin;
        private string[] msg;

        public ChatPanel(MainWindow mainW, User usu)
        {
            InitializeComponent();
            this.mainW = mainW;
            this.usu = usu;
            id_usuarioDestination = usu.UsuDic["id_usuario"];
            nameDestination = usu.UsuDic["nombre"];
            userlbl.Content = usu.UsuDic["nombre"];
            nameOrigin = mainW.usuMod.nombre + " " + mainW.usuMod.apellido;      
        }

        public ChatPanel(MainWindow mainW, string[] msg)
        {
            InitializeComponent();
            this.mainW = mainW;
            this.msg = msg;
            id_usuarioDestination = msg[0];
            nameDestination = msg[1];
            userlbl.Content = msg[1];
            nameOrigin = mainW.usuMod.nombre + " " + mainW.usuMod.apellido;
            addMessage(msg[2]);
        }

        public void addMessage(string msg)
        {
            lstMessages.Children.Add(new LabelMessage(false, msg, nameDestination + ": " + DateTime.Now.ToString()));
        }

        private void closeChatPanel_Click(object sender, RoutedEventArgs e)
        {
            mainW.chatList.Children.Remove(this);
            mainW.listOpenChat.Remove(id_usuarioDestination);
            mainW = null;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lstMessages.Children.Add(new LabelMessage(true, txtBox.Text, nameOrigin + ": " + DateTime.Now.ToString()));
            string[] msg = { mainW.usuMod.id_usuario+"", nameOrigin, txtBox.Text };
            mainW.chat.Messsage(id_usuarioDestination, msg);
            txtBox.Text = "";
        }
    }
}
