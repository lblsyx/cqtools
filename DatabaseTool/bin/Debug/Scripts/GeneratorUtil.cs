using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DatabaseTool.Scripts
{
    public class GeneratorUtil
    {
        public static List<ColumnInfo> GetColumnList(DBInfo oDBInfo, TBInfo oTBInfo/*, List<ColumnInfo> list*/)
        {
            List<ColumnInfo> list = new List<ColumnInfo>();

            string sql = string.Format("SELECT `COLUMN_NAME`,`DATA_TYPE`,`CHARACTER_MAXIMUM_LENGTH`,`COLUMN_TYPE`,`COLUMN_DEFAULT`,`IS_NULLABLE`,`COLUMN_KEY`,`COLUMN_COMMENT`,`EXTRA` FROM `information_schema`.`COLUMNS` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='{1}'", oDBInfo.DBName, oTBInfo.TBName);

            DataSet ds = DBHelper.Query(sql);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    ColumnInfo oColumnInfo = new ColumnInfo();
                    oColumnInfo.ColumnName = Convert.ToString(dr[0]);
                    oColumnInfo.ColumnType = Convert.ToString(dr[3]);
                    oColumnInfo.DataType = Convert.ToString(dr[1]);
                    oColumnInfo.TypeMaxLength = Convert.ToInt32(dr[2]);
                    oColumnInfo.Comment = Convert.ToString(dr[7]);
                    oColumnInfo.Extra = Convert.ToString(dr[8]);
                    list.Add(oColumnInfo);
                }
            }

            return list;
        }
    }
}
