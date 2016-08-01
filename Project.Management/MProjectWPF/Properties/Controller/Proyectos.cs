using ControlDB.Model;
using MProjectWPF.UsersControls.ProjectControls;
using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MProjectWPF.Controller
{
    class Proyectos
    {
        MProjectDeskSQLITEEntities dbMP;
        public proyectos proMod;
        string key;
        public Proyectos(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
            key = (from x in dbMP.mproject_key select x).Single().valor;
        }

        public List<BoxField> loadTemplate(proyectos pro,ProjectPanel propan)
        {
            List<BoxField> lbf = new List<BoxField>();
            BoxField bf;
            foreach(proyectos_meta_datos pmd in pro.proyectos_meta_datos.ToList())
            {
                bf = new BoxField(pmd);
                lbf.Add(bf);
                propan.templatePanel.Children.Add(bf);
            }
            return lbf;
        }

        public void loadTemplate(List<BoxField> lisBF, ProjectPanel propan)
        {   
            foreach (BoxField bf in lisBF)
            {
                try
                {
                    propan.templatePanel.Children.Add(bf);
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                }
               
            }
        }

        public bool saveProject(ProjectPanel ppan, ControlXml cxml)
        {
            usuarios_meta_datos usu = ppan.mainW.usuModel;
            string name = ppan.pName;
            string img = ppan.fileName;
            string desc = ppan.detail;
            List<BoxField> lbf = ppan.lisBF;
            List<BoxField> tlbf = ppan.tLisBF;


            long lastIdCaracteristica;
            try { lastIdCaracteristica = usu.caracteristicas.OrderBy(c => c.id_caracteristica).Last().id_caracteristica + 1; }
            catch { lastIdCaracteristica = 1; }

            caracteristicas car = new caracteristicas();
            car.keym = key;
            car.id_caracteristica = lastIdCaracteristica;
            car.usuarios_meta_datos = usu;
            car.estado = ppan.stageBox.Text;
            car.porcentaje_asignado = 100;
            car.porcentaje_cumplido = 0;
            car.fecha_inicio =  ppan.initialDate.SelectedDate;
            car.fecha_fin = ppan.finalDate.SelectedDate;
            car.recursos = ppan.txtResourses.Text;
            car.presupuesto = ppan.txtEstimation.Text;
            car.costos = ppan.txtCost.Text;
            car.tipo_caracteristica = "p";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;

            long lastidpro;
            try { lastidpro =usu.proyectos.OrderBy(p => p.id_proyecto).Last().id_proyecto + 1; }
            catch { lastidpro = 1; }

            proyectos pro = new proyectos();
            pro.keym = key;
            pro.caracteristicas = car;
            pro.id_proyecto = lastidpro;
            pro.id_caracteristica = car.id_caracteristica;
            pro.usuarios_meta_datos = usu;
            pro.nombre = name;
            pro.plantilla = "plantilla" + name.ToLower().Replace(" ", "") + ".xml";
            pro.IR_proyecto = false;
            pro.icon = img;
            pro.descripcion = desc;
            pro.fecha_ultima_modificacion = DateTime.Now;
            pro.contador = 0;
            dbMP.proyectos.Add(pro);

            cxml.createXmlforProject(name, desc);

            int con = 0;
            foreach (BoxField bf in lbf)
            {
                cxml.createXmlforProject(tlbf.ElementAt(con));

                long lastidpmd;
                try { lastidpmd = usu.proyectos_meta_datos.OrderBy(p => p.id_proyecto_meta_dato).Last().id_proyecto_meta_dato + 1; }
                catch { lastidpmd = 1; }

                proyectos_meta_datos pmd = new proyectos_meta_datos();
                pmd.keym = key;
                pmd.proyectos = pro;
                pmd.id_proyecto_meta_dato = lastidpmd;
                pmd.id_proyecto = pro.id_proyecto;
                pmd.tipo = bf.type.Replace(":", "");
                pmd.valor = bf.boxField2.Text;
                if (bf.opc == 0) pmd.is_title = true;
                else pmd.is_title = false;
                pmd.usuarios_meta_datos = usu;
                pmd.fecha_ultima_modificacion = DateTime.Now;
                
                dbMP.proyectos_meta_datos.Add(pmd);
                con++;
            }

            foreach (List<string> lres in ppan.lstRes)
            {
                long lastidres;
                try { lastidres =usu.recursos.OrderBy(r=> r.id_recurso).Last().id_recurso + 1; }
                catch { lastidres = 1; }

                recursos rec = new recursos();
                rec.keym = key;
                rec.caracteristicas = car;
                rec.id_recurso = lastidres;
                rec.id_caracteristica = car.id_caracteristica;
                rec.nombre_recurso = lres.ElementAt(0);
                rec.cantidad = Convert.ToInt64(lres.ElementAt(1));
                rec.usuarios_meta_datos = usu;

                dbMP.recursos.Add(rec);
            }

            foreach (List<string> lest in ppan.lstEst)
            {
                long lastidest;
                try { lastidest= usu.presupuesto.OrderBy(e => e.id_presupuesto).Last().id_presupuesto + 1; }
                catch { lastidest = 1; }

                presupuesto pre = new presupuesto();
                pre.keym = key;
                pre.caracteristicas = car;
                pre.id_presupuesto = lastidest;
                pre.id_caracteristica = car.id_caracteristica;
                pre.nombre = lest.ElementAt(0);
                pre.cantidad = Convert.ToInt64( lest.ElementAt(1));
                pre.valor = Convert.ToInt64(lest.ElementAt(2));
                pre.usuarios_meta_datos = usu;

                dbMP.presupuesto.Add(pre);
            }

            foreach (List<string> lcost in ppan.lstCos)
            {
                long lastidcost;
                try { lastidcost= usu.costos.OrderBy(c => c.id_costo).Last().id_costo+1; }
                catch { lastidcost = 1; }

                costos cos = new costos();
                cos.keym = key;
                cos.caracteristicas = car;
                cos.id_costo = lastidcost;
                cos.id_caracteristica = car.id_caracteristica;
                cos.nombre = lcost.ElementAt(0);
                cos.cantidad = Convert.ToInt64( lcost.ElementAt(1));
                cos.valor = Convert.ToInt64(lcost.ElementAt(2));
                cos.usuarios_meta_datos = usu;

                dbMP.costos.Add(cos);
            }

            dbMP.caracteristicas.Add(car);
            proMod = pro;
            ppan.proMod = pro;

            return saveChanges();
        }

        public bool updateProject(ProjectPanel ppan, ControlXml cxml)
        {
            usuarios_meta_datos usu = ppan.mainW.usuModel;
            string name = ppan.pName;
            string img = ppan.fileName;
            string desc = ppan.detail;
            proyectos pro = ppan.proMod;
            List<BoxField> lbf = ppan.lisBF;
            List<BoxField> tlbf = ppan.tLisBF;

            //remover tablas para actualizar
            foreach (proyectos_meta_datos pmd in pro.proyectos_meta_datos.ToList())
            {
                dbMP.proyectos_meta_datos.Remove(pmd);
            }
            foreach (recursos rec in pro.caracteristicas.recursoslist.ToList())
            {
                dbMP.recursos.Remove(rec);
            }
            foreach (presupuesto pre in pro.caracteristicas.presupuestolist.ToList())
            {
                dbMP.presupuesto.Remove(pre);
            }
            foreach (costos cos in pro.caracteristicas.costoslist.ToList())
            {
                dbMP.costos.Remove(cos);
            }

            caracteristicas car = pro.caracteristicas;
             
            car.estado = ppan.stageBox.Text;
            car.porcentaje_asignado = 100;
            car.porcentaje_cumplido = 0;
            car.fecha_inicio = ppan.initialDate.SelectedDate;
            car.fecha_fin = ppan.finalDate.SelectedDate;
            car.recursos = ppan.txtResourses.Text;
            car.presupuesto = ppan.txtEstimation.Text;
            car.costos = ppan.txtCost.Text;
            car.tipo_caracteristica = "p";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;           

            // proyectos Update
            pro.nombre = name;
            pro.plantilla = "plantilla" + name.ToLower().Replace(" ", "") + ".xml";
            pro.IR_proyecto = false;
            pro.icon = img;
            pro.descripcion = desc;
            pro.fecha_ultima_modificacion = DateTime.Now;            

            cxml.createXmlforProject(name, desc);           

            //Agregar nuevas tablas
            int con = 0;
            foreach (BoxField bf in lbf)
            {
                cxml.createXmlforProject(tlbf.ElementAt(con));

                long lastidpmd;
                try { lastidpmd = usu.proyectos_meta_datos.OrderBy(p => p.id_proyecto_meta_dato).Last().id_proyecto_meta_dato + 1; }
                catch { lastidpmd = 1; }

                proyectos_meta_datos pmd = new proyectos_meta_datos();
                pmd.keym = key;
                pmd.proyectos = pro;
                pmd.id_proyecto_meta_dato = lastidpmd;
                pmd.id_proyecto = pro.id_proyecto;
                pmd.tipo = bf.type.Replace(":", "");
                pmd.valor = bf.boxField2.Text;
                if (bf.opc == 0) pmd.is_title = true;
                else pmd.is_title = false;
                pmd.usuarios_meta_datos = usu;
                pmd.fecha_ultima_modificacion = DateTime.Now;

                dbMP.proyectos_meta_datos.Add(pmd);
                con++;
            }

            foreach (List<string> lres in ppan.lstRes)
            {
                long lastidres;
                try { lastidres = usu.recursos.OrderBy(r => r.id_recurso).Last().id_recurso + 1; }
                catch { lastidres = 1; }

                recursos rec = new recursos();
                rec.keym = key;
                rec.caracteristicas = car;
                rec.id_recurso = lastidres;
                rec.id_caracteristica = car.id_caracteristica;
                rec.nombre_recurso = lres.ElementAt(0);
                rec.cantidad = Convert.ToInt64(lres.ElementAt(1));
                rec.usuarios_meta_datos = usu;                

                dbMP.recursos.Add(rec);
            }

            foreach (List<string> lest in ppan.lstEst)
            {
                long lastidest;
                try { lastidest = usu.presupuesto.OrderBy(e => e.id_presupuesto).Last().id_presupuesto + 1; }
                catch { lastidest = 1; }

                presupuesto pre = new presupuesto();
                pre.keym = key;
                pre.caracteristicas = car;
                pre.id_presupuesto = lastidest;
                pre.id_caracteristica = car.id_caracteristica;
                pre.nombre = lest.ElementAt(0);
                pre.cantidad = Convert.ToInt64(lest.ElementAt(1));
                pre.valor = Convert.ToInt64(lest.ElementAt(2));
                pre.usuarios_meta_datos = usu;

                dbMP.presupuesto.Add(pre);
            }

            foreach (List<string> lcost in ppan.lstCos)
            {
                long lastidcost;
                try { lastidcost = usu.costos.OrderBy(c => c.id_costo).Last().id_costo + 1; }
                catch { lastidcost = 1; }

                costos cos = new costos();
                cos.keym = key;
                cos.caracteristicas = car;
                cos.id_costo = lastidcost;
                cos.id_caracteristica = car.id_caracteristica;
                cos.nombre = lcost.ElementAt(0);
                cos.cantidad = Convert.ToInt64(lcost.ElementAt(1));
                cos.valor = Convert.ToInt64(lcost.ElementAt(2));
                cos.usuarios_meta_datos = usu;

                dbMP.costos.Add(cos);
            }
                        
            proMod = pro;
            ppan.proMod = pro;
            return saveChanges();
        }

        public bool deleteProject(proyectos pro)
        {
            foreach (proyectos_meta_datos pmd in pro.proyectos_meta_datos.ToList())
                dbMP.proyectos_meta_datos.Remove(pmd);

            foreach (recursos rec in pro.caracteristicas.recursoslist.ToList())
                dbMP.recursos.Remove(rec);

            foreach (presupuesto est in pro.caracteristicas.presupuestolist.ToList())
                dbMP.presupuesto.Remove(est);

            foreach (costos cos in pro.caracteristicas.costoslist.ToList())
                dbMP.costos.Remove(cos);

            dbMP.caracteristicas.Remove(pro.caracteristicas);
            dbMP.proyectos.Remove(pro);            
            return saveChanges();
        }

        private bool saveChanges()
        {
            try
            {
                try
                {
                    dbMP.SaveChanges();
                }
                catch(Exception err)
                {

                    MessageBox.Show(err.InnerException.InnerException.Message);
                }
                return true;
            }
            catch(DbEntityValidationException e)
            {
                MessageBox.Show(e.Message);

                foreach (var eve in e.EntityValidationErrors)
                {
                    MessageBox.Show( eve.Entry.Entity.GetType().Name+" "+ eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MessageBox.Show(ve.PropertyName +" "+ ve.ErrorMessage);                        
                    }
                }
                return false;
            }
        }

    }
}
