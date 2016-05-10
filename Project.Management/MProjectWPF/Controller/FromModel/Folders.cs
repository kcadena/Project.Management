using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Model;

using System.Windows.Controls;

namespace MProjectWPF.Controller.FromModel
{
    class Folders
    {
        private MProjectDeskSQLITEEntities mp = new MProjectDeskSQLITEEntities();

        //crear carpetas
    
        public long createFolder(String name, long project, long father)
        {
            folder fol = new folder();
            fol.id_proyecto = project;
            fol.nombre = name;
            fol.Parent_id_folder = father;
            mp.folders.Add(fol);
            long d = (from x in mp.folders
                      orderby x.id_folder ascending
                      select x.id_folder).Max();
            d = d + 1;
            fol.id_folder = d;
            try
            {
                mp.SaveChanges();
                return d;
            }
            catch (System.Data.ConstraintException err)
            {
                System.Windows.MessageBox.Show(err.InnerException.ToString());
                return -1;
            }
            
        }
        //borrar carpetas
        public bool deleteFolder(long id_fol)
        {
            folder fol = new folder();
            try
            {
                fol = mp.folders.Find(id_fol);
                mp.folders.Remove(fol);
                mp.SaveChanges();
                return true;
            }
            catch (System.Data.ConstraintException err)
            {
                return false;
            }
            
        }
        public bool renameFolder(long id_fol, string name)
        {
            try
            {
                var dat = (from x in mp.folders
                           where x.id_folder == id_fol
                           select x).First();

                folder fol = (folder)dat;
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
        public List<folder> getStructureFolders(long pro)
        {
            var fol = from x in mp.folders
                      where x.id_proyecto == pro
                      orderby x.Parent_id_folder ascending, x.id_folder ascending,x.nombre ascending
                      select x;

            return fol.ToList<folder>();
            
        }

    }
}
