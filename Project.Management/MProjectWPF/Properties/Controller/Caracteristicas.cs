using ControlDB.Model;
//using ControlDB.Model;
using MProjectWPF.UsersControls;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public caracteristicas createCharacteristics(actividades act,caracteristicas car)
        {
            //caracteristicas ncar = new caracteristicas();
            //ncar.id_usuario = car.id_usuario;
            //ncar.actividades = act;
            //ncar.proyecto_padre = car.proyecto_padre;
            //ncar.padre_caracteristica = car.id_caracteristica;

            //dbMP.actividades.Add(act);
            //dbMP.caracteristicas.Add(ncar);
            //dbMP.SaveChanges();

            return null; 
        }
        
        private void createLabelTreeActivity (caracteristicas car, TreeView tv, LabelTreeActivity lta2, ExplorerProject ep,bool isShowArrow)
        {
            actividades act = car.actividades.First();
            LabelTreeActivity lta = new LabelTreeActivity(act,ep,lta2);
            if (tv != null)
            {
                tv.Items.Add(lta);
                ep.child.Add(lta);
                if(isShowArrow) lta.show_arrow(ep.child);
            }
            else
            {
                lta.Margin = new Thickness(-38, 0, 0, 0);                
                lta2.child.Add(lta);
                if(isShowArrow) lta.show_arrow(lta2.child);
                lta2.tvi.Items.Add(lta);
            }
            getActivitiesCharacteristics(car, null, lta, ep);
        }

        public void getActivitiesCharacteristics(caracteristicas carac,TreeView tv,LabelTreeActivity lta2, ExplorerProject ep)
        {
            List<caracteristicas> lstcar = carac.caracteristicas1.OrderBy(c => c.actividades.First().pos).ToList();            
            int con = 0;
            bool sa = false;

            foreach (caracteristicas car in lstcar)
            {
                con++;
                if (con == lstcar.Count()) sa = true;

                if (car != null)
                {
                    if (lta2 != null)
                    {
                        if (lta2.car.usuarios_meta_datos_asignado == null)
                        {   
                            createLabelTreeActivity(car, tv, lta2, ep,sa);
                        }
                        else if (lta2.car.usuarios_meta_datos_asignado.id_usuario != mainW.usuModel.id_usuario && lta2.car.visualizar_superior == true)
                        {
                            createLabelTreeActivity(car, tv, lta2, ep,sa);
                        }                       
                    }
                    else
                    {
                        createLabelTreeActivity(car,tv, lta2, ep,sa);
                    }                                       
                }                                
            }
            
        }

        public void removeCharacteristics(caracteristicas car)
        {
            //dbMP.actividades.Remove();
            //dbMP.caracteristicas.Remove(car);
            //dbMP.SaveChanges();
        }
    }
}
