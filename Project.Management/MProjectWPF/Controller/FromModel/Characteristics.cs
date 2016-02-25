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
        public bool CreateCharacteristicsActivity(Dictionary<string,string> data)
        {
            caracteristica car = new caracteristica();
            car.estado = data["est"];
            car.porcentaje_asignado = Convert.ToInt32(data["per"]);
            car.duracion = Convert.ToInt32(data["dur"]);
            car.tipo_duracion = data["typDur"];
            car.fecha_inicio = DateTime.Parse(data["fec"]);




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
                    Activities acty = new Activities();

                    actividade act = new actividade();
                    act.id_folder = Convert.ToInt32(data["fol"]);
                    System.Windows.MessageBox.Show(data["fol"]);
                    act.nombre = data["nom"];
                    act.descripcion = data["des"];
                    int pos = acty.getPositionAct(Convert.ToInt32(data["fol"]));
                    act.pos = pos + 1;


                    try {
                        MPdb.actividades.Add(act);
                        MPdb.SaveChanges();

                        try
                        {

                            int id = (int) MPdb.actividades.OrderByDescending(e => e.id_actividad).First().id_actividad;
                            System.Windows.MessageBox.Show("" + id);
                            car.id_actividad = id;
                            MPdb.caracteristicas.Add(car);
                            MPdb.SaveChanges();


                        }
                        catch (Exception err) { System.Windows.MessageBox.Show("interno \n"+err.ToString()); }
                    }
                    catch (Exception err) { System.Windows.MessageBox.Show("Externo \n" + err.ToString()); }
                    


                    
                    break;
                //4 subactividad
                case 4:

                    break;
                //5 proyecto generado por actividad
                case 5:

                    break;
            }

            return false;
        }
    }
}
