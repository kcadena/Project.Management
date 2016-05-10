using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MProjectWPF.Model;

namespace MProjectWPF.Controller.FromModel
{
    class Characteristics
    {
        private MProjectDeskSQLITEEntities MPdb = new MProjectDeskSQLITEEntities();


        /*
        opt => opcion al momento de crear una caracteristica

            1   proyecto
            2   subproyecto
            3   actividad
            4   subactividad
            5   proyecto generado por actividad-> necesita de 4 subactividad
        */
        public actividade CreateCharacteristicsActivity(Dictionary<string,string> data)
        {
            Activities acty = new Activities();
            caracteristica car = new caracteristica();
            actividade act = new actividade();

            //Caracterteristicas
            car.estado = data["est"];
            car.porcentaje_asignado = Convert.ToInt32(data["per"]);
            car.duracion = Convert.ToInt32(data["dur"]);
            car.tipo_duracion = data["typDur"];
            car.fecha_inicio = DateTime.Now;
            car.padre_caracteristica = Convert.ToInt64(data["fat_cha"]);
            car.proyecto_padre = Convert.ToInt64(data["fat_prj"]);


            //Actividades
           
            //System.Windows.MessageBox.Show(data["fol"]);
            act.nombre = data["nom"];
            act.descripcion = data["des"];

            if (data["pos"].Equals("OK"))
            {
                act.id_folder = Convert.ToInt64(data["fol"]);
                int pos = acty.getPositionAct(Convert.ToInt64(data["fat_cha"]), Convert.ToInt64(data["fol"]));
                act.pos = pos + 1;
            }
            else {
                act.id_folder = null;
                int pos = acty.getPositionAct(Convert.ToInt64(data["fat_cha"]), null);
                act.pos = pos + 1;
            }
            try
            {
                MPdb.actividades.Add(act);
                MPdb.SaveChanges();

                try
                {
                    int id = (int)MPdb.actividades.OrderByDescending(e => e.id_actividad).First().id_actividad;
                    //System.Windows.MessageBox.Show("" + id);
                    car.id_actividad = id;
                    MPdb.caracteristicas.Add(car);
                    MPdb.SaveChanges();
                }
                catch (Exception err) { System.Windows.MessageBox.Show("interno \n" + err.ToString()); }
                return act;
            }
            catch (Exception err) {
                System.Windows.MessageBox.Show("Externo \n" + err.ToString());
                return null;
            }
            
        }
        public Dictionary<string,long> deleteCharacteristic(long id)
        {
            Dictionary<string, long> dat = new Dictionary<string, long>();
            caracteristica car = new caracteristica();
            try
            {
                //car = MPdb.caracteristicas.Find(id);
                car = (from x in MPdb.caracteristicas
                          where x.id_caracteristica == id
                          select x).First();
                long parcar= (long)car.padre_caracteristica;
                MPdb.caracteristicas.Remove((caracteristica)car);
                MPdb.SaveChanges();
                dat["act"] = (long) car.id_actividad;
                dat["par"] = parcar;
                return dat;
            }
            catch (Exception err)
            {
                string a = err.ToString();
                return dat;
            }
        }
    }
}
























/*

            switch (Convert.ToInt32(data["opt"]))
            {
                //1 proyecto
                case 1:
                    Projects prj = new Projects();
                    Characteristics cha = new Characteristics();

                    break;
                //2 subproyecto
                case 2:

                    break;
                //3 actividad
                case 3:

                    break;
                //4 subactividad
                case 4:

                    break;
                //5 proyecto generado por actividad
                case 5:

                    break;
            }

    */
