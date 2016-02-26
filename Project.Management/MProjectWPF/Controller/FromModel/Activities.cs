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
        public List<List<ActividadesList>> getActivitiesFolders()
        {
            List<List<ActividadesList>> act = new List<List<ActividadesList>>();
            var fol = from x in MPdb.folders
                      where x.id_proyecto == 1
                      select x;
            foreach (var x in fol)
            {
                var aux = from y in MPdb.actividades
                          join a in MPdb.caracteristicas
                          on y.id_actividad equals a.id_actividad
                          where y.id_folder == x.id_folder
                          orderby y.pos ascending
                          select (new ActividadesList
                          {
                              nombre = y.nombre,
                              fol = y.id_folder,
                              pos = y.pos,
                              par_car = a.padre_caracteristica,
                              id_act = a.id_caracteristica 
                          });

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
                try
                {
                    act.Add(aux.ToList<ActividadesList>());
                }
                catch (Exception err){ System.Windows.MessageBox.Show("Error \n"+err.ToString()); }
               
            }
            
            return act;
        }

        public List<List<ActividadesList>> getActivitiesProject(int proPar)
        {
            List<List<ActividadesList>> act = new List<List<ActividadesList>>();

            var dat = from x in MPdb.caracteristicas
                      join y in MPdb.actividades
                      on x.id_actividad equals y.id_actividad
                      //where x.proyecto_padre == proPar
                      orderby x.padre_caracteristica ascending, y.pos ascending
                      select (new ActividadesList()
                      {
                          nombre = y.nombre,
                          fol =y.id_folder,
                          pos =y.pos,
                          par_car = x.padre_caracteristica,
                          id_act = x.id_caracteristica
                      }
                      );
            try
            {
                act.Add(dat.ToList<ActividadesList>());
            }
            catch (Exception err) { System.Windows.MessageBox.Show("Error \n" + err.ToString()); }
            return act; 
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
        public long pos { get; set; }
        public Nullable<long> par_car { get; set; }
        public Nullable<long> id_act { get; set; }
    }
}


