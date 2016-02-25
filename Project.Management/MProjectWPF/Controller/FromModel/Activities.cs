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
        public List<List<actividade>> getActivitiesFolders()
        {
            List<List<actividade>> act = new List<List<actividade>>();
            var fol = from x in MPdb.folders
                      where x.id_proyecto == 1
                      select x;
            foreach(var x in fol)
            {
                var aux = from y in MPdb.actividades
                          where y.id_folder == x.id_folder
                          orderby y.pos ascending
                          select y;
                act.Add(aux.ToList<actividade>());
            }

            return act;
        }
        public bool createActivity(string nom,string des,int pos,int id_fol)
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
                var pos = (from x in MPdb.actividades
                           where x.id_folder == id_fol
                           select x).Max(y => y.pos);
                return (int)pos;
            }
            catch {
                return 0;
            }
            
            
        }
    }
}
