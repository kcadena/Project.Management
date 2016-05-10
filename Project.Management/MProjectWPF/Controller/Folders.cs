using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Model;

using System.Windows.Controls;

namespace MProjectWPF.Controller
{
    class Folders
    {
        private MProjectDeskEntities mp = new MProjectDeskEntities();

        //crear carpetas
        public string createFolder(String name, int project)
        {
            folders fol = new folders();
            fol.id_proyecto = project;
            fol.nombre = name;
            fol.id_proyecto = mp.folders.Last<folders>().id_proyecto + 1;
            mp.folders.Add(fol);
            try
            {
                mp.SaveChanges();
            }
            catch (System.Data.ConstraintException err)
            {
                return err.InnerException.ToString();
            }
            return "ok";
        }
        public string createFolder(String name, int project, int father)
        {
            folders fol = new folders();
            fol.id_proyecto = project;
            fol.nombre = name;
            fol.id_proyecto = mp.folders.Last<folders>().id_proyecto + 1;
            mp.folders.Add(fol);
            try
            {
                mp.SaveChanges();
            }
            catch (System.Data.ConstraintException err)
            {
                return err.InnerException.ToString();
            }
            return "ok";
        }
        //borrar carpetas
        public string deleteFolder(int id_fol)
        {
            folders fol = new folders();
            fol.id_folder = id_fol;
            fol = mp.folders.Find(fol);
            mp.folders.Remove(fol);
            try
            {
                mp.SaveChanges();
            }
            catch (System.Data.ConstraintException err)
            {
                return err.InnerException.ToString();
            }
            return "ok";
        }
        private bool renameFolder(int id_fol, string name)
        {
            try
            {
                var dat = (from x in mp.folders
                           where x.id_folder == id_fol
                           select x).First();

                folders fol = (folders)dat;
                fol.nombre = name;
                mp.folders.Attach(fol);
                var entry = mp.Entry(fol);
                entry.Property(e => e.nombre).IsModified = true;
                // other changed properties
                mp.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<folders> getStructureFolders()
        {
            var fol = from x in mp.folders
                      where x.id_proyecto == 1
                      orderby x.Parent_id_folder ascending, x.id_folder ascending
                      select x;

            return fol.ToList<folders>();


        }

    }
}
