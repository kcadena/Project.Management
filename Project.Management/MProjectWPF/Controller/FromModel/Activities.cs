using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Model;
using System.Windows.Controls;

namespace MProjectWPF.Controller.FromModel
{
    class Activities
    {


        private MProjectDeskSQLITEEntities MPdb = new MProjectDeskSQLITEEntities();
        public List<long> getActivitiesFolders()
        {
            var fol = from x in MPdb.folders
                      join y in MPdb.actividades on x.id_folder equals y.id_actividad
                      where x.id_proyecto == 1
                      select x.id_folder;

            return fol.ToList<long>();

                //var zx = (MPdb.actividades.Join(
                //    MPdb.caracteristicas,
                //    actividades => actividades.id_actividad,
                //    caracteristicas => caracteristicas.id_actividad,
                //    (actividades, caracteristicas) => new { caracteristicas = caracteristicas, actividades = actividades }
                //    )
                //    .Where(q=>q.actividades.id_folder ==x.id_folder)
                //    .Select(
                //      w => new ActividadesList()
                //      {
                //          nombre = w.actividades.nombre,
                //          fol =(long) w.actividades.id_folder,
                //          pos = (long) w.actividades.pos,
                //          //par_car = (long)  w.caracteristicas.padre_caracteristica ?? 
                //      }));
                
            
        }

        public List<long> getAtivitiesParents(long pro)
        {
            var dat = (from x in MPdb.actividades
                      join y in MPdb.caracteristicas
                      on x.id_actividad equals y.id_actividad
                      where y.proyecto_padre == pro
                      select (y.padre_caracteristica)).Distinct();
            List<long> list = new List<long>(); 
            foreach(var x in dat)
            {
                //System.Windows.MessageBox.Show(""+x);
                list.Add((long)x);
            }
            return list;
        }

        public List<ActividadesList> getActivitiesProject(long proPar)
        {
             try {
                var dat = (from x in MPdb.caracteristicas
                           join y in MPdb.actividades
                           on x.id_actividad equals y.id_actividad
                           where x.proyecto_padre == proPar
                           orderby x.padre_caracteristica ascending, y.pos ascending
                           select (new ActividadesList()
                           {
                               nombre = y.nombre,
                               fol = y.id_folder,
                               pos = y.pos,
                               par_car = x.padre_caracteristica,
                               id_act = x.id_caracteristica
                           }
                           ));
                
                
                return dat.ToList<ActividadesList>();
            }
            catch (Exception err){
                System.Windows.MessageBox.Show(err.InnerException.ToString());
                return null;
            }
           
        }
        
        public bool createActivity(string nom, string des, int pos, int id_fol)
        {
            actividade act = new actividade();
            act.nombre = nom;
            act.descripcion = des;
            act.pos = pos;
            act.id_folder = id_fol;
            try
            {
                MPdb.actividades.Add(act);
                MPdb.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int getPositionAct(int id_fol)
        {
            try
            {
                //var pos = (from x in MPdb.actividades
                //           where x.id_folder == id_fol
                //           select x).Max(y => y.pos);

                var pos = (from x in MPdb.actividades
                           join y in MPdb.caracteristicas
                           on x.id_actividad equals y.id_actividad
                           where y.padre_caracteristica == id_fol
                           select x.pos
                         ).Max();

                return (int)pos;
            }
            catch {
                return 0;
            }


        }

    }

    public class ActividadesList
    {
        public string nombre { get; set; }
        public Nullable<long> fol { get; set; }
        public Nullable<long> pos { get; set; }
        public Nullable<long> par_car { get; set; }
        public Nullable<long> id_act { get; set; }
        //public Nullable<long> par_car
    }
}


