using ControlDB.Model;
using MProjectWPF.UsersControls;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MProjectWPF.Controller
{
    public class Caracteristicas
    {
        MProjectDeskSQLITEEntities dbMP;
        MainWindow mainW;

        public Caracteristicas(MainWindow mw)
        {
            mainW = mw;
            dbMP = mainW.dbMP;
        }

        public Caracteristicas(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
        }
        
        public void getActivitiesCharacteristics(caracteristicas carac, TreeView tv, LabelTreeActivity lta2, ExplorerProject ep)
        {
            try
            {
                List < caracteristicas > lstcar = carac.caracteristicas1.OrderBy(c => c.actividades.First().pos).ToList();

                for (int i = 0; i < lstcar.Count; i++)
                {
                    caracteristicas car = lstcar.ElementAt(i);
                    if (car != null)
                    {
                        if (lta2 != null)
                        {
                            if (car.id_usuario == mainW.usuModel.id_usuario)
                            {
                                createLabelTreeActivity(car, tv, lta2, ep, i, lstcar.Count);
                            }
                            else if (car.id_usuario != mainW.usuModel.id_usuario && car.visualizar_superior == true)
                            {
                                createLabelTreeActivity(car, tv, lta2, ep, i, lstcar.Count);
                            }
                            //if (lta2.car.usuarios_meta_datos_asignado == null)
                            //{
                            //    createLabelTreeActivity(car, tv, lta2, ep, i, lstcar.Count);
                            //}
                            //else if (lta2.car.usuarios_meta_datos_asignado.id_usuario != mainW.usuModel.id_usuario && lta2.car.visualizar_superior == true)
                            //{   
                            //    createLabelTreeActivity(car, tv, lta2, ep, i, lstcar.Count);
                            //}
                        }
                        else
                        {
                            createLabelTreeActivity(car, tv, lta2, ep, i, lstcar.Count);
                        }
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        
        private void createLabelTreeActivity(caracteristicas car, TreeView tv, LabelTreeActivity lta2, ExplorerProject ep, int i, int max)
        {
            actividades act = car.actividades.First();
            LabelTreeActivity lta = new LabelTreeActivity(act, ep, lta2);            
            lta.show_arrow(lta, i, max);

            if (tv != null)
            {
                tv.Items.Add(lta);
            }
            else
            {
                lta.Margin = new Thickness(-38, 0, 0, 0);
                lta2.addchilds(lta);
            }
            getActivitiesCharacteristics(car, null, lta, ep);
        }

        //SERVER DOWNLOAD
        public caracteristicas getCaracteristicas(XmlNode nodeF)
        {
            string keym = nodeF.Attributes["keym"].Value;
            long id_caracteristica = Convert.ToInt64(nodeF.Attributes["id_caracteristica"].Value);
            long id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);

            caracteristicas car;
            bool isUpdate = true;

            try
            {
                car = (from c in dbMP.caracteristicas
                       where c.keym == keym && c.id_caracteristica == id_caracteristica && c.id_usuario == id_usuario
                       select c).Single();
            }
            catch
            {
                car = new caracteristicas();
                isUpdate = false;
            }

            car.keym = keym;
            car.id_caracteristica = id_caracteristica;
            car.id_usuario = id_usuario;
            car.id_caracteristica_padre = nodeF.Attributes["id_caracteristica_padre"].Value;
            try { car.idx_caracteristica_padre = Convert.ToInt64(nodeF.Attributes["idx_caracteristica_padre"].Value); } catch { }
            car.estado = nodeF.Attributes["estado"].Value;
            car.Porcentaje = Convert.ToInt64(nodeF.Attributes["porcentaje"].Value);
            car.porcentaje_asignado = Convert.ToInt64(nodeF.Attributes["porcentaje_asignado"].Value);
            car.porcentaje_cumplido = Convert.ToInt64(nodeF.Attributes["porcentaje_cumplido"].Value);
            try { car.fecha_inicio = Convert.ToDateTime(nodeF.Attributes["fecha_inicio"].Value); } catch { }
            try { car.fecha_fin = Convert.ToDateTime(nodeF.Attributes["fecha_fin"].Value); } catch { }
            car.recursos = nodeF.Attributes["recurso"].Value;
            car.recursos_restantes = nodeF.Attributes["recursos_restantes"].Value;
            car.presupuesto = nodeF.Attributes["presupuesto"].Value;
            car.costos = nodeF.Attributes["costo"].Value;
            car.tipo_caracteristica = nodeF.Attributes["tipo_caracteristica"].Value;
            car.visualizar_superior = Convert.ToBoolean(nodeF.Attributes["visualizar_superior"].Value);
            car.fecha_ultima_modificacion = Convert.ToDateTime(nodeF.Attributes["fecha_ultima_modificacion"].Value);
            try { car.usuario_asignado = Convert.ToInt64(nodeF.Attributes["usuario_asignado"].Value); } catch { }

            if (!isUpdate) dbMP.caracteristicas.Add(car);

            return car;
        }
    }
}
