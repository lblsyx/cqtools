using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSScriptApp.ProtocolCore.GenProtocols
{
    /// <summary>
    /// 结构信息
    /// </summary>
    public class StructInfo
    {
        /// <summary>
        /// 结构ID
        /// </summary>
        public int StructID { get; set; }

        /// <summary>
        /// 结构名称
        /// </summary>
        public string StructName { get; set; }

        /// <summary>
        /// 结构说明
        /// </summary>
        public string StructSummary { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 所属项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public BindingList<FieldInfo> Fields = new BindingList<FieldInfo>();
    }
}
