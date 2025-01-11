using ProtocolCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityLight.Internets;

namespace ProtocolClient
{
    public class Global
    {
        public static string[] args = null;

        private static ProjectInfo mProjectInfo;

        private static int mStructOperateVer;
        private static int mProtocolOperateVer;

        public static MainForm MainForm = null;

        public const string CODE_REPLACE_STR = "$$CodeContent$$";

        public static string[] ExportFileExts = new string[] { "*.xml"/*, "*.txt"*/ };

        public static Config AppConfig = new Config();

        /// <summary>
        /// 字段类型数据源
        /// </summary>
        public static ArrayList FieldTypeList = new ArrayList();

        /// <summary>
        /// 字段基础类型
        /// </summary>
        public static readonly string[] BasicTypeArray = new string[]
        {
            "bool",
            "char",
            "uchar",
            "short",
            "ushort",
            "int",
            "uint",
            "int64",
            "uint64",
            "float",
            "double",
            "string",
            "map"
        };

        public static int SelectedIndex
        {
            get { return ProjectList.IndexOf(mProjectInfo); }
            set { if (value >= 0 && value < ProjectList.Count) SelectedProject = ProjectList[value]; else SelectedProject = null; }
        }

        public static ProjectInfo SelectedProject
        {
            get { return mProjectInfo; }
            set { mProjectInfo = value; MainForm.RefreshCurrentProjectView(); }
        }

        public static int StructOperateVer
        {
            get { return mStructOperateVer; }
            set { mStructOperateVer = value; MainForm.RefreshCurStructOperateVerView(); }
        }

        public static void SetStructOperateVerUnRefreshView(int ver)
        {
            mStructOperateVer = ver;
        }

        public static int ProtocolOperateVer
        {
            get { return mProtocolOperateVer; }
            set { mProtocolOperateVer = value; MainForm.RefreshCurProtocolOperateVerView(); }
        }

        public static void SetProtocolOperateVerUnRefreshView(int ver)
        {
            mProtocolOperateVer = ver;
        }

        public static BindingList<ProjectInfo> ProjectList = new BindingList<ProjectInfo>();

        public static void ClearProjects()
        {
            ProjectList.Clear();
            MainForm.ClearProjects();
        }

        public static void AddProject(ProjectInfo oProjectInfo)
        {
            ProjectList.Add(oProjectInfo);
            MainForm.AddProject(oProjectInfo);
        }

        public static void UpdateProject(ProjectInfo oProjectInfo)
        {
            MainForm.UpdateProject(oProjectInfo);

            if (SelectedProject == oProjectInfo)
            {//刷新界面
                SelectedProject = oProjectInfo;
            }
        }

        public static void DelProject(ProjectInfo oProjectInfo)
        {
            if (oProjectInfo == null) return;

            if (Global.SelectedProject == oProjectInfo)
            {
                Global.SelectedProject = null;
            }

            int idx = ProjectList.IndexOf(oProjectInfo);

            ProjectList.RemoveAt(idx);

            MainForm.DelProject(oProjectInfo);
        }
    }
}
