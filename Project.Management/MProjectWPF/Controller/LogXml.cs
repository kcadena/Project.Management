using ControlDB.Model;
using System.Windows;
using System.Xml;

namespace MProjectWPF.Controller
{
    public class LogXml
    {
        XmlDocument docXml;
        XmlNode nodeRoot;
        caracteristicas carMod;
        proyectos proMod;
        string doc;

        public LogXml(string d)
        {
            docXml = new XmlDocument();
            doc = d;
            docXml.Load(doc);
            nodeRoot = docXml.DocumentElement;
        }

        public void addField(string id, string action)
        {   
            nodeRoot.AppendChild(createElement(id,action));
            docXml.Save(doc);
        }

        public void readLogUpdate(MProjectDeskSQLITEEntities dbMP)
        {
            Proyectos proCon;
            Actividades actCon;
            int countChilds = nodeRoot.ChildNodes.Count;
            for (int i = 0; i<countChilds; i++)
            {
                XmlNode nodeF = nodeRoot.FirstChild;
                string action = nodeF.Attributes["action"].Value;
                switch (action)
                {
                    case "addPro":
                        proCon = new Proyectos(dbMP);
                        proCon.saveProjectServ(nodeF.InnerText, true);
                        break;

                    case "updPro":
                        proCon = new Proyectos(dbMP);
                        proCon.saveProjectServ(nodeF.InnerText, false);
                        break;

                    case "delPro":
                        proCon = new Proyectos(dbMP);
                        proCon.deleteProjectServ(nodeF.InnerText);
                        break;

                    case "addAct":
                        actCon = new Actividades(dbMP);
                        actCon.saveActivityServ(nodeF.InnerText,true);
                        break;

                    case "updAct":
                        actCon = new Actividades(dbMP);
                        actCon.saveActivityServ(nodeF.InnerText, false);
                        break;

                    case "delAct":
                        actCon = new Actividades(dbMP);
                        actCon.deleteActivityServ(nodeF.InnerText);
                        break;
                }

                proCon = null;
                nodeRoot.RemoveChild(nodeF);
            }
            docXml.Save(doc);
        }

        public void writeLogDownload(XmlNode node, MProjectDeskSQLITEEntities dbMP)
        {
            if(node == null) node = nodeRoot;

            foreach (XmlNode nodeF in node.ChildNodes)
            {
                if (nodeF.Name == "caracteristica")
                {
                    Caracteristicas carCon = new Caracteristicas(dbMP);
                    carMod = carCon.getCaracteristicas(nodeF);
                }
                else if (nodeF.Name == "proyecto")
                {
                    Proyectos proCon = new Proyectos(dbMP);
                    proMod = proCon.getProject(nodeF,carMod);
                }
                else if (nodeF.Name == "proyecto_meta_datos")
                {
                    Proyectos proCon = new Proyectos(dbMP);
                    proCon.getProyectoMetaDato(nodeF, proMod);
                }
                else if (nodeF.Name == "actividad")
                {
                    Actividades actCon = new Actividades(dbMP);
                    actCon.getActivity(nodeF,carMod);
                }
                writeLogDownload(nodeF,dbMP);
            }
        }
        
        private XmlElement createElement(string id, string action)
        {
            XmlElement element = docXml.CreateElement("node");
            element.SetAttribute("action", action);
            element.InnerText = id;
            return element;
        }

    }
}
