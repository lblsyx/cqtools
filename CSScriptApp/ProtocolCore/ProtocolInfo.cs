using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSScriptApp.ProtocolCore.GenProtocols
{
    /// <summary>
    /// 协议信息
    /// </summary>
    public class ProtocolInfo
    {
        /// <summary>
        /// 协议ID
        /// </summary>
        public int ProtocolID { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        public string ProtocolName { get; set; }

        /// <summary>
        /// 协议说明
        /// </summary>
        public string ProtocolSummary { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 所属项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 请求字段
        /// </summary>
        public BindingList<FieldInfo> ReqFields = new BindingList<FieldInfo>();

        /// <summary>
        /// 响应字段
        /// </summary>
        public BindingList<FieldInfo> ResFields = new BindingList<FieldInfo>();
    }
}
