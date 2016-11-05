
using System.Windows;
using System.Windows.Input;
using ControlDB.Model;
using System.Diagnostics;
using System.Threading;
using MProjectWPF.MProjectWCF;
using MProjectWPF.Controller;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;
using System;

namespace MProjectWPF.UserControl.Inicio
{

    public partial class LoginWindow : Window
    {
        #region Variables del User Control
        MProjectDeskSQLITEEntities dbMP;
        MainWindow mainW;
        bool canClose = true;
        Thread th;
        #endregion

        public LoginWindow(MainWindow mainW)
        {
            InitializeComponent();
            dbMP = mainW.dbMP;
            this.mainW = mainW;
        }

        #region INICIAR SESION

        //ACCION DE INICIAR SESION
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            messageGrid.Visibility = Visibility.Collapsed;
            messageErrorGrid.Visibility = Visibility.Collapsed;

            //Valida los campos
            if (validateFields())
            {   

                //Verifica si usuario se encuentra en la base local
                if (mainW.user_Click(userTxt.Text,passTxt.Password))
                {
                    canClose = false;
                    mainW.Visibility = Visibility.Visible;
                    Close();
                }
                //Verifica si usuario se encuentra en la base remota
                else
                {
                    messageGrid.Visibility = Visibility.Visible;
                    message.Text = "Buscando Usuario Online...";
                    DownloadRepositorio();
                }
            }
        }
        
        private bool validateFields()
        {
            bool validate = true;

            if (userTxt.Text == "")
            {
                userTxtSh.Visibility = Visibility.Visible;
                userTxtSh.Content = "Debe colocar su Email";
                validate = false;
            }
            if (passTxt.Password == "")
            {
                passTxtSh.Visibility = Visibility.Visible;
                passTxtSh.Content = "Falta Contraseña";
                validate = false;
            }

            return validate;
        }

        #region Otros Click
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(mainW.user_Click("a@gmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }

        private void button_Click2(object sender, RoutedEventArgs e)
        {
            if (mainW.user_Click("b@gmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }

        private void button_Click3(object sender, RoutedEventArgs e)
        {
            if (mainW.user_Click("c@hotmail.com", "123"))
            {
                canClose = false;
                mainW.Visibility = Visibility.Visible;
                Close();
            }
        }
        #endregion
        #endregion

        #region DESCARGA EL REPOSITORIO DEL USUARIO
        private void DownloadRepositorio()
        {
            mainW.serviceClient = new MProjectServiceClient();

            string email = userTxt.Text;
            string password = passTxt.Password;

            #region Iniciar el Hilo
            th = new Thread(() =>
            {
                string[] folderList = null;
                string[] fileList = null;
                Stream fileStream = null;
                string repositorio = mainW.repositorio;

                #region Habilitar Interfaz de Descarga
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    btnLogin.IsEnabled = false;
                    downloadGrid.Visibility = Visibility.Visible;
                }));
                #endregion

                #region Verificamos si Usuario Existe en la base remota
                try
                {
                    fileList = mainW.serviceClient.getUser(email, password, out folderList, out fileStream);

                    addMaximumProgress(fileList.Length + 4);
                    addValueProgress();
                }
                catch/*(Exception err)*/
                {
                    addMaximumProgress(0);
                    //MessageBox.Show(err.ToString());
                }
                #endregion

                if (fileStream != null)
                {
                    #region Crear XML de Datos Usuario
                    messageText("Creando XML");
                    ServerController.CreateFile(fileStream, mainW.repositorio + email + ".xml");
                    addValueProgress();
                    #endregion

                    #region Crear estructura folders repositorio usuario
                    messageText("Crear folders repositorio");
                    foreach (string folder in folderList)
                    {
                        Directory.CreateDirectory(repositorio + folder);
                    }
                    addValueProgress();
                    #endregion

                    #region Obtener Archivos de lista Archivos                    
                    foreach (string file in fileList)
                    {
                        string filePath = file;
                        string[] namefile = file.Split('\\');
                        messageText("Descargando Archivo... \n =>" + namefile[namefile.Length - 1]);
                        try
                        {
                            fileStream = mainW.serviceClient.downloadFile(ref filePath);
                            ServerController.CreateFile(fileStream, mainW.repositorio + filePath);
                        }
                        catch (Exception err)
                        {
                            messageErrorText(file + "=> " + err.ToString());
                        }
                        addValueProgress();
                    }
                    #endregion

                    #region Almacenar Datos de Usuario 
                    messageText("Almacenar datos local");
                    LogXml conXml = new LogXml(mainW.repositorio + email + ".xml");
                    conXml.writeLogDownload(null, dbMP);
                    dbMP.SaveChanges();
                    addValueProgress();
                    #endregion

                    #region Fin de Descarga
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        message.Text = "Descarga Completa";
                        btnLogin.IsEnabled = true;

                        canClose = false;
                        mainW.user_Click(conXml.usuMod);
                        conXml = null;
                        mainW.Visibility = Visibility.Visible;
                        File.Delete(mainW.repositorio + email + ".xml");
                        mainW.serviceClient.Close();
                        Close();
                    }));
                    
                    #endregion
                }
                else
                {
                    #region No encontro resultado
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        messageGrid.Visibility = Visibility.Collapsed;
                        downloadGrid.Visibility = Visibility.Collapsed;
                        messageErrorGrid.Visibility = Visibility.Visible;
                        messageError.Text = "Email o Password Incorrectos";
                        btnLogin.IsEnabled = true;
                    }));
                    mainW.serviceClient.Close();
                    try { th.Abort(); } catch { }
                    #endregion
                }
            });
            th.Start();
            #endregion
        }
        #endregion

        #region Control de Advertencias de errores
        private void userTxtSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            userTxtSh.Visibility = Visibility.Collapsed;
            userTxt.Focus();
        }

        private void passTxtSh_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            passTxtSh.Visibility = Visibility.Collapsed;
            passTxt.Focus();
        }

        //Muestra Mensajes de Informacion
        private void messageText(string msg)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                messageGrid.Visibility = Visibility.Visible;
                message.Text = msg;
            }));
        }

        //Muestra Mensajes de error
        private void messageErrorText(string msg)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                messageErrorGrid.Visibility = Visibility.Visible;
                messageError.Text = msg;
            }));
        }
        #endregion
        
        #region Otros Eventos
        private void addMaximumProgress(int max)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                progressDownload.Maximum = max;
            }));
        }

        private void addValueProgress()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {   
                progressDownload.Value = progressDownload.Value + 1;
                int progressValue = Convert.ToInt32((progressDownload.Value * 100) / progressDownload.Maximum);
                progressLbl.Content =  progressValue+"%";
            }));
        }

        private void btn_register_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_forgotpassword_MouseEnter(object sender, MouseEventArgs e)
        {
            //btn_forgotpassword.TextDecorations = TextDecorations.Baseline;
        }

        private void btn_forgotpassword_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try { if(th!=null) th.Abort(); } catch { }
            if (canClose)
            {
                Process.GetCurrentProcess().Kill();
            }
        }
        #endregion
    }
}
