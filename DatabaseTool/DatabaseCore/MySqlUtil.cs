using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    public class MySqlUtil
    {
        public static void GetDatabaseList(List<DBInfo> list)
        {
            string sql = "SELECT `SCHEMA_NAME`,`DEFAULT_CHARACTER_SET_NAME`,`DEFAULT_COLLATION_NAME` FROM `information_schema`.`SCHEMATA` WHERE `SCHEMA_NAME` <> 'information_schema' AND `SCHEMA_NAME` <> 'mysql' AND `SCHEMA_NAME` <> 'performance_schema'";

            DataSet ds = DBHelper.Query(sql);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    DBInfo oDBInfo = new DBInfo();
                    oDBInfo.DBName = Convert.ToString(dr[0]);
                    oDBInfo.Charset = Convert.ToString(dr[1]);
                    oDBInfo.Collation = Convert.ToString(dr[2]);
                    list.Add(oDBInfo);
                }
            }
        }

        public static void GetTableList(DBInfo oDBInfo, List<TBInfo> list)
        {
            string sql = string.Format("SELECT `TABLE_NAME`,`TABLE_COLLATION`,`ENGINE`,`TABLE_COMMENT` FROM `information_schema`.`TABLES` WHERE `TABLE_SCHEMA` = '{0}'", oDBInfo.DBName);

            DataSet ds = DBHelper.Query(sql);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    TBInfo oTBInfo = new TBInfo();
                    oTBInfo.Selected = true;
                    //oTBInfo.Charset = oDBInfo.Charset.Substring(0, oDBInfo.Charset.IndexOf("_")); ;
                    oTBInfo.TBName = Convert.ToString(dr[0]);
                    oTBInfo.Charset = Convert.ToString(dr[1]);
                    oTBInfo.Engine = Convert.ToString(dr[2]);
                    oTBInfo.Comment = Convert.ToString(dr[3]);
                    list.Add(oTBInfo);
                }
            }
        }

        public static void DeleteTable(DBInfo oDBInfo, TBInfo oTBInfo)
        {
            string sql = string.Format("DELETE FROM `{0}`.`{1}`", oDBInfo.DBName, oTBInfo.TBName);

            int rows = DBHelper.ExecuteSql(sql);
        }

        public static void DeleteTable(DBInfo oDBInfo, TBInfo[] oTBInfos)
        {
            int rows = 0;
            string sql = string.Empty;

            foreach (TBInfo oTBInfo in oTBInfos)
            {
                sql = string.Format("DELETE FROM `{0}`.`{1}`", oDBInfo.DBName, oTBInfo.TBName);

                rows = DBHelper.ExecuteSql(sql);
            }
        }

        public static void TruncateTable(DBInfo oDBInfo, TBInfo oTBInfo)
        {
            string sql = string.Format("TRUNCATE TABLE `{0}`.`{1}`", oDBInfo.DBName, oTBInfo.TBName);

            int rows = DBHelper.ExecuteSql(sql);
        }

        public static void TruncateTable(DBInfo oDBInfo, TBInfo[] oTBInfos)
        {
            foreach (TBInfo oTBInfo in oTBInfos)
            {
                string sql = string.Format("TRUNCATE TABLE `{0}`.`{1}`", oDBInfo.DBName, oTBInfo.TBName);

                int rows = DBHelper.ExecuteSql(sql);
            }
        }
    }
}
