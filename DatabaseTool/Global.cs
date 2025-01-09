using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatabaseTool
{
    public class Global
    {
        public static MainForm MainForm;

        public static Paths AppPaths = new Paths();
        public static Config AppConfig = new Config();

        public static BindingList<DBInfo> DBInfoList = new BindingList<DBInfo>();

        public static BindingList<TBInfo> TBInfoList = new BindingList<TBInfo>();
    }
}
