using ControlDB.Model;
using MProjectWPF.UsersControls;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Xml;

namespace MProjectWPF.Controller
{
    class Actividades
    {
        MProjectDeskSQLITEEntities dbMP;

        public Actividades(MProjectDeskSQLITEEntities db)
        {
            dbMP = db;
        }

        public bool createActivity(ActivityPanel actPan)
        {
            string keym;
            usuarios_meta_datos usu = actPan.mainW.usuMod;

            caracteristicas carPad = null;
            
            if (actPan.proMod != null)
            {
                keym = actPan.proMod.keym;
                carPad = actPan.proMod.caracteristicas;
            }
            else
            {
                keym = actPan.actMod.keym;
                carPad = actPan.actMod.caracteristicas;
                actPan.actMod.folder = 1;
            }
            
            usu.table_sequence.caracteristicas = usu.table_sequence.caracteristicas + 1;
            long  lastIdCaracteristica = (long)usu.table_sequence.caracteristicas;

            caracteristicas car = new caracteristicas();
            car.keym = keym;
            car.caracteristicas2 = carPad;
            try
            {
                car.id_caracteristica_padre = carPad.keym + "-" + carPad.id_caracteristica + "-" + carPad.id_usuario;
            }
            catch { }

            car.id_caracteristica = lastIdCaracteristica;
            car.usuarios_meta_datos = usu;
            car.estado = actPan.stageBox.Text;
            car.Porcentaje = Convert.ToInt32(actPan.txtPA.Text.Replace("%", ""));

            try
            {
                actPan.proPanPrev.proMod.caracteristicas.porcentaje_asignado = actPan.proPanPrev.proMod.caracteristicas.porcentaje_asignado + Convert.ToInt32(actPan.txtPA.Text.Replace("%", ""));
            }
            catch
            {
                actPan.actPanPrev.actMod.caracteristicas.porcentaje_asignado = actPan.actPanPrev.actMod.caracteristicas.porcentaje_asignado + Convert.ToInt32(actPan.txtPA.Text.Replace("%", ""));
            }

            if (actPan.tran.estimationValue() > 0)
            {
                caracteristicas carFatCos;
                List<List<string>> lstCos;
                try
                {
                    carFatCos = actPan.proPanPrev.proMod.caracteristicas;
                    lstCos = actPan.proPanPrev.lstCos;
                    
                }
                catch
                {
                    carFatCos = actPan.actPanPrev.actMod.caracteristicas;
                    lstCos = actPan.actPanPrev.lstCos;
                }

                List<string> lstRes = new List<string>();
                lstRes.Add("Asignación a " + actPan.txtNom.Text);
                lstRes.Add("1");
                lstRes.Add(actPan.tran.txtEstimationAs.Text);
                lstRes.Add(actPan.tran.txtEstimationAs.Text);
                lstCos.Add(lstRes);

                long lastidcost;
                try { lastidcost = carFatCos.usuarios_meta_datos.costos.OrderBy(c => c.id_costo).Last().id_costo + 1; }
                catch { lastidcost = 1; }

                carFatCos.costos = "" + (Convert.ToInt64(carFatCos.costos) + Convert.ToInt64(actPan.tran.txtEstimationAs.Text));

                costos cos = new costos();                
                cos.caracteristicas = carFatCos;
                cos.keym = carFatCos.keym;
                cos.id_costo = lastidcost;
                cos.id_caracteristica = carFatCos.keym + "-" + carFatCos.id_caracteristica + "-" + carFatCos.id_usuario;
                cos.nombre = "Asignado a "+ actPan.txtNom.Text;
                cos.cantidad = 1;
                cos.valor = Convert.ToInt64(actPan.tran.txtEstimationAs.Text);
                cos.usuarios_meta_datos = carFatCos.usuarios_meta_datos;
                dbMP.costos.Add(cos);
            }

            car.porcentaje_asignado = 0;
            car.porcentaje_cumplido = 0;
            car.fecha_inicio = actPan.initialDate.SelectedDate;
            car.fecha_fin = actPan.finalDate.SelectedDate;
            car.recursos = actPan.txtResourses.Text;
            car.recursos_restantes = actPan.txtResourses.Text;
            car.presupuesto = actPan.txtEstimation.Text;
            car.costos = actPan.txtCost.Text;
            car.tipo_caracteristica = "a";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;

            usu.table_sequence.actividades = usu.table_sequence.actividades + 1;
            long lastidact =(long) usu.table_sequence.actividades;

            actividades nact = new actividades();
            nact.keym = keym;
            nact.caracteristicas = car;
            nact.id_actividad = lastidact;
            nact.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + usu.id_usuario;
            nact.usuarios_meta_datos = usu;
            nact.nombre = actPan.txtNom.Text;
            nact.descripcion = actPan.txtDesc.Text;
            nact.pos = actPan.lta.pos + 1;
            nact.folder = 0;
            nact.fecha_ultima_modificacion = DateTime.Now;

            dbMP.actividades.Add(nact);

            foreach (List<string> lres in actPan.lstRes)
            {
                long lastidres;
                try { lastidres = usu.recursos.OrderBy(r => r.id_recurso).Last().id_recurso + 1; }
                catch { lastidres = 1; }

                recursos rec = new recursos();
                rec.keym = keym;
                rec.caracteristicas = car;
                rec.id_recurso = lastidres;
                rec.id_caracteristica =  car.keym + "-" + car.id_caracteristica +"-"+ car.id_usuario;
                rec.nombre_recurso = lres.ElementAt(0);
                rec.cantidad = Convert.ToInt64(lres.ElementAt(1));
                rec.usuarios_meta_datos = usu;

                dbMP.recursos.Add(rec);
            }
            foreach (List<string> lest in actPan.lstEst)
            {
                long lastidest;
                try { lastidest = usu.presupuesto.OrderBy(e => e.id_presupuesto).Last().id_presupuesto + 1; }
                catch { lastidest = 1; }

                presupuesto pre = new presupuesto();
                pre.keym = keym;
                pre.caracteristicas = car;
                pre.id_presupuesto = lastidest;
                pre.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
                pre.nombre = lest.ElementAt(0);
                pre.cantidad = Convert.ToInt64(lest.ElementAt(1));
                pre.valor = Convert.ToInt64(lest.ElementAt(2));
                pre.usuarios_meta_datos = usu;

                dbMP.presupuesto.Add(pre);
            }

            foreach (List<string> lcost in actPan.lstCos)
            {
                long lastidcost;
                try { lastidcost = usu.costos.OrderBy(c => c.id_costo).Last().id_costo + 1; }
                catch { lastidcost = 1; }

                costos cos = new costos();
                cos.keym = keym;
                cos.caracteristicas = car;
                cos.id_costo = lastidcost;
                cos.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
                cos.nombre = lcost.ElementAt(0);
                cos.cantidad = Convert.ToInt64(lcost.ElementAt(1));
                cos.valor = Convert.ToInt64(lcost.ElementAt(2));
                cos.usuarios_meta_datos = usu;

                dbMP.costos.Add(cos);
            }

            dbMP.caracteristicas.Add(car);
            
            actPan.actMod = nact;
            return saveChanges();
        }

        public bool editActivity(ActivityPanel actPan)
        {
            usuarios_meta_datos usu = actPan.mainW.usuMod;
            string keym = actPan.actMod.keym;

            caracteristicas car = actPan.actMod.caracteristicas;                      
            car.estado = actPan.stageBox.Text;
            car.porcentaje_asignado = Convert.ToInt32(actPan.txtPA.Text.Replace("%",""));
            car.porcentaje_cumplido = 0;
            car.fecha_inicio = actPan.initialDate.SelectedDate;
            car.fecha_fin = actPan.finalDate.SelectedDate;
            car.recursos = actPan.txtResourses.Text;
            car.presupuesto = actPan.txtEstimation.Text;
            car.costos = actPan.txtCost.Text;
            car.fecha_ultima_modificacion = DateTime.Now;
          
            actividades nact = actPan.actMod;           
            
            nact.nombre = actPan.txtNom.Text;
            nact.descripcion = actPan.txtDesc.Text;            
            nact.fecha_ultima_modificacion = DateTime.Now;

            foreach (recursos rec in actPan.actMod.caracteristicas.recursoslist.ToList())
            {
                dbMP.recursos.Remove(rec);
            }
            foreach (presupuesto pre in actPan.actMod.caracteristicas.presupuestolist.ToList())
            {
                dbMP.presupuesto.Remove(pre);
            }
            foreach (costos cos in actPan.actMod.caracteristicas.costoslist.ToList())
            {
                dbMP.costos.Remove(cos);
            }


            foreach (List<string> lres in actPan.lstRes)
            {
                long lastidres;
                try { lastidres = usu.recursos.OrderBy(r => r.id_recurso).Last().id_recurso + 1; }
                catch { lastidres = 1; }

                recursos rec = new recursos();
                rec.keym = keym;
                rec.caracteristicas = car;
                rec.id_recurso = lastidres;
                rec.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
                rec.nombre_recurso = lres.ElementAt(0);
                rec.cantidad = Convert.ToInt64(lres.ElementAt(1));
                rec.usuarios_meta_datos = usu;

                dbMP.recursos.Add(rec);
            }
            foreach (List<string> lest in actPan.lstEst)
            {
                long lastidest;
                try { lastidest = usu.presupuesto.OrderBy(e => e.id_presupuesto).Last().id_presupuesto + 1; }
                catch { lastidest = 1; }

                presupuesto pre = new presupuesto();
                pre.keym = keym;
                pre.caracteristicas = car;
                pre.id_presupuesto = lastidest;
                pre.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario;
                pre.nombre = lest.ElementAt(0);
                pre.cantidad = Convert.ToInt64(lest.ElementAt(1));
                pre.valor = Convert.ToInt64(lest.ElementAt(2));
                pre.usuarios_meta_datos = usu;

                dbMP.presupuesto.Add(pre);
            }

            foreach (List<string> lcost in actPan.lstCos)
            {
                long lastidcost;
                try { lastidcost = usu.costos.OrderBy(c => c.id_costo).Last().id_costo + 1; }
                catch { lastidcost = 1; }

                costos cos = new costos();
                cos.keym = keym;
                cos.caracteristicas = car;
                cos.id_costo = lastidcost;
                cos.id_caracteristica = car.keym + "-" + car.id_caracteristica + "-" + car.id_usuario; ;
                cos.nombre = lcost.ElementAt(0);
                cos.cantidad = Convert.ToInt64(lcost.ElementAt(1));
                cos.valor = Convert.ToInt64(lcost.ElementAt(2));
                cos.usuarios_meta_datos = usu;

                dbMP.costos.Add(cos);
            }
            actPan.actMod = nact;            
            return saveChanges();
        }

        public bool removeActivity(actividades actMod)
        {
            foreach (caracteristicas car in actMod.caracteristicas.caracteristicas1.ToList())
            {
                if (car.usuarios_meta_datos_asignado == null)
                {
                    removeActivity(car.actividades.First());
                }
                else
                {
                    car.usuarios_meta_datos = car.usuarios_meta_datos_asignado;
                    car.usuarios_meta_datos_asignado = null;
                }
            }
            foreach (archivos arc in actMod.caracteristicas.archivos.ToList())
                dbMP.archivos.Remove(arc);

            foreach (recursos rec in actMod.caracteristicas.recursoslist.ToList())
                dbMP.recursos.Remove(rec);

            foreach (presupuesto est in actMod.caracteristicas.presupuestolist.ToList())
                dbMP.presupuesto.Remove(est);

            foreach (costos cos in actMod.caracteristicas.costoslist.ToList())
                dbMP.costos.Remove(cos);

            dbMP.caracteristicas.Remove(actMod.caracteristicas);
            dbMP.actividades.Remove(actMod);

            return saveChanges();
        }

        //ACTUALIZAR POSICIONES///////////
        public void updatePos(LabelTreeActivity ltaFather)
        {
            if (ltaFather.tvi.Items.Count > 0)
            {
                for (int i = 0; i < ltaFather.tvi.Items.Count; i++)
                {
                    LabelTreeActivity lta = (LabelTreeActivity)ltaFather.tvi.Items.GetItemAt(i);
                    updatePos(lta, i, ltaFather.tvi.Items.Count - 1);
                }
            }
            else
            {
                ltaFather.showActivityIcon();
                ltaFather.actMod.folder = 0;
            }
            saveChanges();
        }

        public void updatePos(ExplorerProject exPro)
        {
            for (int i = 0; i < exPro.tvPro.Items.Count; i++)
            {   
                LabelTreeActivity lta = (LabelTreeActivity)exPro.tvPro.Items.GetItemAt(i);
                updatePos(lta,i, exPro.tvPro.Items.Count - 1);
            }
            saveChanges();
        }

        private void updatePos(LabelTreeActivity lta,int i,int max)
        {
            lta.ua.Visibility = Visibility.Collapsed;
            lta.da.Visibility = Visibility.Collapsed;
            if (max > 0)
            {
                actividades act = lta.actMod;
                act.pos = i + 1;

                if (i == 0) { lta.da.Visibility = Visibility.Visible; lta.pos = i + 1; }
                else if (i == max) { lta.ua.Visibility = Visibility.Visible;  lta.pos = i + 1; }
                else
                {
                    lta.ua.Visibility = Visibility.Visible;
                    lta.da.Visibility = Visibility.Visible;
                    lta.pos = i + 1;
                }
            }
        }
        //FINISH /////////////////////////

        private bool saveChanges()
        {
            try
            {
                try
                {
                    dbMP.SaveChanges();
                }
                catch (Exception err)
                {
                    try
                    {
                        MessageBox.Show(err.InnerException.InnerException.Message);
                    }
                    catch
                    {
                        
                        try
                        {
                            MessageBox.Show(err.InnerException.Message);
                        }
                        catch
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (DbEntityValidationException e)
            {
                MessageBox.Show(e.Message);

                foreach (var eve in e.EntityValidationErrors)
                {
                    MessageBox.Show(eve.Entry.Entity.GetType().Name + " " + eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MessageBox.Show(ve.PropertyName + " " + ve.ErrorMessage);
                    }
                }
                return false;
            }
        }

        //OPCIONES SERVIDOR
        public void saveActivityServ(string caracteristica, bool opt)
        {
            string[] id_car = caracteristica.Split('-');

            string keym = id_car[0];
            long id_caracteristica = Convert.ToInt64(id_car[1]);
            long id_usuario = Convert.ToInt64(id_car[2]);

            caracteristicas car = (from c in dbMP.caracteristicas
                                   where c.keym == keym && c.id_caracteristica == id_caracteristica && c.id_usuario == id_usuario
                                   select c).First();
            

            Dictionary<string, string> u = new Dictionary<string, string>();
            table_sequence tabSeq = car.usuarios_meta_datos.table_sequence;
            u["id_usuario"] = "" + tabSeq.id_usuario ;
            u["actividades"] = "" + tabSeq.actividades ;
            u["archivos"] = "" + tabSeq.archivos ;
            u["caracteristicas"] = "" + tabSeq.caracteristicas ;
            u["costos"] = "" + tabSeq.costos ;
            u["proyectos"] = "" + tabSeq.proyectos ;
            u["proyectos_meta_datos"] = "" + tabSeq.proyectos_meta_datos ;
            u["recursos"] = "" + tabSeq.recursos ;
            u["presupuesto"] = "" + tabSeq.presupuesto ;
            ServerController.updateTableSequence(u);

            u = new Dictionary<string, string>();
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

            if (opt) ServerController.addCaracteristica(u);
            else ServerController.updateCaracteristica(u);


            u = new Dictionary<string, string>();
            actividades act = car.actividades.Single();            
            u["keym"] =  act.keym;
            u["id_actividad"] = "" + act.id_actividad;
            u["id_usuario"] = "" + act.id_usuario;
            u["id_caracteristica"] = "" + act.id_caracteristica;
            u["nombre"] =  act.nombre;
            u["descripcion"] = "" + act.descripcion;
            u["pos"] = "" + act.pos;            
            u["folder"] = "" + act.folder;
            u["fecha_ultima_modificacion"] = "" + act.fecha_ultima_modificacion;

            if (opt) ServerController.addActividad(u);
            else ServerController.updateActividad(u);
           
            foreach (recursos rec in act.caracteristicas.recursoslist.ToList())
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

            foreach (presupuesto pre in act.caracteristicas.presupuestolist.ToList())
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

            foreach (costos cos in act.caracteristicas.costoslist.ToList())
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

        public void deleteActivityServ(string caracteristica)
        {
            string[] id_car = caracteristica.Split('-');

            string keym = id_car[0];
            long id_caracteristica = Convert.ToInt64(id_car[1]);
            long id_usuario = Convert.ToInt64(id_car[2]);

            Dictionary<string, string> u = new Dictionary<string, string>();
            u.Add("keym", keym);
            u.Add("id_caracteristica", "" + id_caracteristica.ToString());
            u.Add("id_usuario", "" + "" + id_usuario);
            ServerController.deleteActividad(u);
        }

        public void getActivity(XmlNode nodeF, caracteristicas carMod)
        {
            actividades actMod;
            bool isUpdate = true;

            try
            {
                actMod = carMod.actividades.Single();
            }
            catch
            {
                actMod = new actividades();
                isUpdate = false;
            }

            actMod.keym = nodeF.Attributes["keym"].Value;
            actMod.id_actividad = Convert.ToInt64(nodeF.Attributes["id_actividad"].Value);
            actMod.id_usuario = Convert.ToInt64(nodeF.Attributes["id_usuario"].Value);
            actMod.id_caracteristica = nodeF.Attributes["id_caracteristica"].Value;
            actMod.caracteristicas = carMod;
            actMod.nombre = nodeF.Attributes["nombre"].Value;
            actMod.descripcion = nodeF.Attributes["descripcion"].Value;
            actMod.pos = Convert.ToInt64(nodeF.Attributes["pos"].Value);
            actMod.folder = Convert.ToInt64(nodeF.Attributes["folder"].Value);
            actMod.fecha_ultima_modificacion = Convert.ToDateTime(nodeF.Attributes["fecha_ultima_modificacion"].Value);

            if (!isUpdate) dbMP.actividades.Add(actMod);
        }
    }
}
