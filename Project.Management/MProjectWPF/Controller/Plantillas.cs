using ControlDB.Model;
using MProjectWPF.UsersControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace MProjectWPF.Controller
{
    class Plantillas
    {
        MProjectDeskSQLITEEntities dbMP;
        string key;

        public Plantillas(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
            key = (from x in dbMP.mproject_key select x).Single().valor;    
        }

        public bool savetemplate(XmlNodeList lstNodeF, string nom, string desc,usuarios_meta_datos usu,string ky)
        {
            if (ky == "")
            {
                ky = key;
            }
            int lastIndexPlantilla;
            try { lastIndexPlantilla = (int)usu.plantillas.Last().id_plantilla + 1; }
            catch{ lastIndexPlantilla = 1; }

            plantillas pla = new plantillas();
            pla.keym = ky;
            pla.id_plantilla = lastIndexPlantilla;
            pla.usuarios_meta_datos = usu;
            pla.nombre = nom;
            pla.descripcion = desc;
            pla.fecha_ultima_modificacion = DateTime.Now;

            dbMP.plantillas.Add(pla);

            foreach (XmlNode nodeF in lstNodeF)
            {
                string descripcion = nodeF.Attributes["content"].Value;
                int tipo_dato = Convert.ToInt32(nodeF.Attributes["option"].Value);
                bool required = Convert.ToBoolean(nodeF.Attributes["isRequired"].Value);



                int lastIndexMetaDatos;
                try { lastIndexMetaDatos = (int)usu.meta_datos.Last().id_meta_datos + 1; }
                catch { lastIndexMetaDatos = 1; }

                meta_datos md = new meta_datos();
                md.keym = ky;
                md.id_meta_datos = lastIndexMetaDatos;
                md.usuarios_meta_datos = usu;
                md.descripcion = descripcion.Replace(":","");
                md.meta_dato_ir = true;
                md.tipos_datos = (from dato in dbMP.tipos_datos where dato.id_tipo_dato== tipo_dato select dato).Single();
                md.fecha_ultima_modificacion = DateTime.Now;
                
                int lastIndexPlantMetaDatos;
                try { lastIndexPlantMetaDatos = (int)usu.plantillas_meta_datos.Last().id_plantilla_meta_dato + 1; }
                catch { lastIndexPlantMetaDatos = 1; }

                plantillas_meta_datos pmd = new plantillas_meta_datos();
                pmd.keym = ky;
                pmd.plantillas = pla;
                pmd.meta_datos = md;
                pmd.id_plantilla_meta_dato = lastIndexPlantMetaDatos;
                pmd.id_plantilla = pla.keym +"-"+ pla.id_plantilla+"-"+ pla.id_usuario;
                pmd.id_meta_dato = md.keym +"-"+ md.id_meta_datos+"-"+md.id_usuario;
                pmd.usuarios_meta_datos = usu;
                pmd.requerido = required;
                pmd.fecha_ultima_modificacion = DateTime.Now;

                dbMP.meta_datos.Add(md);
                dbMP.plantillas_meta_datos.Add(pmd);                

                if (tipo_dato == 3)
                {   
                    foreach (XmlNode nodeI in nodeF.ChildNodes)
                    {
                        string descI = nodeI.Attributes["content"].Value;
                        
                        lastIndexMetaDatos = (int)usu.meta_datos.Last().id_meta_datos + 1;

                        meta_datos mdI = new meta_datos();
                        mdI.keym = ky;
                        mdI.id_meta_datos = lastIndexMetaDatos;
                        mdI.usuarios_meta_datos = usu;
                        mdI.descripcion = descI;
                        mdI.meta_dato_ir = true;
                        mdI.id_tipo_dato = 5;
                        mdI.fecha_ultima_modificacion = DateTime.Now;

                        lastIndexPlantMetaDatos = (int)usu.plantillas_meta_datos.Last().id_plantilla_meta_dato + 1;

                        plantillas_meta_datos pmdI = new plantillas_meta_datos();
                        pmdI.keym = ky;
                        pmdI.plantillas = pla;
                        pmdI.meta_datos = mdI;
                        pmdI.id_plantilla_meta_dato = lastIndexPlantMetaDatos;
                        pmdI.id_plantilla = pla.keym + "-" + pla.id_plantilla + "-" + pla.id_usuario;
                        pmdI.id_meta_dato = md.keym + "-" + md.id_meta_datos + "-" + md.id_usuario;
                        pmdI.usuarios_meta_datos = usu;
                        pmdI.requerido = required;
                        pmdI.fecha_ultima_modificacion = DateTime.Now;
                        
                        dbMP.meta_datos.Add(mdI);
                        dbMP.plantillas_meta_datos.Add(pmdI);                        
                    }
                }
            }
            return saveChanges();
        }

        public List<plantillas> listTemplate()
        {  
            return (from x in dbMP.plantillas orderby x.nombre select x).ToList();
        }
        
        public List<BoxField> listBoxField(plantillas pla,newProjectPanel npp)
        {
            List<BoxField> lbx = new List<BoxField>();
            BoxField bf = null;          
                          
            foreach (var pmd in pla.plantillas_meta_datos.OrderBy(a => a.id_meta_dato).ToList())
            {   
                string name = pmd.meta_datos.descripcion;                
                int opt = (int)pmd.meta_datos.id_tipo_dato;                

                if (opt != 5)
                {
                    bf = new BoxField(pmd,0);
                    npp.vTemplate.stackPanelFields.Children.Add(bf);

                    if (opt == 0)
                    {
                        npp.fieldTitle = bf;
                        npp.fieldTitle.boxField3.TextChanged += new System.Windows.Controls.TextChangedEventHandler(npp.titleBoxField_TextChanged);
                    }
                    lbx.Add(bf);                                          
                }
                else
                {
                    bf.comBoxField3.Items.Add(name);
                }                
            }
            return lbx;
        }

        public void listBoxField(plantillas pla, FieldTemplatePanel ftp)
        {          
            BoxField bf = null;

            foreach (var pmd in pla.plantillas_meta_datos.OrderBy(a => a.id_meta_dato).ToList())
            {
                string name = pmd.meta_datos.descripcion;
                int opt = (int)pmd.meta_datos.id_tipo_dato;

                if (opt != 5)
                {
                    bf = new BoxField(pmd);
                    ftp.listFields.Children.Add(bf);
                }
                else
                {
                    bf.comBoxField2.Items.Add(name);
                }
            }
        }

        public bool deleteTemplate(plantillas pla)
        {                      
            foreach (plantillas_meta_datos pmd in pla.plantillas_meta_datos.ToList())
            {
                dbMP.meta_datos.Remove(pmd.meta_datos);
                dbMP.plantillas_meta_datos.Remove(pmd);
            }
            dbMP.plantillas.Remove(pla);
            return saveChanges();
        }

        private bool saveChanges()
        {
            try
            {
                dbMP.SaveChanges();
                return true;              
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return false;
            }
        }
    }
}
