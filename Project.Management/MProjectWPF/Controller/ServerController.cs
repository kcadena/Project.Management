using ControlDB.Model;
using MProjectWPF.MProjectWCF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MProjectWPF.Controller
{
    public static class ServerController
    {
        public static void addCaracteristica(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addCaracteristicas(u);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
            client.Close();
        }

        public static void addProyecto(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addProyectos(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void addProyectoMetaDato(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addProyectosMetaDatos(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void addRecursos(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addRecursos(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void addPresupuesto(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addPresupuesto(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void addCostos(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addCostos(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void updateCaracteristica(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.updateCaracteristicas(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void updateProyecto(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.updateProyectos(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void deleteProyecto(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.DeleteProject(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void addActividad(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.addActividades(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void updateActividad(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.updateActividades(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void deleteActividad(Dictionary<string, string> u)
        {
            MProjectServiceClient client = new MProjectServiceClient();
            try
            {
                client.DeleteActividad(u);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

            client.Close();
        }

        public static void UploadFile(string source, string route)
        {
            foreach (string val in Directory.EnumerateDirectories(source))
            {   
                UploadFile(val,route);
            }
            foreach (string val in Directory.EnumerateFiles(source))
            {
                MProjectServiceClient client;
                try
                {
                    source = val;
                    
                    FileStream filestream = File.OpenRead(source);
                    string[] var = source.Split('\\');
                    string[] var1 = route.Split('\\');

                    source = source.Replace(var[var.Length - 1],"").Replace(route,"");
                    source = var1[var1.Length - 1] + "\\" + source;
                    
                    client = new MProjectServiceClient();
                    RemoteFileInfo request = new RemoteFileInfo();


                    request.FileName = var[var.Length - 1];
                    request.route = source;
                    request.FileStream = filestream;
                    try
                    {
                        client.fileUpload(request.FileName, request.route, request.FileStream);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                    client.Close();
                }
                catch { }

            }
        }

        public static void DownloadFile(string routeFile)
        {
            try
            {
                MProjectServiceClient client = new MProjectServiceClient();
                DownloadRequest requestData = new DownloadRequest();
                RemoteFileInfo fileInfo = new RemoteFileInfo();
                requestData.usuario = routeFile;
                client.fileDownload(requestData.usuario,out fileInfo.route,out fileInfo.FileStream);
                string nameFile = routeFile.Split('\\').Last();
                string path = @"D:\RepositoriosMProject\" + routeFile.Replace(nameFile,"");

                Stream fileS = fileInfo.FileStream;
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (var file = File.Create(path + nameFile))
                    {
                        fileS.CopyTo(file);
                        fileS.Dispose();
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }

        public static void getProyect(string id_usu, MProjectDeskSQLITEEntities dbMP)
        {
            try
            {
                MProjectServiceClient client = new MProjectServiceClient();
                DownloadRequest requestData = new DownloadRequest();
                RemoteFileInfo fileInfo = new RemoteFileInfo();
                requestData.usuario = id_usu;

                client.getProyects(requestData.usuario,out fileInfo.route , out fileInfo.FileStream);
                client.deleteFile(id_usu);
                string path = @"D:\RepositoriosMProject\";
                Stream fileS = fileInfo.FileStream;
                string[] route = fileInfo.route.Split('_');

                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    using (var file = File.Create(path + route[0]))
                    {
                        fileS.CopyTo(file);
                        fileS.Dispose();
                    }

                    ///////////////////////////////////////////////
                    LogXml conXml = new LogXml(path + route[0]);
                    conXml.writeLogDownload(null, dbMP);
                    dbMP.SaveChanges();
                    File.Delete(path + route[0]);
                    int count = 0; 
                    foreach(string file in route)
                    {
                        if(count > 0)
                        {
                            DownloadFile(file);
                        }
                        count++;
                    } 
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }

        }
    }
}
