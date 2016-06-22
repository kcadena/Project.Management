using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MProjectWPF.Controller
{
    class ConectionString
    {
        public ConectionString()
        {

        }
        public string getConectionString()
        {            
            return @"metadata=res://*/Model.MProjectDeskSQLITE.csdl|res://*/Model.MProjectDeskSQLITE.ssdl|res://*/Model.MProjectDeskSQLITE.msl;provider=System.Data.SQLite.EF6;provider connection string='data source=" + Environment.CurrentDirectory + "\\Model\\MProjectDeskSQLITE.sqlite'";

        }
    }
}
