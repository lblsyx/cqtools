using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    /// <summary>
    /// 数据库信息
    /// </summary>
    //[DataContract]
    public class DBInfo
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        //[DataMember(Order = 1)]
        public string DBName { get; set; }

        /// <summary>
        /// 默认字符集
        /// </summary>
        //[DataMember(Order = 2)]
        public string Charset { get; set; }

        /// <summary>
        /// 默认排序规则
        /// </summary>
        //[DataMember(Order = 3)]
        public string Collation { get; set; }
    }
}