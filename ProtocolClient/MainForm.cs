using ProtocolCore;
using ProtocolCore.Exports;
using ProtocolCore.Generates;
using ProtocolCore.Imports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using UnityLight.Internets;
using UnityLight.Timers;
using UnityLight.Loggers;
using System.Text.RegularExpressions;

namespace ProtocolClient
{
    public partial class MainForm : Form
    {
        private int mSearchStart;

        public MainForm()
        {
            InitializeComponent();
        }

        public void SetCurrentUser(string name)
        {
            UserStatusLabel.Text = string.Format("当前用户：{0}", name);
        }

        public void SetImports(List<IImport> list)
        {
            ImportToolStripMenuItem.DropDownItems.Clear();

            foreach (var item in list)
            {
                ToolStripMenuItem oImportToolMenuItem = new ToolStripMenuItem();
                oImportToolMenuItem.Text = item.Name;
                oImportToolMenuItem.Tag = item;
                oImportToolMenuItem.Click += oImportToolMenuItem_Click;
                ImportToolStripMenuItem.DropDownItems.Add(oImportToolMenuItem);
            }
        }

        private void oImportToolMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem oToolStripMenuItem = sender as ToolStripMenuItem;
            IImport iIImport = oToolStripMenuItem.Tag as IImport;
            if (iIImport == null) return;

            string path = ProtocolUtil.GetPath(iIImport.InputPathType, iIImport.InputFilter);//string.Format("导入文件({0})|{0}", iIImport.InputFilter)

            if (string.IsNullOrEmpty(path)) return;

            ProjectInfo oProjectInfo = null;

            try
            {
                oProjectInfo = iIImport.Import(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("导入{0}失败!ErrorMsg:{1}", iIImport.Name, ex.Message));
                return;
            }

            if (oProjectInfo == null) return;

            if (ProtocolUtil.ProjectExists(oProjectInfo.ProjectName)) return;

            int errCount = 0;
            if (ProtocolUtil.AddProject(oProjectInfo))
            {
                BindingList<StructInfo> structs = oProjectInfo.Structs;
                BindingList<ProtocolInfo> protocols = oProjectInfo.Protocols;
                oProjectInfo.Structs = new BindingList<StructInfo>();
                oProjectInfo.Protocols = new BindingList<ProtocolInfo>();

                Global.SelectedProject = oProjectInfo;

                for (int i = 0; i < structs.Count; i++)
                {
                    StructInfo oStructInfo = structs[i];
                    oStructInfo.ProjectID = oProjectInfo.ProjectID;
                    if (ProtocolUtil.AddStruct(oStructInfo) == false)
                    {
                        errCount += 1;
                    }
                }

                for (int i = 0; i < protocols.Count; i++)
                {
                    ProtocolInfo oProtocolInfo = protocols[i];
                    oProtocolInfo.ProjectID = oProjectInfo.ProjectID;
                    if (ProtocolUtil.AddProtocol(oProtocolInfo) == false)
                    {
                        errCount += 1;
                    }
                }

                MessageBox.Show(string.Format("导入完成!错误{0}个!", errCount));
            }
        }

        public void SetExports(List<IExport> list)
        {
            ExportToolStripMenuItem.DropDownItems.Clear();

            foreach (var item in list)
            {
                ToolStripMenuItem oExportToolMenuItem = new ToolStripMenuItem();
                oExportToolMenuItem.Text = item.Name;
                oExportToolMenuItem.Tag = item;
                oExportToolMenuItem.Click += oExportToolMenuItem_Click;
                ExportToolStripMenuItem.DropDownItems.Add(oExportToolMenuItem);
            }
        }

        private void oExportToolMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;

            ToolStripMenuItem oToolStripMenuItem = sender as ToolStripMenuItem;
            IExport iIExport = oToolStripMenuItem.Tag as IExport;

            if (iIExport == null) return;

            string path = ProtocolUtil.GetPath(iIExport.OutputPathType, iIExport.OutputFilter);//string.Format("导出文件({0})|{0}", iIImport.InputFilter)

            if (string.IsNullOrEmpty(path)) return;

            try
            {
                iIExport.Export(Global.SelectedProject, path);
                MessageBox.Show("导出{0}成功!", iIExport.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("导出{0}失败!ErrorMsg:{1}", iIExport.Name, ex.Message));
            }
        }

        public void SetGenerators(List<GeneratorInfo> list)
        {
            GenCodeToolStripMenuItem.DropDownItems.Clear();

            foreach (var item in list)
            {
                ToolStripMenuItem oGenCodeToolMenuItem = new ToolStripMenuItem();
                oGenCodeToolMenuItem.Text = item.Name;
                oGenCodeToolMenuItem.Tag = item;
                oGenCodeToolMenuItem.Click += oGenCodeToolMenuItem_Click;
                GenCodeToolStripMenuItem.DropDownItems.Add(oGenCodeToolMenuItem);

                if (Global.args.Length > 0)
                {
                    string args = Global.args[0];
                    if (item.Name.IndexOf(args) > -1)
                    {
                        System.Timers.Timer t = new System.Timers.Timer(2000);   //实例化Timer类，设置间隔时间为10000毫秒；   
                        t.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => theout(s, e, oGenCodeToolMenuItem)); //到达时间的时候执行事件；   
                        t.AutoReset = false;   //设置是执行一次（false）还是一直执行(true)；   
                        t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

                        return;
                    }
                }
            }
        }

        public void theout(object source, System.Timers.ElapsedEventArgs e, ToolStripMenuItem oGenCodeToolMenuItem)
        {
            oGenCodeToolMenuItem_Click(oGenCodeToolMenuItem, null);
        }



        /// <summary>
        /// 生成菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oGenCodeToolMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;

            ToolStripMenuItem oGenCodeToolMenuItem = sender as ToolStripMenuItem;

            GeneratorInfo oGeneratorInfo = oGenCodeToolMenuItem.Tag as GeneratorInfo;
            if (oGeneratorInfo == null)
            {
                MessageBox.Show("打开生成代码前需要先设置生成器信息!\r\n即调用SetGeneratorInfo(GeneratorInfo)方法!");
                return;
            }

            GeneratorSetting oGeneratorSetting = ProtocolUtil.LoadGeneratorSetting(oGeneratorInfo.Type);
            if (oGeneratorSetting == null)
            {
                MessageBox.Show(string.Format("找不到【{0}({1})】的生成模板!\r\n请打开菜单[协议 -> 生成模板]新建模板!", oGeneratorInfo.Name, oGeneratorInfo.Type));
                return;
            }

            GenCodeForm oGenCodeForm = new GenCodeForm();
            oGenCodeForm.SetGeneratorInfo(oGeneratorInfo, oGeneratorSetting);
            oGenCodeForm.ShowDialog(this);
        }

        public void RefreshCurStructOperateVerView()
        {
            verTextBox.Text = Global.StructOperateVer.ToString();
        }

        public void RefreshCurProtocolOperateVerView()
        {
            verTextBox.Text = Global.ProtocolOperateVer.ToString();
        }

        public void RefreshCurrentProjectView()
        {
            for (int i = 0; i < SelectProjToolStripMenuItem.DropDownItems.Count; i++)
            {
                ToolStripMenuItem oToolStripItem = SelectProjToolStripMenuItem.DropDownItems[i] as ToolStripMenuItem;
                oToolStripItem.Checked = false;
            }

            int idx = Global.SelectedIndex;
            if (idx >= 0 && idx < Global.ProjectList.Count)
            {
                ProjectStatusLabel.Text = string.Format("当前项目：{0}", Global.SelectedProject.ProjectName);

                ToolStripMenuItem oToolStripMenuItem = SelectProjToolStripMenuItem.DropDownItems[idx] as ToolStripMenuItem;
                if (oToolStripMenuItem != null) oToolStripMenuItem.Checked = true;

                structDataGridView.DataSource = Global.SelectedProject.Structs;
                protocolDataGridView.DataSource = Global.SelectedProject.Protocols;

                ProtocolUtil.ReloadStructList(Global.SelectedProject, Global.StructOperateVer);
                ProtocolUtil.ReloadProtocolList(Global.SelectedProject, Global.ProtocolOperateVer);
            }
            else
            {
                ProjectStatusLabel.Text = "当前项目：-";
                structDataGridView.DataSource = null;
                protocolDataGridView.DataSource = null;
                Global.AppConfig.ProjectID = 0;
                Global.AppConfig.Save();
            }
        }

        public void ClearProjects()
        {
            SelectProjToolStripMenuItem.DropDownItems.Clear();
        }

        public void AddProject(ProjectInfo oProjectInfo)
        {
            if (oProjectInfo == null) return;

            ToolStripMenuItem oToolStripMenuItem = new ToolStripMenuItem();
            oToolStripMenuItem.Text = oProjectInfo.ProjectName;
            oToolStripMenuItem.Click += oToolStripMenuItem_Click;
            SelectProjToolStripMenuItem.DropDownItems.Add(oToolStripMenuItem);
        }

        public void UpdateProject(ProjectInfo oProjectInfo)
        {
            int idx = Global.SelectedIndex;

            if (idx == -1) return;

            SelectProjToolStripMenuItem.DropDownItems[idx].Text = oProjectInfo.ProjectName;
        }

        public void DelProject(ProjectInfo oProjectInfo)
        {
            int idx = Global.ProjectList.IndexOf(oProjectInfo);

            if (idx == -1) return;

            SelectProjToolStripMenuItem.DropDownItems.RemoveAt(idx);
        }

        private void oToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem oToolStripMenuItem = sender as ToolStripMenuItem;

            if (oToolStripMenuItem == null) return;

            int idx = SelectProjToolStripMenuItem.DropDownItems.IndexOf(oToolStripMenuItem);

            Global.SelectedProject = Global.ProjectList[idx];

            if (Global.SelectedProject != null)
            {
                Global.AppConfig.ProjectID = Global.SelectedProject.ProjectID;
                Global.AppConfig.Save();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //globalTimer.Start();

            XLogger.AddLogger(new MsgBoxLogger());

            Global.AppConfig.Load();

            Global.FieldTypeList.AddRange(Global.BasicTypeArray);

            if (ProtocolUtil.CheckUserName())
            {
                SetCurrentUser(Global.AppConfig.Name);
            }

            if (ProtocolUtil.CheckSQLConnectionString())
            {
                ProtocolUtil.ReloadProjectList();
                ProtocolUtil.ReloadCurStructOperateVer();
                ProtocolUtil.ReloadCurProtocolOperateVer();
            }

            if (Global.AppConfig.ProjectID != 0)
            {
                foreach (var item in Global.ProjectList)
                {
                    if (item.ProjectID == Global.AppConfig.ProjectID)
                    {
                        Global.SelectedProject = item;
                    }
                }
            }

            ProtocolUtil.CompilerScripts();
        }

        private void globalTimer_Tick(object sender, EventArgs e)
        {
            //if (mStopwatch.IsRunning)
            //{
            //    mStopwatch.Stop();
            //    deltaTime = mStopwatch.ElapsedMilliseconds;
            //    mStopwatch.Reset();
            //}
            //mStopwatch.Start();

            //TimerMgr.Update(deltaTime / 1000.0f);
        }

        private void NewProjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectInfoForm oProjInfoForm = new ProjectInfoForm();
            oProjInfoForm.New();
            oProjInfoForm.ShowDialog(this);
        }

        private void EditProjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckSelectProject())
            {
                ProjectInfoForm oProjInfoForm = new ProjectInfoForm();
                oProjInfoForm.Edit(Global.SelectedProject);
                oProjInfoForm.ShowDialog(this);
            }
        }

        private void ProjMgrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectMgrForm oProjMgrForm = new ProjectMgrForm();
            oProjMgrForm.ShowDialog(this);
        }

        private void ToolDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm oDBSettingForm = new SettingForm();
            oDBSettingForm.ShowDialog(this);

            if (ProtocolUtil.CheckSQLConnectionString())
            {
                ProtocolUtil.ReloadProjectList();
                ProtocolUtil.ReloadCurStructOperateVer();
                ProtocolUtil.ReloadCurProtocolOperateVer();
            }

            ProtocolUtil.CheckRight();
        }

        private void RefreshProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProtocolUtil.ReloadStructList(Global.SelectedProject, Global.StructOperateVer);
            ProtocolUtil.ReloadProtocolList(Global.SelectedProject, Global.ProtocolOperateVer);
        }

        private void ComplierScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ProtocolUtil.CompilerScripts();
        }

        private void newProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用最新版本号和当前版本号对比
            if (Global.ProtocolOperateVer != MySQLUtil.ReloadCurProtocolOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            ProtocolInfoForm oProtocolInfoForm = new ProtocolInfoForm();
            oProtocolInfoForm.New();
            oProtocolInfoForm.ShowDialog(this);
        }

        private void editProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用协议最新版本号和当前版本号对比
            if (Global.ProtocolOperateVer != MySQLUtil.ReloadCurProtocolOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            if (protocolDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = protocolDataGridView.SelectedRows[0];
                ProtocolInfo oProtocolInfo = oDataGridViewRow.DataBoundItem as ProtocolInfo;

                if (ProtocolUtil.CheckProtocolLocker(oProtocolInfo)) return;

                if (ProtocolUtil.UpdateProtocolLocker(oProtocolInfo, Global.AppConfig.Name))
                {
                    ProtocolInfoForm oProtocolInfoForm = new ProtocolInfoForm();
                    oProtocolInfoForm.Edit(oProtocolInfo);
                    oProtocolInfoForm.ShowDialog(this);
                    ProtocolUtil.ClearProtocolLocker(oProtocolInfo, Global.AppConfig.Name);
                    protocolDataGridView_SelectionChanged(null, null);
                }
                else
                {
                    MessageBox.Show("锁定失败!");
                }
            }
            else
            {
                MessageBox.Show(string.Format("请先左键选择协议后编辑!"));
            }
        }

        private void delProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用协议最新版本号和当前版本号对比
            if (Global.ProtocolOperateVer != MySQLUtil.ReloadCurProtocolOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            if (protocolDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = protocolDataGridView.SelectedRows[0];
                int idx = protocolDataGridView.Rows.IndexOf(oDataGridViewRow);
                if (idx < 0 || idx >= Global.SelectedProject.Protocols.Count) return;

                ProtocolInfo oProtocolInfo = Global.SelectedProject.Protocols[idx];
                if (MessageBox.Show(string.Format("是否删除【{0}】协议!", oProtocolInfo.ProtocolName), "删除协议", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ProtocolUtil.DelProtocol(oProtocolInfo);
                }
            }
            else
            {
                MessageBox.Show(string.Format("请先左键选择协议后删除!"));
            }
        }

        private void protocolDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            editProtocolToolStripMenuItem_Click(null, null);
        }

        private void newStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用结构的最新版本号和当前版本号对比
            if (Global.StructOperateVer != MySQLUtil.ReloadCurStructOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            StructInfoForm oStructInfoForm = new StructInfoForm();
            oStructInfoForm.New();
            oStructInfoForm.ShowDialog(this);
        }

        private void editStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用结构的最新版本号和当前版本号对比
            if (Global.StructOperateVer != MySQLUtil.ReloadCurStructOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            if (structDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = structDataGridView.SelectedRows[0];
                StructInfo oStructInfo = oDataGridViewRow.DataBoundItem as StructInfo;

                if (ProtocolUtil.CheckStructLocker(oStructInfo)) return;

                if (ProtocolUtil.UpdateStructLocker(oStructInfo, Global.AppConfig.Name))
                {
                    StructInfoForm oStructInfoForm = new StructInfoForm();
                    oStructInfoForm.Edit(oStructInfo);
                    oStructInfoForm.ShowDialog(this);
                    ProtocolUtil.ClearStructLocker(oStructInfo, Global.AppConfig.Name);
                    structDataGridView_SelectionChanged(null, null);
                }
                else
                {
                    MessageBox.Show("锁定失败!");
                }
            }
            else
            {
                MessageBox.Show(string.Format("请先左键选择结构后编辑!"));
            }
        }

        private void delStructToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;
            // 用结构的最新版本号和当前版本号对比
            if (Global.StructOperateVer != MySQLUtil.ReloadCurStructOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            if (structDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = structDataGridView.SelectedRows[0];
                int idx = structDataGridView.Rows.IndexOf(oDataGridViewRow);
                if (idx < 0 || idx >= Global.SelectedProject.Structs.Count) return;

                StructInfo oStructInfo = Global.SelectedProject.Structs[idx];
                if (MessageBox.Show(string.Format("是否删除【{0}】结构，此操作无法恢复!", oStructInfo.StructName), "删除结构", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ProtocolUtil.DelStruct(oStructInfo);
                }
            }
            else
            {
                MessageBox.Show(string.Format("请先左键选择结构后删除!"));
            }
        }

        private void structDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            editStructToolStripMenuItem_Click(null, null);
        }

        private void GenSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckRight() == false) return;

            GenSettingMgrForm oGenSettingMgrForm = new GenSettingMgrForm();
            oGenSettingMgrForm.ShowDialog(this);
        }

        private void protocolDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (psTabControl.SelectedIndex != 0) return;

            if (protocolDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = protocolDataGridView.SelectedRows[0];
                ProtocolInfo oProtocolInfo = oDataGridViewRow.DataBoundItem as ProtocolInfo;

                if (oProtocolInfo == null) return;

                string reqStr = genViewStr(string.Format("Req{0}", oProtocolInfo.ProtocolName), oProtocolInfo.ReqFields);
                string resStr = genViewStr(string.Format("Res{0}", oProtocolInfo.ProtocolName), oProtocolInfo.ResFields);

                fieldsRichTextBox.Text = string.Format("{0}\r\n\r\n{1}", reqStr, resStr);
            }
            else
            {
                fieldsRichTextBox.Text = string.Empty;
            }
        }

        private string genViewStr(string name, BindingList<FieldInfo> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name).Append(":\r\n");

            int nameMaxLen = 0;
            int typeMaxLen = 0;
            foreach (var item in list)
            {
                nameMaxLen = Math.Max(nameMaxLen, item.FieldName.Length);
                typeMaxLen = Math.Max(typeMaxLen, item.FieldType.Length);
                if (item.FieldType == "map")
                {
                    string realTypeStr = string.Format("{0}<{1},{2}>", item.FieldType, item.MapFieldKeyType, item.MapFieldValueType);
                    typeMaxLen = Math.Max(typeMaxLen, realTypeStr.Length);
                }
            }

            int temp = 0;
            temp = nameMaxLen % 4;
            if (temp != 0)
            {
                nameMaxLen = nameMaxLen + 4 - temp;
            }
            else
            {
                nameMaxLen += 4;
            }
            temp = typeMaxLen % 4;
            if (temp != 0)
            {
                typeMaxLen = typeMaxLen + 4 - temp;
            }
            else
            {
                typeMaxLen += 4;
            }

            List<string> fields = new List<string>();
            foreach (var item in list)
            {
                string nameStr = item.FieldName.PadRight(nameMaxLen);
                string typeStr = item.FieldType.PadRight(typeMaxLen);

                if (item.FieldType == "map")
                {
                    typeStr = string.Format("{0}<{1},{2}>", item.FieldType, item.MapFieldKeyType, item.MapFieldValueType).PadRight(typeMaxLen);
                }
                fields.Add(string.Format("    {0}{1}{3}//{2}", nameStr, typeStr, item.FieldSummary, string.IsNullOrEmpty(item.FieldLength) ? string.Empty : "[" + item.FieldLength + "]"));
            }

            sb.Append(string.Join("\r\n", fields.ToArray()));

            return sb.ToString();
        }

        private void structDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (psTabControl.SelectedIndex != 1) return;
            if (structDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = structDataGridView.SelectedRows[0];
                StructInfo oStructInfo = oDataGridViewRow.DataBoundItem as StructInfo;

                if (oStructInfo == null) return;

                fieldsRichTextBox.Text = genViewStr(oStructInfo.StructName, oStructInfo.Fields);
            }
            else
            {
                fieldsRichTextBox.Text = string.Empty;
            }
        }

        private void psTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            mSearchStart = 0;
            if (psTabControl.SelectedIndex == 0)
            {
                RefreshCurProtocolOperateVerView();
                protocolDataGridView_SelectionChanged(null, null);
            }
            else if (psTabControl.SelectedIndex == 1)
            {
                RefreshCurStructOperateVerView();
                structDataGridView_SelectionChanged(null, null);
            }
            else
            {
                //RefreshCurStructOperateVerView();
                //RefreshCurProtocolOperateVerView();
                fieldsRichTextBox.Text = string.Empty;
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            mSearchStart = 0;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (ProtocolUtil.CheckSelectProject() == false) return;

            string searchStr = searchTextBox.Text.ToLower().Trim();

            if (string.IsNullOrEmpty(searchStr)) return;

            bool bSearchEnd = true;
            ProjectInfo oProjectInfo = Global.SelectedProject;

            if (psTabControl.SelectedIndex == 0)
            {
                for (int i = mSearchStart; i < oProjectInfo.Protocols.Count; i++)
                {
                    ProtocolInfo oProtocolInfo = oProjectInfo.Protocols[i];

                    if (oProtocolInfo.ProtocolName.ToLower().IndexOf(searchStr) != -1)
                    {
                        //bExists = true;
                        bSearchEnd = false;
                        mSearchStart = i + 1;
                        protocolDataGridView.CurrentCell = protocolDataGridView.Rows[i].Cells[0];
                        break;
                    }
                    else if (oProtocolInfo.ProtocolSummary.ToLower().IndexOf(searchStr) != -1)
                    {
                        //bExists = true;
                        bSearchEnd = false;
                        mSearchStart = i + 1;
                        protocolDataGridView.CurrentCell = protocolDataGridView.Rows[i].Cells[0];
                        break;
                    }

                    if (i == (oProjectInfo.Protocols.Count - 1))
                    {
                        bSearchEnd = true;
                    }
                }

                if (mSearchStart >= oProjectInfo.Protocols.Count)
                {
                    bSearchEnd = true;
                }

                if (bSearchEnd)
                {
                    mSearchStart = oProjectInfo.Protocols.Count;
                }
            }
            else if (psTabControl.SelectedIndex == 1)
            {
                for (int i = mSearchStart; i < oProjectInfo.Structs.Count; i++)
                {
                    StructInfo oStructInfo = oProjectInfo.Structs[i];

                    if (oStructInfo.StructName.IndexOf(searchStr) == -1)
                    {
                        //bExists = true;
                        bSearchEnd = false;
                        mSearchStart = i + 1;
                        structDataGridView.CurrentCell = structDataGridView.Rows[i].Cells[0];
                        break;
                    }
                    else if (oStructInfo.StructSummary.IndexOf(searchStr) == -1)
                    {
                        //bExists = true;
                        bSearchEnd = false;
                        mSearchStart = i + 1;
                        structDataGridView.CurrentCell = structDataGridView.Rows[i].Cells[0];
                        break;
                    }

                    if (i == (oProjectInfo.Structs.Count - 1))
                    {
                        bSearchEnd = true;
                    }
                }

                if (mSearchStart >= oProjectInfo.Protocols.Count)
                {
                    bSearchEnd = true;
                }

                if (bSearchEnd)
                {
                    mSearchStart = oProjectInfo.Protocols.Count;
                }
            }

            if (bSearchEnd)
            {
                mSearchStart = 0;
                MessageBox.Show("本次定位结束,重新搜索!");
            }
        }

        private void verTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(verTextBox.Text))
            {
                if (psTabControl.SelectedIndex == 0)
                {
                    Global.ProtocolOperateVer = 0;
                }
                else
                {
                    Global.StructOperateVer = 0;
                }
                verTextBox.Select(0, 1);
                return;
            }

            bool isNumber = true;
            char[] chars = verTextBox.Text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsDigit(chars[i]) == false)
                {
                    isNumber = false;
                    break;
                }
            }
            if (isNumber)
            {
                if (psTabControl.SelectedIndex == 0)
                {
                    Global.SetProtocolOperateVerUnRefreshView(Convert.ToInt32(verTextBox.Text));
                }
                else
                {
                    Global.SetStructOperateVerUnRefreshView(Convert.ToInt32(verTextBox.Text));
                }
            }
            else
            {

                int i = verTextBox.SelectionStart - 1;
                if (psTabControl.SelectedIndex == 0)
                {
                    Global.ProtocolOperateVer = Global.ProtocolOperateVer;
                }
                else
                {
                    Global.StructOperateVer = Global.StructOperateVer;
                }
                verTextBox.Select(i, 0);
            }
        }

        private void verTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                viewButton_Click(null, null);
            }
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            if (psTabControl.SelectedIndex == 0)
            {
                ProtocolUtil.ReloadProtocolList(Global.SelectedProject, Global.ProtocolOperateVer);
            }
            else
            {
                ProtocolUtil.ReloadStructList(Global.SelectedProject, Global.StructOperateVer);
            }
        }

        private void lastViewButton_Click(object sender, EventArgs e)
        {
            if (psTabControl.SelectedIndex == 0)
            {
                ProtocolUtil.ReloadCurProtocolOperateVer();
            }
            else
            {
                ProtocolUtil.ReloadCurStructOperateVer();
            }
            viewButton_Click(null, null);
        }

        private void protocolDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            protocolDataGridView.EndEdit();
        }

        private void Column_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e == null || e.RowIndex < 0) return;
            if (ProtocolUtil.CheckRight() == false) return;
            // 用最新版本号和当前版本号对比
            if (Global.ProtocolOperateVer != MySQLUtil.ReloadCurProtocolOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }

            if (protocolDataGridView.SelectedRows.Count != 0)
            {
                ProtocolInfo oProtocolInfo = protocolDataGridView.SelectedRows[0].DataBoundItem as ProtocolInfo;
                string Text = string.Empty;
                if (oProtocolInfo.FromClient == 0)
                {
                    Text = "设定：客户端不请求";
                }
                else
                {
                    Text = "设定：客户端将请求";
                }
                string Title = string.Format("Req{0}", oProtocolInfo.ProtocolName);
                if (MessageBox.Show(Text, Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ProtocolUtil.UpdateProtocol(oProtocolInfo);
                }
                else
                {
                    if (oProtocolInfo.FromClient == 0)
                    {
                        oProtocolInfo.FromClient = 1;
                    }
                    else
                    {
                        oProtocolInfo.FromClient = 0;
                    }

                    protocolDataGridView.UpdateCellValue(e.RowIndex, e.ColumnIndex);
                }
            }
            else
            {
                MessageBox.Show(string.Format("请先左键选择协议后编辑!"));
            }
        }
    }
}
