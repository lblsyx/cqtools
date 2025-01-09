using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSScriptApp.ProtocolCore.GenProtocols
{
    /// <summary>
    /// 字段信息
    /// </summary>
    //[DataContract]
    public class FieldInfo : ICloneable
    {
        /// <summary>
        /// 字段名
        /// </summary>
        //[DataMember(Order = 1)]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        //[DataMember(Order = 2)]
        public string FieldType { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        //[DataMember(Order = 3)]
        public string FieldLength { get; set; }

        /// <summary>
        /// 字段说明
        /// </summary>
        //[DataMember(Order = 4)]
        public string FieldSummary { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int SortIndex { get; set; }

        public object Clone()
        {
            FieldInfo oFieldInfo = new FieldInfo();

            XMLUtil.CopyObject(this, oFieldInfo);

            return oFieldInfo;
        }
    }
}
