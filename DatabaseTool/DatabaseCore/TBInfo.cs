using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    /// <summary>
    /// 数据表信息
    /// </summary>
    //[DataContract]
    public class TBInfo
    {
        /// <summary>
        /// 是否选中
        /// </summary>
        //[DataMember(Order = 0)]
        public bool Selected { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        //[DataMember(Order = 1)]
        public string TBName { get; set; }

        /// <summary>
        /// 默认字符集
        /// </summary>
        //[DataMember(Order = 2)]
        public string Charset { get; set; }

        /// <summary>
        /// 数据引擎
        /// </summary>
        //[DataMember(Order = 3)]
        public string Engine { get; set; }

        /// <summary>
        /// 数据表说明
        /// </summary>
        //[DataMember(Order = 4)]
        public string Comment { get; set; }
    }
}