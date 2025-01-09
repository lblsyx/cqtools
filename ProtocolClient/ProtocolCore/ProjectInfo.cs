using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ProtocolCore
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 数据包版本号
        /// </summary>
        public ushort PacketVer { get; set; }

        /// <summary>
        /// 项目说明
        /// </summary>
        public string ProjectSummary { get; set; }

        /// <summary>
        /// 结构列表
        /// </summary>
        public BindingList<StructInfo> Structs = new BindingList<StructInfo>();

        /// <summary>
        /// 协议列表
        /// </summary>
        public BindingList<ProtocolInfo> Protocols = new BindingList<ProtocolInfo>();
    }
}
