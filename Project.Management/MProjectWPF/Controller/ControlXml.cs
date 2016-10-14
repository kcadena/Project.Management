using MProjectWPF.Controller;
using ControlDB.Model;
using MProjectWPF.UsersControls.TemplatesControls.FieldsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace MProjectWPF.UsersControls.TemplatesControls
{   
    public class ControlXml
    {
        XmlDocument docXml;
        XmlNode nodeRoot;
        XmlNode nodeField;
        public string nameTemplate;
        public string detailTemplate;
        public bool isSaved;
        string doc;

        public ControlXml(string d)
        {
            docXml = new XmlDocument();
            doc = d;
            try
            {
                docXml.Load(doc);
                nodeRoot = docXml.DocumentElement;
                nameTemplate = nodeRoot.ChildNodes.Item(0).InnerText;
                detailTemplate = nodeRoot.ChildNodes.Item(1).InnerText;
                isSaved = Convert.ToBoolean(nodeRoot.Attributes["haveContent"].Value);
                nodeField = nodeRoot.ChildNodes.Item(4);
                if (doc == "Logs//TemplateTemp.xml") removeAllFields();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
        }

        // PLANTILLAS
        public void addNameAndDetailTemplate(string name, string detail)
        {
            nameTemplate = name;
            detailTemplate = detail;
            nodeRoot.ChildNodes.Item(0).InnerText = name;
            nodeRoot.ChildNodes.Item(1).InnerText = detail;
        }

        public void verifyContent(string name, string detail)
        {
            int c = nodeField.ChildNodes.Count;
            if (name == "" && detail == "" && c == 0)
            {
                nodeRoot.Attributes["haveContent"].Value = "false";
            }
            else {
                addNameAndDetailTemplate(name,detail);
            }
            docXml.Save(doc);
        }

        public void addField(string type, string contentField, string required, int opc)
        {   
            nodeRoot.Attributes["haveContent"].Value = "true";
            nodeField.AppendChild(createElement(type,contentField,required,opc));
            docXml.Save(doc);
        }

        public void editField(string type, string contentField, string required,int pos, int opc)
        {
            XmlNode nodeSelected = nodeField.ChildNodes.Item(pos);
            nodeField.ReplaceChild(createElement(type, contentField, required,opc),nodeSelected);
            docXml.Save(doc);
        }

        public void addItem(string content, int pos)
        {   
            XmlNode nodeItem = nodeField.ChildNodes.Item(pos);
            XmlElement item = docXml.CreateElement("Item");
            item.SetAttribute("content", content);
            nodeItem.AppendChild(item);
            docXml.Save(doc);
        }

        public void editItem(string content,int posField, int posItem)
        {
            XmlNode nodeItem = nodeField.ChildNodes.Item(posField);
            XmlNode oldItem = nodeItem.ChildNodes.Item(posItem);

            XmlElement item = docXml.CreateElement("Item");            
            item.SetAttribute("content", content);
            nodeItem.ReplaceChild(item,oldItem);

            docXml.Save(doc);
        }

        public void removeField(int posField)
        {
            XmlNode nodeSelected = nodeField.ChildNodes.Item(posField);
            nodeField.RemoveChild(nodeSelected);
            docXml.Save(doc);
        }

        public void removeItem(int posField, int posItem)
        {
            XmlNode nodeItem = nodeField.ChildNodes.Item(posField);
            XmlNode oldItem = nodeItem.ChildNodes.Item(posItem);
            nodeItem.RemoveChild(oldItem);
            docXml.Save(doc);
        }

        private XmlElement createElement(string type, string contentField, string required, int opc)
        {
            XmlElement element = docXml.CreateElement(type.Replace(" ",""));
            element.SetAttribute("content", contentField);
            element.SetAttribute("option", ""+opc);
            element.SetAttribute("isRequired", required);
            return element;
        }

        public List<BoxField> loadXmlToTemplate(NewTemplatePanel nt)
        {   
            List<BoxField> lbx = new List<BoxField>();
            foreach (XmlNode nodeF in nodeField.ChildNodes)
            {
                string name = nodeF.Attributes["content"].Value;
                int opt = Convert.ToInt32(nodeF.Attributes["option"].Value);
                bool required = Convert.ToBoolean(nodeF.Attributes["isRequired"].Value);
                BoxField bf = new BoxField(name, opt, nt, required);
                if (opt == 0)
                {   
                    nt.lstField.Items.RemoveAt(nt.lstField.SelectedIndex);                    
                    nt.lstField.SelectedIndex = 0;
                    nt.titleProjectValidation = true;
                }
                if (opt == 3)
                {   
                    foreach (XmlNode nodeI in nodeF.ChildNodes)
                        bf.addComBoxField(nodeI.Attributes["content"].Value);

                    if (nodeF.ChildNodes.Count > 0) bf.comBoxField.Items.RemoveAt(0);
                    bf.comBoxField.SelectedIndex = 0;

                    nt.listValidation.Add(bf);
                }
                lbx.Add(bf);
            }
            return lbx;
        }
        
        public void saveToDataBase(MainWindow mainW, string name, string detail, plantillas p)
        {
            Plantillas pla = new Plantillas(mainW.dbMP);
            string key = p.keym;
            if (pla.deleteTemplate(p) && pla.savetemplate(nodeField.ChildNodes, name, detail,mainW.usuModel,key))
            {
                MessageBox.Show("La plantilla se ha guardado con exito!!");
                removeAllFields();
            }
        }
        
        public void saveToDataBase(MainWindow mainW, string name, string detail)
        {
            Plantillas pla = new Plantillas(mainW.dbMP);            
            
            if(pla.savetemplate(nodeField.ChildNodes, name, detail,mainW.usuModel,""))
            {
                MessageBox.Show("La plantilla se ha guardado con exito!!");
                removeAllFields();
            }         
        }

        public void createXmlFromDatabase(plantillas pla)
        {
            int cont = 0;
            
            addNameAndDetailTemplate(pla.nombre,pla.descripcion);

            foreach (var pmd in pla.plantillas_meta_datos.OrderBy(a => a.id_meta_dato).ToList())
            {
                //revisar
                string type = "Fecha";
                try { 
                    type = pmd.meta_datos.tipos_datos.descripcion;
                }catch(Exception err)
                {
                    MessageBox.Show(err.Message + " " + pmd.meta_datos.id_tipo_dato);
                }
                string contentField = pmd.meta_datos.descripcion;
                string required = pmd.requerido.ToString();
                int opc = (int)pmd.meta_datos.id_tipo_dato;

                if (opc != 5)
                {   
                    addField(type, contentField, required, opc);
                    cont++;
                }
                else
                {
                    addItem(contentField, cont-1);
                }                
            }

        }

        public void removeAllFields()
        {
            nodeRoot.Attributes["haveContent"].Value = "false";
            nodeField.RemoveAll();
            docXml.Save(doc);
        }

        // PROYECTOS
        public void createXmlforProject(string name,string detail)
        {   
            nodeRoot.ChildNodes.Item(2).InnerText = name;
            nodeRoot.ChildNodes.Item(3).InnerText = detail;
        }

        public void createXmlforProject(BoxField bf)
        {
            string type = bf.type2;
            string content = bf.labelBoxField3.Content.ToString().Replace(":", "");
            string required = bf.required.ToString();
            int opc = bf.opc;

            XmlElement element = docXml.CreateElement(type);
            element.SetAttribute("content", content);
            element.SetAttribute("option", "" + opc);
            element.SetAttribute("isRequired", required);

            if (opc != 3) element.InnerText = bf.getValueField();
            nodeField.AppendChild(element);
            if (opc == 3)
            {
                XmlNode nodeItem = nodeField.LastChild;
                foreach (string itemcad in bf.comBoxField3.Items)
                {
                    XmlElement item = docXml.CreateElement("Item");
                    item.SetAttribute("content", itemcad);

                    if (bf.getValueField() == itemcad) item.SetAttribute("isSelected", "" + true);
                    else item.SetAttribute("isSelected", "" + false);
                    nodeItem.AppendChild(item);
                }
            }
            docXml.Save(doc);
        }

        public void loadListBoxField(List<BoxField> lbf)
        {
            try
            {
                BoxField bf;
                foreach (XmlNode nodeF in nodeField.ChildNodes)
                {
                    string value = nodeF.InnerText;
                    string type = nodeF.Name;
                    string descripcion = nodeF.Attributes["content"].Value;
                    int tipo_dato = Convert.ToInt32(nodeF.Attributes["option"].Value);
                    bool required = Convert.ToBoolean(nodeF.Attributes["isRequired"].Value);
                    bf = new BoxField(tipo_dato, descripcion, required, type, value);
                    if (tipo_dato == 3)
                    {
                        int cont = 0;
                        foreach (XmlNode nodeI in nodeF.ChildNodes)
                        {
                            string descI = nodeI.Attributes["content"].Value;
                            bool isSelected = Convert.ToBoolean(nodeI.Attributes["isSelected"].Value);
                            bf.addComBoxField(descI, true);
                            if (isSelected)
                                bf.comBoxField3.SelectedIndex = cont;
                            cont++;
                        }
                    }
                    lbf.Add(bf);
                }
            }
            catch{ }
        }
    }
}

