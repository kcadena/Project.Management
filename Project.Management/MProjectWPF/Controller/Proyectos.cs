using ControlDB.Model;
using MProjectWPF.MProjectWCF;
using MProjectWPF.UsersControls.ProjectControls;
using MProjectWPF.UsersControls.TemplatesControls;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

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
            string img = ppan.iconName;
            string desc = ppan.detail;
            List<BoxField> lbf = ppan.lisBF;
            List<BoxField> tlbf = ppan.tLisBF;


            usu.table_sequence.caracteristicas = usu.table_sequence.caracteristicas + 1;
            long lastIdCaracteristica = (long)usu.table_sequence.caracteristicas;

            caracteristicas car = new caracteristicas();
            car.keym = key;
            car.id_caracteristica = lastIdCaracteristica;
            car.usuarios_meta_datos = usu;
            car.estado = ppan.stageBox.Text;
            car.Porcentaje = 100;
            car.porcentaje_asignado = 0;
            car.porcentaje_cumplido = 0;
            car.fecha_inicio =  ppan.initialDate.SelectedDate;
            car.fecha_fin = ppan.finalDate.SelectedDate;
            car.recursos = ppan.txtResourses.Text;
            car.recursos_restantes = ppan.txtResourses.Text;
            car.presupuesto = ppan.txtEstimation.Text;
            car.costos = ppan.txtCost.Text;
            car.tipo_caracteristica = "p";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;

            usu.table_sequence.proyectos = usu.table_sequence.proyectos + 1;
            long lastidpro = (long)usu.table_sequence.proyectos;

            proyectos pro = new proyectos();
            pro.keym = key;
            pro.caracteristicas = car;
            pro.id_proyecto = lastidpro;
            pro.id_caracteristica =car.keym + "-" + car.id_caracteristica + "-" + usu.id_usuario;
            pro.usuarios_meta_datos = usu;
            pro.nombre = name;
            pro.plantilla = "plantilla" + pro.keym + "-" + pro.id_proyecto + "-" + usu.id_usuario + ".xml";
            pro.IR_proyecto = false;
            pro.icon = img;
            pro.descripcion = desc;
            pro.fecha_ultima_modificacion = DateTime.Now;
            pro.contador = 0;
            dbMP.proyectos.Add(pro);

            cxml.createXmlforProject("plantilla" + pro.keym + "-" + pro.id_proyecto + "-" + pro.id_usuario + ".xml", desc);

            int con = 0;
            foreach (BoxField bf in lbf)
            {
                cxml.createXmlforProject(tlbf.ElementAt(con));

                usu.table_sequence.proyectos_meta_datos = usu.table_sequence.proyectos_meta_datos + 1;
                long lastidpmd = (long)usu.table_sequence.proyectos_meta_datos;
                
                proyectos_meta_datos pmd = new proyectos_meta_datos();
                pmd.keym = key;
                pmd.proyectos = pro;
                pmd.id_proyecto_meta_dato = lastidpmd;
                pmd.id_proyecto = pro.keym +"-" + pro.id_proyecto + "-" + pro.id_usuario;
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
                rec.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
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
                pre.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
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
                cos.id_caracteristica = car.keym +"-"+ car.id_caracteristica + "-" + car.id_usuario;
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
            string img = ppan.iconName;
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
            car.porcentaje_asignado = 0;
            car.porcentaje_cumplido = 0;
            car.fecha_inicio = ppan.initialDate.SelectedDate;
            car.fecha_fin = ppan.finalDate.SelectedDate;
            car.recursos = ppan.txtResourses.Text;
            car.recursos_restantes = ppan.txtResourses.Text;
            car.presupuesto = ppan.txtEstimation.Text;
            car.costos = ppan.txtCost.Text;
            car.tipo_caracteristica = "p";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;           

            // proyectos Update
            pro.nombre = name;
            pro.plantilla = "plantilla" + ppan.idName + ".xml";
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
                pmd.id_proyecto = pro.keym + "-" + pro.id_proyecto + "-" +pro.id_usuario;
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
                rec.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
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
                pre.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
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
                cos.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
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

        public bool deleteProject(proyectos proMod)
        {
            Actividades actCon = new Actividades(dbMP);
            foreach (caracteristicas car in proMod.caracteristicas.caracteristicas1.ToList())
            {
                actCon.removeActivity(car.actividades.First());
            }

            foreach (archivos arc in proMod.caracteristicas.archivos.ToList())
                dbMP.archivos.Remove(arc);

            foreach (proyectos_meta_datos pmd in proMod.proyectos_meta_datos.ToList())
                dbMP.proyectos_meta_datos.Remove(pmd);

            foreach (recursos rec in proMod.caracteristicas.recursoslist.ToList())
                dbMP.recursos.Remove(rec);

            foreach (presupuesto est in proMod.caracteristicas.presupuestolist.ToList())
                dbMP.presupuesto.Remove(est);

            foreach (costos cos in proMod.caracteristicas.costoslist.ToList())
                dbMP.costos.Remove(cos);

            dbMP.caracteristicas.Remove(proMod.caracteristicas);
            dbMP.proyectos.Remove(proMod);            
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
                    try
                    {
                        MessageBox.Show(err.InnerException.InnerException.Message);
                    }
                    catch
                    {
                        MessageBox.Show(err.Message);
                    }
                    
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

        // OPCIONES SERVIDOR

        public void saveProjectServ(string caracteristica,bool opt)
        {
            string[] id_car = caracteristica.Split('-');

            string keym = id_car[0];
            long id_caracteristica = Convert.ToInt64(id_car[1]);
            long id_usuario = Convert.ToInt64(id_car[2]);

            caracteristicas car = (from c in dbMP.caracteristicas
                                   where c.keym == keym && c.id_caracteristica == id_caracteristica && c.id_usuario == id_usuario
                                   select c).First();


            Dictionary<string, string> u = new Dictionary<string, string>();
            u.Add("keym", car.keym);
            u.Add("id_caracteristica", car.id_caracteristica.ToString());
            u.Add("id_usuario", "" + car.id_usuario);
            u.Add("id_caracteristica_padre", car.id_caracteristica_padre);
            u.Add("estado", car.estado);
            u.Add("porcentaje", "" + car.Porcentaje);
            u.Add("porcentaje_asignado", "" + car.porcentaje_asignado);
            u.Add("porcentaje_cumplido", "" + car.porcentaje_cumplido);
            u.Add("fecha_inicio", "" + car.fecha_inicio);
            u.Add("fecha_fin", "" + car.fecha_fin);
            u.Add("recursos", "" + car.recursos);
            u.Add("recursos_restantes", "" + car.recursos_restantes);
            u.Add("presupuesto", "" + car.presupuesto);
            u.Add("costos", "" + car.costos);
            u.Add("tipo_caracteristica", "" + car.tipo_caracteristica);
            u.Add("fecha_ultima_modificacion", "" + car.fecha_ultima_modificacion);
            u.Add("car.usuario_asignado", "" + car.usuario_asignado);

            if (opt)  ServerController.addCaracteristica(u);
            else      ServerController.updateCaracteristica(u);
            

            u = new Dictionary<string, string>();
            proyectos pro = car.proyectos.First();
            u.Add("keym_pro", "" + pro.keym);
            u.Add("id_proyecto_pro", "" + pro.id_proyecto);
            u.Add("id_usuario_pro", "" + pro.id_usuario);
            u.Add("id_caracteristica_pro", "" + pro.id_caracteristica);
            u.Add("pro.nombre_pro", "" + pro.nombre);
            u.Add("plantilla_pro", "" + pro.plantilla);
            u.Add("IR_proyecto_pro", "" + pro.IR_proyecto);
            u.Add("icon_pro", "" + pro.icon);
            u.Add("descripcion_pro", "" + pro.descripcion);
            u.Add("fecha_ultima_modificacion_pro", "" + pro.fecha_ultima_modificacion);
            u.Add("contador_pro", "" + pro.contador);            
            if(opt) ServerController.addProyecto(u);
            else ServerController.updateProyecto(u);

            string userRoute = pro.usuarios_meta_datos.repositorios_usuarios.ruta_repositorio_local;
            string projectRoute = "\\proyectos\\" + pro.keym + "-" + pro.id_proyecto + "-" + pro.id_usuario;
            string source = userRoute + projectRoute;
            ServerController.UploadFile(source,userRoute);
            

            foreach (proyectos_meta_datos pmd in pro.proyectos_meta_datos.ToList())
            {
                u = new Dictionary<string, string>();
                u.Add("keym_pmd", pmd.keym);
                u.Add("id_proyecto_meta_dato_pmd", "" + pmd.id_proyecto_meta_dato);
                u.Add("id_usuario_pmd", "" + pmd.id_usuario);
                u.Add("id_proyecto_pmd", "" + pmd.id_proyecto);
                u.Add("valor_pmd", "" + pmd.valor);
                u.Add("is_title_pmd", "" + pmd.is_title);
                u.Add("tipo_pmd", "" + pmd.tipo);
                u.Add("fecha_ultima_modificacion_pmd", "" + pmd.fecha_ultima_modificacion);
                ServerController.addProyectoMetaDato(u);
            }

            foreach (recursos rec in pro.caracteristicas.recursoslist.ToList())
            {
                u = new Dictionary<string, string>();
                u.Add("keym", "" + rec.keym);
                u.Add("id_recurso", "" + rec.id_recurso);
                u.Add("id_usuario", "" + rec.id_usuario);
                u.Add("id_caracteristica", "" + rec.id_caracteristica);
                u.Add("nombre_recurso", "" + rec.nombre_recurso);
                u.Add("cantidad", "" + rec.cantidad);
                ServerController.addRecursos(u);
            }

            foreach (presupuesto pre in pro.caracteristicas.presupuestolist.ToList())
            {
                u = new Dictionary<string, string>();
                u.Add("keym", "" + pre.keym);
                u.Add("id_presupuesto", "" + pre.id_presupuesto);
                u.Add("id_usuario", "" + pre.id_usuario);
                u.Add("id_caracteristica", "" + pre.id_caracteristica);
                u.Add("nombre", "" + pre.nombre);
                u.Add("cantidad", "" + pre.cantidad);
                u.Add("valor", "" + pre.valor);
                ServerController.addPresupuesto(u);

            }

            foreach (costos cos in pro.caracteristicas.costoslist.ToList())
            {
                u = new Dictionary<string, string>();
                u.Add("keym", "" + cos.keym);
                u.Add("id_costo", "" + cos.id_costo);
                u.Add("id_usuario", "" + cos.id_usuario);
                u.Add("id_caracteristica", "" + cos.id_caracteristica);
                u.Add("nombre", "" + cos.nombre);
                u.Add("cantidad", "" + cos.cantidad);
                u.Add("valor", "" + cos.valor);
                ServerController.addCostos(u);
            }
        }

        public void deleteProjectServ(string caracteristica)
        {
            string[] id_car = caracteristica.Split('-');

            string keym = id_car[0];
            long id_caracteristica = Convert.ToInt64(id_car[1]);
            long id_usuario = Convert.ToInt64(id_car[2]);

            Dictionary<string, string> u = new Dictionary<string, string>();
            u.Add("keym", keym);
            u.Add("id_caracteristica","" + id_caracteristica.ToString());
            u.Add("id_usuario", "" + "" + id_usuario);
            ServerController.deleteProyecto(u);
        }

        public proyectos getProject(XmlNode nodeF, caracteristicas carMod)
        {
            proyectos proMod;
            bool isUpdate = true;

            try
            {
                proMod = carMod.proyectos.Single();
            }
            catch
            {
                proMod = new proyectos();
                isUpdate = false;
            }

            proMod.keym = nodeF.Attributes["keym"].Value;
            proMod.id_proyecto = Convert.ToInt64(nodeF.Attributes["id_proyecto"].Value);
            proMod.id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);
            proMod.id_caracteristica = nodeF.Attributes["id_caracteristica"].Value;
            proMod.caracteristicas = carMod;
            proMod.nombre = nodeF.Attributes["nombre"].Value;
            proMod.plantilla = nodeF.Attributes["plantilla"].Value;
            proMod.IR_proyecto = Convert.ToBoolean(nodeF.Attributes["ir_proyecto"].Value);

            if(nodeF.Attributes["icon"].Value != "")
                proMod.icon = nodeF.Attributes["icon"].Value;

            proMod.descripcion = nodeF.Attributes["descripcion"].Value;
            proMod.contador = Convert.ToInt64(nodeF.Attributes["contador"].Value);

            proMod.fecha_ultima_modificacion = Convert.ToDateTime(nodeF.Attributes["fecha_ultima_modificacion"].Value);
            if (!isUpdate) dbMP.proyectos.Add(proMod);
            return proMod;
        }

        public void getProyectoMetaDato(XmlNode nodeF, proyectos proMod)
        {
            proyectos_meta_datos pmdMod;

            long id_proyecto_meta_dato = Convert.ToInt64(nodeF.Attributes["id_proyecto_meta_dato"].Value);

            try
            {
                dbMP.proyectos_meta_datos.Remove(proMod.proyectos_meta_datos.Where(pmd => pmd.id_proyecto_meta_dato == id_proyecto_meta_dato).Single());
            }
            catch { }
            
            pmdMod = new proyectos_meta_datos();

            pmdMod.keym = nodeF.Attributes["keym"].Value;
            pmdMod.id_proyecto_meta_dato = Convert.ToInt64(nodeF.Attributes["id_proyecto_meta_dato"].Value);
            pmdMod.id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);
            pmdMod.proyectos = proMod;
            pmdMod.id_proyecto = nodeF.Attributes["id_proyecto"].Value;
            pmdMod.valor = nodeF.Attributes["valor"].Value;
            pmdMod.is_title = Convert.ToBoolean(nodeF.Attributes["is_title"].Value);
            pmdMod.tipo = nodeF.Attributes["tipo"].Value;
            pmdMod.fecha_ultima_modificacion = Convert.ToDateTime(nodeF.Attributes["fecha_ultima_modificacion"].Value);

            dbMP.proyectos_meta_datos.Add(pmdMod);
        }
    }
}
