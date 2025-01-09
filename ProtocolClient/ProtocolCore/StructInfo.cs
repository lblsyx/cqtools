using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ProtocolCore
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
        /// 操作类型，1：新增，2：修改，3：删除
        /// </summary>
        public int OperateType { get; set; }

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
        /// 是否支持msgpack
        /// </summary>
        public int EnableMsgpack { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public BindingList<FieldInfo> Fields = new BindingList<FieldInfo>();

        /// <summary>
        /// 克隆结构体对象
        /// </summary>
        /// <returns></returns>
        public StructInfo Clone()
        {
            StructInfo oStructInfo = new StructInfo();

            oStructInfo.StructID = StructID;
            oStructInfo.OperateType = OperateType;
            oStructInfo.StructName = StructName;
            oStructInfo.StructSummary = StructSummary;
            oStructInfo.SortIndex = SortIndex;
            oStructInfo.ProjectID = ProjectID;
            oStructInfo.EnableMsgpack = EnableMsgpack;

            foreach (var item in Fields)
            {
                oStructInfo.Fields.Add((FieldInfo)item.Clone());
            }

            return oStructInfo;
        }
    }
}
