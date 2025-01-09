using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using UnityLight.Internets;
using UnityLight.Tpls;
using RXHWRobot.Robots;
using RXHWRobot.Datas;
using RXHWRobot.Serializes.IOs;
using System.Threading;

namespace RXHWRobot
{
    public partial class MainForm : Form
    {
        private bool logTextChange = false;
        private object logTextLocker = new object();
        private List<string> logTextList = new List<string>();

        private string assetRoot = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        #region 配置文件

        private void MainForm_Load(object sender, EventArgs e)
        {
            timer1.Start();

            PacketConfig.PackageHeader = PacketVer.Ver;
            Global.CurrentAssembly = typeof(Global).Assembly;
            assetRoot = Path.Combine(Application.StartupPath, "Assets");

            TplMgr.SearchAssembly(Global.CurrentAssembly);
            PacketFactory.SearchAssembly(Global.CurrentAssembly);

            if (RobotConfig.Exists == false)
            {
                loginServerList.SelectedIndex = 0;

                updateConfig();

                RobotConfig.Save();
            }
            else
            {
                RobotConfig.Load();

                updateFormData();
            }

            if (RobotConfig.Instance.AutoLoadResource && Directory.Exists(assetRoot))
            {
                LoadResource(false);
            }
        }

        private void updateConfig()
        {
            RobotConfig.Instance.PlatformID = Convert.ToInt32(platformIDText.Text);
            RobotConfig.Instance.LoginServerSelectedIndex = loginServerList.SelectedIndex;
            RobotConfig.Instance.AutoLoadResource = autoLoadCheck.Checked;
            RobotConfig.Instance.EnableRandomFight = randomFightCheck.Checked;
            RobotConfig.Instance.LoginServers.Clear();
            foreach (var item in loginServerList.Items)
            {
                RobotConfig.Instance.LoginServers.Add(item.ToString());
            }
            RobotConfig.Instance.AssetRoot = assetRootText.Text;
            RobotConfig.Instance.RobotSignKey = robotSignKeyText.Text;
            RobotConfig.Instance.AccountPre = accountPreText.Text;
            RobotConfig.Instance.RobotNum = Convert.ToInt32(robotNumText.Text);
            RobotConfig.Instance.MapID[0] = Convert.ToUInt32(MapID0.Text);
            RobotConfig.Instance.MapID[1] = Convert.ToUInt32(MapID1.Text);
            RobotConfig.Instance.MapID[2] = Convert.ToUInt32(MapID2.Text);
            RobotConfig.Instance.MapID[3] = Convert.ToUInt32(MapID3.Text);
            RobotConfig.Instance.MapID[4] = Convert.ToUInt32(MapID4.Text);
            RobotConfig.Instance.MinMapX = Convert.ToUInt32(minMapXText.Text);
            RobotConfig.Instance.MinMapY = Convert.ToUInt32(minMapYText.Text);
            RobotConfig.Instance.MaxMapX = Convert.ToUInt32(maxMapXText.Text);
            RobotConfig.Instance.MaxMapY = Convert.ToUInt32(maxMapYText.Text);
            //RobotConfig.Instance.LoginSpeed = Convert.ToDouble(loginSpeedText.Text);
        }

        private void updateFormData()
        {
            platformIDText.Text = RobotConfig.Instance.PlatformID.ToString();
            loginServerList.SelectedIndex = -1;
            loginServerList.Items.Clear();
            foreach (var item in RobotConfig.Instance.LoginServers)
            {
                loginServerList.Items.Add(item);
            }
            loginServerList.SelectedIndex = RobotConfig.Instance.LoginServerSelectedIndex;
            autoLoadCheck.Checked = RobotConfig.Instance.AutoLoadResource;
            randomFightCheck.Checked = RobotConfig.Instance.EnableRandomFight;
            assetRootText.Text = RobotConfig.Instance.AssetRoot;
            robotSignKeyText.Text = RobotConfig.Instance.RobotSignKey;
            accountPreText.Text = RobotConfig.Instance.AccountPre;
            robotNumText.Text = RobotConfig.Instance.RobotNum.ToString();
            minMapXText.Text = RobotConfig.Instance.MinMapX.ToString();
            minMapYText.Text = RobotConfig.Instance.MinMapY.ToString();
            maxMapXText.Text = RobotConfig.Instance.MaxMapX.ToString();
            maxMapYText.Text = RobotConfig.Instance.MaxMapY.ToString();
            //loginSpeedText.Text = RobotConfig.Instance.LoginSpeed.ToString();
            MapID0.Text = RobotConfig.Instance.MapID[0].ToString();
            MapID1.Text = RobotConfig.Instance.MapID[1].ToString();
            MapID2.Text = RobotConfig.Instance.MapID[2].ToString();
            MapID3.Text = RobotConfig.Instance.MapID[3].ToString();
            MapID4.Text = RobotConfig.Instance.MapID[4].ToString();
        }

        #endregion

        public bool LoadResource(bool reload)
        {
            try
            {
                byte[] tempBytes;
                string url = string.Empty;
                string fileName = string.Empty;

                fileName = Path.Combine(assetRoot, "MapList.dat");
                url = Path.Combine(RobotConfig.Instance.AssetRoot, "MapList.dat");
                MapDeployMgr.Clear();
                if (reload)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    Directory.CreateDirectory(assetRoot);
                    tempBytes = FileUtil.ReadWebFile(url, fileName);
                }
                else
                {
                    tempBytes = FileUtil.ReadFileBytes(fileName);
                }

                if (tempBytes.Length != 0)
                {
                    if (MapDeployMgr.decodeMapDeployList(new ByteArray(tempBytes, tempBytes.Length)) == false)
                    {
                        MessageBox.Show("地图数据解析失败!");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("地图数据加载失败!");
                    return false;
                }

                fileName = Path.Combine(assetRoot, "TplList.dat");
                url = Path.Combine(RobotConfig.Instance.AssetRoot, "TplList.dat");
                TplMgr.Clear();
                if (reload)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                    Directory.CreateDirectory(assetRoot);
                    tempBytes = FileUtil.ReadWebFile(url, fileName);
                }
                else
                {
                    tempBytes = FileUtil.ReadFileBytes(fileName);
                }
                //tempBytes = FileUtil.ReadFileBytes(fileName);
                if (tempBytes.Length != 0)
                {
                    if (TplMgr.ParseByteArray(tempBytes))
                    {
                        while (TplMgr.ParseDone == false)
                        {
                            TplMgr.Update();
                        }
                    }
                    else
                    {
                        MessageBox.Show("模板数据解析失败!");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("模板数据加载失败!");
                    return false;
                }

                Global.MaxNameNum = Templates.NameTemplates.FindAll().Count;

                List<int> assetIDList = new List<int>();
                MapDeploy[] oMapDeployArray = MapDeployMgr.getMapDeploy();
                foreach (var item in oMapDeployArray)
                {
                    if (assetIDList.IndexOf(item.assetID) == -1)
                    {
                        assetIDList.Add(item.assetID);
                    }
                }

                MapAssetMgr.Clear();
                ByteArray fileBytes = new ByteArray();
                MapData oMapData = new MapData();
                MapAssetIO oMapAssetIO = new MapAssetIO();
                foreach (var item in assetIDList)
                {
                    fileName = Path.Combine(assetRoot, string.Format("MiniMap\\{0}.jpg", item));
                    url = Path.Combine(RobotConfig.Instance.AssetRoot, string.Format("Map/{0}/mini.jpg", item));
                    if (reload)
                    {
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        Directory.CreateDirectory(assetRoot);
                        tempBytes = FileUtil.ReadWebFile(url, fileName);
                    }
                    else
                    {
                        tempBytes = FileUtil.ReadFileBytes(fileName);
                    }
                    if (tempBytes.Length == 0)
                    {
                        Log("资源读取失败!\r\n{0}\r\n{1}", url, fileName);
                        continue;
                    }
                    fileBytes.WrapBuffer(tempBytes, tempBytes.Length);
                    oMapAssetIO.DetachAndDecode(fileBytes, oMapData);

                    MapAsset oMapAsset = new MapAsset();
                    oMapAsset.copyFrom(oMapData);
                    MapAssetMgr.AddMapAsset(oMapAsset);
                }

                Global.ResourceLoaded = true;

                return true;
            }
            catch (Exception ex)
            {
                Log(ex.StackTrace);
                return false;
            }
        }

        public void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);
            lock (logTextLocker)
            {
                logTextList.Add(msg);
                if (logTextList.Count > 1000)
                {
                    logTextList.RemoveRange(0, logTextList.Count - 1000);
                }
            }
            logTextChange = true;
        }

        private void OnRobotCtrlStop(RobotCtrl oRobotCtrl)
        {
            lock (Global.RobotCtrlListLocker)
            {
                Global.RobotCtrlList.Remove(oRobotCtrl);
            }
            
            Log("{0}停止运行!", oRobotCtrl.RobotKey);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (logTextChange)
            {
                lock (logTextLocker)
                {
                    logTextChange = false;

                    logText.Text = string.Join("\r\n", logTextList.ToArray());
                    logText.Select(logText.Text.Length, 0);
                    logText.ScrollToCaret();

                    int count = 0;
                    for (int i = 0; i < Global.RobotCtrlList.Count; i++)
                    {
                        if (Global.RobotCtrlList[i].LoginCompleted)
                        {
                            count++;
                        }
                    }

                    loginedText.Text = count.ToString();

                    //if (Global.RobotCtrlList.Count == 0)
                    //{
                    //    timer1.Stop();
                    //}

                    if(Global.Started && Global.RobotCtrlList.Count + RobotConfig.Instance.LoginSpeed < RobotConfig.Instance.RobotNum)
                    {
                        for (int i = 0; i < RobotConfig.Instance.LoginSpeed; i++)
                        {
                            Global.CreateRobot();
                        }
                    }
                }
            }
        }

        private void loginServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int serverID = loginServerList.SelectedIndex + 1;
            serverIDText.Text = serverID.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (Global.RobotCtrlList.Count != 0)
            {
                MessageBox.Show("已启动！");
                return;
            }

            if (loginServerList.SelectedIndex == -1 || string.IsNullOrEmpty(loginServerList.Text))
            {
                MessageBox.Show("未选择登陆服务器!");
                return;
            }

            if (Global.ResourceLoaded == false)
            {
                MessageBox.Show("资源未加载，请先加载资源!");
                return;
            }

            string loginIP = loginServerList.Text.Split(':')[0];
            int loginPort = int.Parse(loginServerList.Text.Split(':')[1]);

            bool serverOpened = false;
            RobotClient oRobotClient = new RobotClient();
            if (oRobotClient.Connect(loginIP, loginPort))
            {
                serverOpened = true;
                oRobotClient.Close();
            }

            if (serverOpened == false)
            {
                MessageBox.Show("远程服务器未启动!");
                return;
            }

            linkLabel1_LinkClicked(null, null);

            updateConfig();
            RobotConfig.Save();

            Global.Main.Log("=========================================");

            Global.Started = true;
            Global.RobotCtrlList.Clear();
            Global.ErrorRobotCtrlList.Clear();

            ushort platformID = (ushort)RobotConfig.Instance.PlatformID;
            ushort serverID = (ushort)(loginServerList.SelectedIndex + 1);

            Global.LoginIP = loginIP;
            Global.LoginPort = loginPort;
            Global.PlatformID = platformID;
            Global.ServerID = serverID;
            Global.PreAccount = accountPreText.Text;
            Global.RobotCtrlStop = OnRobotCtrlStop;

            Global.RobotID = 0;
            Global.CreateRobot();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Global.StopRunTask())
            {
                MessageBox.Show("已停止机器人上线，等待机器人完成退出，再次点击停止强制所有机器人下线!");
                return;
            }
            if (Global.RobotCtrlList.Count == 0)
            {
                MessageBox.Show("已全部停止!");
                return;
            }

            Global.Started = false;
            lock (Global.RobotCtrlListLocker)
            {
                foreach (var item in Global.RobotCtrlList)
                {
                    item.CloseSocket();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool reload = MessageBox.Show("是否从资源服下载最新资源?", "更新", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes;

            if (reload == false && Directory.Exists(assetRoot))
            {
                MessageBox.Show("请选择从资源服下载最新资源!");
                return;
            }

            if (LoadResource(reload))
            {
                MessageBox.Show("资源加载成功!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateConfig();
            RobotConfig.Save();
            MessageBox.Show("保存成功!");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lock (logTextLocker)
            {
                logTextList.Clear();
            }
            logTextChange = true;
        }

        private void randomFightCheck_CheckedChanged(object sender, EventArgs e)
        {
            RobotConfig.Instance.EnableRandomFight = randomFightCheck.Checked;
            if (randomFightCheck.Checked)
            {
                //
            }
        }
    }
}
