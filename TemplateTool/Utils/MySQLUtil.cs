using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TemplateTool.Datas;
using UnityLight.Loggers;

namespace TemplateTool.Utils
{
    public class MySQLUtil
    {
        /// <summary>
        /// Returns a string with backslashes before characters that need to be quoted
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }


        /// <summary>
        /// Un-quotes a quoted string
        /// </summary>
        /// <param name="InputTxt">Text string need to be escape with slashes</param>
        public static string StripSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;

            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"(\\)([\000\010\011\012\015\032\042\047\134\140])", "$2");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }

            return Result;
        }

        public static MySqlConnection OpenMySQLConnection(string connStr)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                try { conn.Close(); }
                catch { }

                XLogger.Error(ex);
                return null;
            }

            return conn;
        }

        public static bool TestMySQLConnect(string host, string port, string user, string pwd, ref string errorMsg)
        {
            if (string.IsNullOrEmpty(host)) host = string.Empty;
            if (string.IsNullOrEmpty(port)) port = string.Empty;
            if (string.IsNullOrEmpty(user)) user = string.Empty;
            if (string.IsNullOrEmpty(pwd)) pwd = string.Empty;

            MySqlConnection conn = new MySqlConnection(string.Format(Global.MYSQL_CONNECT_TEST_FORMAT, host.Trim(), port.Trim(), user.Trim(), pwd.Trim(), "mysql"));

            bool bCanConnected = false;

            try
            {
                conn.Open();
                bCanConnected = true;
            }
            catch (Exception ex)
            {
                errorMsg = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
                bCanConnected = false;
            }
            finally
            {
                try { conn.Close(); }
                catch { }
            }

            return bCanConnected;
        }

        public static void CreateDatabaseIfNotExists(MySqlConnection conn, string db)
        {
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = conn;
            sqlcmd.CommandText = string.Format(Global.MYSQL_CREATE_DATABASE, db);

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }
        }

        public static IList<string> GetTableList(MySqlConnection conn, string db)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = string.Format(Global.MYSQL_SELECT_TABLES, db);

            IList<string> tables = new List<string>();
            IDataReader reader = null;

            try
            {
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);

                try { reader.Close(); }
                catch { }
            }

            return tables;
        }
        public static void DropTable(MySqlConnection conn, string db, string tableName)
        {

            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = conn;
            sqlcmd.CommandText = string.Format(Global.MYSQL_DROP_TABLES, db, tableName);
            try
            {
                sqlcmd.ExecuteNonQuery();
                //XLogger.InfoFormat("数据表删除成功!{0}.{1}", db, tableName);
            }
            catch (Exception ex)
            {
                XLogger.ErrorFormat("数据表删除失败!{0}.{1} : {2}", db, tableName, sqlcmd.CommandText);
                XLogger.Error(ex);
            }
        }
        public static void DropTables(MySqlConnection conn, string db, bool force)
        {
            IList<string> tables = GetTableList(conn, db);

            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = conn;

            foreach (string table in tables)
            {
                if (force)
                {
                    sqlcmd.CommandText = string.Format(Global.MYSQL_FORCE_DROP_TABLES, db, table);
                }
                else
                {
                    sqlcmd.CommandText = string.Format(Global.MYSQL_DROP_TABLES, db, table);
                }

                try
                {
                    sqlcmd.ExecuteNonQuery();
                    //XLogger.InfoFormat("数据表删除成功!{0}.{1}", db, table);
                }
                catch (Exception ex)
                {
                    XLogger.ErrorFormat("数据表删除失败!{0}.{1} : {2}", db, table, sqlcmd.CommandText);
                    XLogger.Error(ex);
                }
            }
        }

        public static bool CreateTable(MySqlConnection conn, string db, TableInfo table, string columnStr)
        {
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = conn;
            sqlcmd.CommandText = string.Format(Global.MYSQL_CREATE_TABLE, db, table.TableName, columnStr);

            try
            {
                sqlcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);
            }

            return false;
        }

        public static bool InsertTableRecord(MySqlConnection conn, string db, TableInfo table, string insertValString)
        {
            MySqlCommand sqlcmd = new MySqlCommand();
            sqlcmd.Connection = conn;
            sqlcmd.CommandText = string.Format(Global.MYSQL_INSERT_DATA, db, table.TableName, insertValString);
            try
            {
                sqlcmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                XLogger.Error(ex);

            }

            return false;
        }
        public static double GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalMilliseconds;
        }
    }
}
