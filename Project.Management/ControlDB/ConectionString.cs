using System;

namespace ControlDB
{
    public class ConectionString
    {
        public static string getConectionString()
        {            
            return @"metadata=res://*/Model.MProjectDeskSQLITE.csdl|res://*/Model.MProjectDeskSQLITE.ssdl|res://*/Model.MProjectDeskSQLITE.msl;provider=System.Data.SQLite.EF6;provider connection string='data source=" + Environment.CurrentDirectory + "\\Model\\MProjectDeskSQLITE.sqlite'";
        }
    }
}
