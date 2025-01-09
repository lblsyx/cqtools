using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ProtocolCore
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
        /// 操作类型，1：新增，2：修改，3：删除
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 协议名称
        /// </summary>
        public string ProtocolName { get; set; }

        /// <summary>
        /// 协议号
        /// </summary>
        public int ProtocolCode { get; set; }

        /// <summary>
        /// 是否客户端请求
        /// </summary>
        public int FromClient { get; set; }

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

        /// <summary>
        /// 克隆协议
        /// </summary>
        /// <returns></returns>
        public ProtocolInfo Clone()
        {
            ProtocolInfo oProtocolInfo = new ProtocolInfo();

            oProtocolInfo.ProtocolID = ProtocolID;
            oProtocolInfo.OperateType = OperateType;
            oProtocolInfo.ProtocolName = ProtocolName;
            oProtocolInfo.FromClient = FromClient;
            oProtocolInfo.ProtocolSummary = ProtocolSummary;
            oProtocolInfo.SortIndex = SortIndex;
            oProtocolInfo.ProjectID = ProjectID;

            foreach (var item in ReqFields)
            {
                oProtocolInfo.ReqFields.Add(item.Clone() as FieldInfo);
            }

            foreach (var item in ResFields)
            {
                oProtocolInfo.ResFields.Add(item.Clone() as FieldInfo);
            }

            return oProtocolInfo;
        }
    }
}
