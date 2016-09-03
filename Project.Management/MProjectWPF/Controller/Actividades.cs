using ControlDB.Model;
using MProjectWPF.UsersControls;
using MProjectWPF.UsersControls.ActivityControls.FieldsControls;
using MProjectWPF.UsersControls.ProjectControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;

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
            usuarios_meta_datos usu = actPan.mainW.usuModel;          

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

            long lastIdCaracteristica;
            try { lastIdCaracteristica = usu.caracteristicas.OrderBy(c => c.id_caracteristica).Last().id_caracteristica + 1; }
            catch { lastIdCaracteristica = 1; }

            caracteristicas car = new caracteristicas();
            car.keym = keym;
            car.caracteristicas2 = carPad;
            car.id_caracteristica_padre = carPad.id_caracteristica;
            car.id_caracteristica = lastIdCaracteristica;
            car.usuarios_meta_datos = usu;
            car.estado = actPan.stageBox.Text;
            car.porcentaje_asignado = Convert.ToInt32(actPan.txtPA.Text);
            car.porcentaje_cumplido = 0;
            car.fecha_inicio = actPan.initialDate.SelectedDate;
            car.fecha_fin = actPan.finalDate.SelectedDate;
            car.recursos = actPan.txtResourses.Text;
            car.presupuesto = actPan.txtEstimation.Text;
            car.costos = actPan.txtCost.Text;
            car.tipo_caracteristica = "a";
            car.visualizar_superior = false;
            car.fecha_ultima_modificacion = DateTime.Now;
            
            long lastidact;
            try { lastidact = usu.actividades.OrderBy(a => a.id_actividad).Last().id_actividad + 1; }
            catch { lastidact = 1; }

            actividades nact = new actividades();
            nact.keym = keym;
            nact.caracteristicas = car;
            nact.id_actividad = lastidact;
            nact.id_caracteristica = car.id_caracteristica;
            nact.usuarios_meta_datos = usu;
            nact.nombre = actPan.txtNom.Text;
            nact.descripcion = actPan.txtDesc.Text;

            try
            {
                nact.pos = actPan.actPanPrev.lta.childsCount()+1;
            }
            catch
            {
                nact.pos = actPan.proPanPrev.expP.childsCount()+1;
            }

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
                rec.id_caracteristica = car.id_caracteristica;
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
                pre.id_caracteristica = car.id_caracteristica;
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
                cos.id_caracteristica = car.id_caracteristica;
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
            usuarios_meta_datos usu = actPan.mainW.usuModel;
            string keym = actPan.actMod.keym;

            caracteristicas car = actPan.actMod.caracteristicas;                      
            car.estado = actPan.stageBox.Text;
            car.porcentaje_asignado = Convert.ToInt32(actPan.txtPA.Text);
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
                rec.id_caracteristica = car.id_caracteristica;
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
                pre.id_caracteristica = car.id_caracteristica;
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
                cos.id_caracteristica = car.id_caracteristica;
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

        public bool removeActivity(actividades actMod)
        {
            List<caracteristicas> lstcar = actMod.caracteristicas.caracteristicas1.ToList();

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

        public void updatePos(ActivityPanel actPan)
        {      
            LabelTreeActivity ltaFather = actPan.lta;
                   
            for (int i = 0; i <= ltaFather.tvi.Items.Count; i++)
            {
                LabelTreeActivity lta =(LabelTreeActivity)ltaFather.tvi.Items.GetItemAt(i);
                actividades act = lta.actMod;
                act.pos = i + 1;

                if (i == 0) { lta.da.Visibility = Visibility.Visible; lta.isFirst = true; lta.pos = i + 1; }
                else if (i == lta.tvi.Items.Count - 1) { lta.ua.Visibility = Visibility.Visible; lta.isLast = true; lta.pos = i + 1; }
                else
                {
                    lta.ua.Visibility = Visibility.Visible;
                    lta.da.Visibility = Visibility.Visible;
                    lta.pos = i + 1;
                }

            }
            saveChanges();
        }

        public void updatePos(ProjectPanel proPan)
        {
            
            for (int i = 0; i <= proPan.expP.tvPro.Items.Count; i++)
            {
                LabelTreeActivity lta = (LabelTreeActivity)proPan.expP.tvPro.Items.GetItemAt(i);
                actividades act = lta.actMod;
                act.pos = i + 1;

                if (i == 0) { lta.da.Visibility = Visibility.Visible; lta.isFirst = true; lta.pos = i + 1; }
                else if (i == lta.tvi.Items.Count - 1) { lta.ua.Visibility = Visibility.Visible; lta.isLast = true; lta.pos = i + 1; }
                else
                {
                    lta.ua.Visibility = Visibility.Visible;
                    lta.da.Visibility = Visibility.Visible;
                    lta.pos = i + 1;
                }

            }
            saveChanges();
        }

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
                    MessageBox.Show(err.InnerException.InnerException.Message);
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
    }
}
