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


        private static MProjectDeskSQLITEEntities MPdb = new MProjectDeskSQLITEEntities();
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
            foreach (var x in dat)
            {
                //System.Windows.MessageBox.Show(""+x);
                list.Add((long)x);
            }
            return list;
        }

        public List<ActividadesList> getActivitiesProject(long proPar)
        {
            try
            {
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
            catch (Exception err)
            {
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
            catch
            {
                return 0;
            }


        }
        public bool deleteActivity(long id)
        {
            actividade act = new actividade();
            try
            {
                act = MPdb.actividades.Find(id);
                MPdb.actividades.Remove(act);
                MPdb.SaveChanges();
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        public static ActividadesList activity_charac(long id)
        {
            var dat = (from x in MPdb.caracteristicas
                       join y in MPdb.actividades
                       on x.id_actividad equals y.id_actividad
                       where y.id_actividad == id
                       orderby x.padre_caracteristica ascending, y.pos ascending
                       select (new ActividadesList()
                       {
                           nombre = y.nombre,
                           fol = y.id_folder,
                           pos = y.pos,
                           par_car = x.padre_caracteristica,
                           id_act = x.id_caracteristica
                       }
                       )).First();

            return (ActividadesList)dat;
        }



        public bool changePoitionActivity(long parcar, int pos, int op)
        {
            //id => padre
            //pos=> posicion a modificar
            //op => tipo de modificacion (1-> arriba        2-> abajo)
            //pos_mod  posision de la actividad que se ve afectada por act_mod

            long pos_mod = pos ;
            actividade act1 = new actividade();
            actividade act2 = new actividade();
            
            if (op == 1)
                pos_mod = pos_mod - 1;
            else
                pos_mod = pos_mod + 1 ;

            try
            {
                List<actividade> act = (from x in MPdb.actividades
                                        join y in MPdb.caracteristicas
                                        on x.id_actividad equals y.id_actividad
                           where y.padre_caracteristica== parcar && (x.pos == pos || x.pos == pos_mod)
                           orderby x.pos ascending
                           select x).ToList<actividade>();
                long pax = (long)act.ElementAt(0).pos;
                act1 = act.ElementAt(0);
                act2 = act.ElementAt(1);
                act1.pos = act2.pos;

                if(op == 1)
                    act2.pos = pos_mod;
                else
                    act2.pos = pos;

                MPdb.actividades.Attach(act1);
                var entry1 = MPdb.Entry(act1);
                entry1.Property(e => e.pos).IsModified = true;

                MPdb.SaveChanges();

                MPdb.actividades.Attach(act2);
                var entry2 = MPdb.Entry(act2);
                entry2.Property(e => e.pos).IsModified = true;

                MPdb.SaveChanges();
                return true;

            }
            catch(Exception err)
            {
                System.Windows.MessageBox.Show(err.ToString());
                return false;
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


