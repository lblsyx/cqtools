using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityLight.Tpls;

namespace TemplateTool
{
    [TplSearchable]
    public class Global
    {
        public static string[] args;

        public static Assembly CurrentAssembly = null;
        public static string CodeReplaceString = "__CodeContent__";

        public const string MYSQL_CONNECT_TEST_FORMAT = "server={0};port={1};user={2};pwd={3};database={4};pooling=false;connect timeout=2;charset=utf8mb4";
        public const string MYSQL_CONNECTION_FORMAT = "server={0};port={1};user={2};pwd={3};database={4};pooling=false;charset=utf8mb4;Allow User Variables=True;";

        /// <summary>
        /// {0}：数据库名
        /// </summary>
        public const string MYSQL_CREATE_DATABASE = "CREATE DATABASE IF NOT EXISTS `{0}`";
        /// <summary>
        /// {0}：数据库名
        /// </summary>
        public const string MYSQL_SELECT_TABLES = "SELECT `TABLE_NAME` FROM `information_schema`.`TABLES` WHERE `TABLE_SCHEMA` = '{0}'";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// </summary>
        public const string MYSQL_DROP_TABLES = "DROP TABLE IF EXISTS `{0}`.`{1}`";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// </summary>
        public const string MYSQL_FORCE_DROP_TABLES = "DROP TABLE `{0}`.`{1}`";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// {2}：数据列列表
        /// </summary>
        public const string MYSQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS `{0}`.`{1}` (\r\n  {2}\r\n) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;";
        /// <summary>
        /// {0}：数据库名
        /// {1}：数据表名
        /// {2}：行数据
        /// </summary>
        public const string MYSQL_INSERT_DATA = "INSERT INTO `{0}`.`{1}` VALUES ({2});";
    }
}
