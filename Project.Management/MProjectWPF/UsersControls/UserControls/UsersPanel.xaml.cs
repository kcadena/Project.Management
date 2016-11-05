using System.Collections.Generic;
using System.Windows.Controls;
using ControlDB.Model;
using ControlDB.ChatServiceDuplex;
using System.Windows.Threading;
using System;
using ControlDB.ChatService;
using System.Windows;
using MProjectWPF.UsersControls.ChatControls;

namespace MProjectWPF.UsersControls.UserControls
{
    /// <summary>
    /// Lógica de interacción para UsersPanel.xaml
    /// </summary>
    public partial class UsersPanel : System.Windows.Controls.UserControl
    {
        MainWindow mainW;
        LabelUser lu;
        LabelUser actlu;
        Dictionary<string,LabelUser> ids;
        string id;

        public UsersPanel(MainWindow mw)
        {
            InitializeComponent();
            mainW = mw;
            ids = new Dictionary<string, LabelUser>();
        }

        public void loadActUser(usuarios_meta_datos usuMod)
        {
            id = "" + usuMod.id_usuario;
            actlu = new LabelUser(usuMod);
            actUser.Children.Add(actlu);
        }

        public void loadUsers(List<ControlDB.ChatServiceDuplex.User> users)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                lstUser.Items.Clear();
                int con = 0;
                foreach (ControlDB.ChatServiceDuplex.User usua in users)
                {
                    if (usua.UsuDic["id_usuario"] != id)
                    {
                        lstUser.Items.Add(new LabelUser(mainW, usua));
                        con++;
                    }
                    if (con >= 10) break;
                }
            }));
        }

        public void loadUsers(Dictionary<string, ControlDB.ChatService.User> dictionary)
        {
            int con = 0;

            foreach (KeyValuePair<string, ControlDB.ChatService.User> usu in dictionary)
            {

                if(usu.Key == id && usu.Value.Messages.Length > 0)
                {
                    foreach (string[] msg in usu.Value.Messages)
                    {
                        ChatPanel chatPan;
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            if (mainW.listOpenChat.ContainsKey(msg[0]))
                            {
                                chatPan = mainW.listOpenChat[msg[0]];
                                chatPan.addMessage(msg[2]);
                            }
                            else
                            {
                                chatPan = new ChatPanel(mainW, msg);
                                mainW.chatList.Children.Add(chatPan);
                                mainW.listOpenChat.Add(msg[0],chatPan);
                            }
                        }));

                    }
                    mainW.chat.clearMessage(usu.Key);
                }

                if (usu.Key != id)
                {
                    if (!ids.ContainsKey(usu.Key))
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            LabelUser labU = new LabelUser(mainW, usu.Value);
                            lstUser.Items.Add(labU);
                            ids.Add(usu.Key, labU);
                        }));
                    }
                    con++;
                }
                if (con >= 10) break;
                verifyUsers(dictionary);
            }
        }

        public void deleteUsers()
        {
            lstUser.Items.Clear();
        }

        private void lstUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lu != null) lu.hideButtons();

            mainW.spViewA.Children.Clear();
            lu = (LabelUser)lstUser.SelectedItem;
            if (lu != null)
            {
                lu.showButtons();
                //mainWin.spViewA.Children.Add(new UserDetails(lu.user));
                mainW.spViewA.Children.Add(new UserDetails(lu.usu));
            }
        }

        private void actUser_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lu != null) lu.hideButtons();
            lstUser.SelectedIndex = -1;
            mainW.spViewA.Children.Clear();
            lu = actlu;
            mainW.spViewA.Children.Add(new UserDetails(lu.usuMod));
        }
        
        private void verifyUsers(Dictionary<string, ControlDB.ChatService.User> dictionary)
        {
            ////try
            ////{
                foreach (KeyValuePair<string, LabelUser> llu in ids)
                {
                    if (!dictionary.ContainsKey(llu.Key))
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            lstUser.Items.Remove(llu.Value);
                        }));
                        //ids.Remove(llu.Key);
                    }
                }
            //}
            //catch { }

        }
    }
}
