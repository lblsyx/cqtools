using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using CSScriptApp.ProtocolCore.Generates;

namespace CSScriptApp.ProtocolCore.GenProtocols
{
    public class MySQLUtil
    {
        #region 协议编辑相关

        public static bool ProjectExists(string sProjectName)
        {
            string sql = "SELECT `ProjectID` FROM `projects` WHERE `ProjectName`=@ProjectName AND `IsDelete`=0";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectName", sProjectName)
            };

            int ProjectID = Convert.ToInt32(DBHelper.ExecuteScalar(sql, oParams));

            return ProjectID != 0;
        }

        public static long AddProject(ProjectInfo oProjectInfo)
        {
            if (oProjectInfo == null) return 0;

            string sql = "INSERT INTO `projects` (`ProjectName`, `PacketVer`, `ProjectSummary`, `IsDelete`) VALUES (@ProjectName, @PacketVer, @ProjectSummary, 0)";
            MySqlParameter[] pArray = new MySqlParameter[] {
                new MySqlParameter("@ProjectName", oProjectInfo.ProjectName),
                new MySqlParameter("@PacketVer", oProjectInfo.PacketVer),
                new MySqlParameter("@ProjectSummary", oProjectInfo.ProjectSummary)
            };
            long id = DBHelper.ExecuteLastID(sql, pArray);

            return id;
        }

        public static bool UpdateProject(ProjectInfo oProjectInfo)
        {
            if (oProjectInfo == null) return false;

            string sql = "UPDATE `projects` SET `ProjectName`=@ProjectName, `PacketVer`=@PacketVer, `ProjectSummary`=@ProjectSummary WHERE `ProjectID` = @ProjectID";
            MySqlParameter[] pArray = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID),
                new MySqlParameter("@ProjectName", oProjectInfo.ProjectName),
                new MySqlParameter("@PacketVer", oProjectInfo.PacketVer),
                new MySqlParameter("@ProjectSummary", oProjectInfo.ProjectSummary)
            };

            int rows = DBHelper.ExecuteSql(sql, pArray);

            return rows != 0;
        }

        public static bool DelProject(ProjectInfo oProjectInfo)
        {
            if (oProjectInfo == null) return false;

            string sql = "UPDATE `projects` SET `IsDelete`=1 WHERE `ProjectID` = @ProjectID";
            MySqlParameter[] pArray = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID)
            };

            int rows = DBHelper.ExecuteSql(sql, pArray);

            return rows != 0;
        }

        public static void ReloadProjectList(List<ProjectInfo> list)
        {
            string sql = "SELECT `ProjectID`,`ProjectName`,`ProjectSummary`,`PacketVer` FROM `projects` WHERE `IsDelete`=0";

            DataSet ds = DBHelper.Query(sql);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    ProjectInfo oProjectInfo = new ProjectInfo();
                    oProjectInfo.ProjectID = Convert.ToInt32(dr[0]);
                    oProjectInfo.ProjectName = Convert.ToString(dr[1]);
                    oProjectInfo.PacketVer = Convert.ToUInt16(dr[3]);
                    oProjectInfo.ProjectSummary = Convert.ToString(dr[2]);
                    list.Add(oProjectInfo);
                }
            }
        }

        public static bool StructExists(int nProjectID, string sStructName)
        {
            string sql = "SELECT `StructID` FROM `structs` WHERE `StructName`=@StructName AND `ProjectID`=@ProjectID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructName", sStructName),
                new MySqlParameter("@ProjectID", nProjectID)
            };

            int StructID = Convert.ToInt32(DBHelper.ExecuteScalar(sql, oParams));

            return StructID != 0;
        }

        public static long AddStruct(ProjectInfo oProjectInfo, StructInfo oStructInfo)
        {
            oStructInfo.ProjectID = oProjectInfo.ProjectID;

            string sql = "INSERT INTO `structs` (`StructName`, `StructSummary`, `StructFields`, `ProjectID`, `SortIndex`, `LockerName`) VALUES (@StructName,@StructSummary,@StructFields,@ProjectID,@SortIndex,'')";

            string fieldsStr = XMLUtil.Serialize(oStructInfo);
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructName", oStructInfo.StructName),
                new MySqlParameter("@StructSummary", oStructInfo.StructSummary),
                new MySqlParameter("@StructFields", fieldsStr),
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID),
                new MySqlParameter("@SortIndex", oStructInfo.SortIndex)
            };

            long id = DBHelper.ExecuteLastID(sql, oParams);
            oStructInfo.StructID = (int)id;
            return id;
        }

        public static bool UpdateStruct(StructInfo oStructInfo)
        {
            string sql = "UPDATE `structs` SET `StructName`=@StructName, `StructSummary`=@StructSummary, `StructFields`=@StructFields, `SortIndex`=@SortIndex WHERE StructID=@StructID";

            string fieldsStr = XMLUtil.Serialize(oStructInfo);
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID),
                new MySqlParameter("@StructName", oStructInfo.StructName),
                new MySqlParameter("@StructSummary", oStructInfo.StructSummary),
                new MySqlParameter("@StructFields", fieldsStr),
                new MySqlParameter("@SortIndex", oStructInfo.SortIndex)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static bool DelStruct(StructInfo oStructInfo)
        {
            string sql = "DELETE FROM `structs` WHERE `StructID`=@StructID";
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static void LoadStructFields(StructInfo oStructInfo)
        {
            string sql = "SELECT `StructFields` FROM `structs` WHERE `StructID`=@StructID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string fieldsStr = Convert.ToString(dr[0]);

                StructInfo si = XMLUtil.Deserialize<StructInfo>(fieldsStr);

                for (int j = 0; j < si.Fields.Count; j++)
                {
                    oStructInfo.Fields.Add(si.Fields[j].Clone() as FieldInfo);
                }
            }
        }

        public static string GetStructLocker(StructInfo oStructInfo)
        {
            string sql = "SELECT `LockerName` FROM `structs` WHERE `StructID`=@StructID";
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID)
            };

            object ret = DBHelper.ExecuteScalar(sql, oParams);

            if (ret == null) return null;

            return Convert.ToString(ret);
        }

        public static bool UpdateStructLocker(StructInfo oStructInfo, string sName)
        {
            string sql = "UPDATE `structs` SET `LockerName`=@LockerName WHERE StructID=@StructID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID),
                new MySqlParameter("@LockerName", sName)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static void ClearStructLocker(StructInfo oStructInfo, string sName)
        {
            string sql = "UPDATE `structs` SET `LockerName`='' WHERE StructID=@StructID AND `LockerName`=@LockerName";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructID", oStructInfo.StructID),
                new MySqlParameter("@LockerName", sName)
            };

            DBHelper.ExecuteSql(sql, oParams);
        }

        public static void ReloadStructList(ProjectInfo oProjectInfo, IList<StructInfo> list)
        {
            string sql = "SELECT `StructID`,`StructName`,`StructSummary`, `StructFields`, `SortIndex`, `ProjectID` FROM `structs` WHERE `ProjectID`=@ProjectID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    string fieldsStr = Convert.ToString(dr[3]);

                    StructInfo oStructInfo = XMLUtil.Deserialize<StructInfo>(fieldsStr);

                    oStructInfo.StructID = Convert.ToInt32(dr[0]);
                    oStructInfo.StructName = Convert.ToString(dr[1]);
                    oStructInfo.StructSummary = Convert.ToString(dr[2]);
                    oStructInfo.SortIndex = Convert.ToInt32(dr[4]);
                    oStructInfo.ProjectID = Convert.ToInt32(dr[5]);

                    list.Add(oStructInfo);
                }
            }
        }

        public static bool ProtocolExists(int nProjectID, string sProtocolName)
        {
            string sql = "SELECT `ProtocolID` FROM `protocols` WHERE `ProtocolName`=@StructName AND `ProjectID`=@ProjectID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@StructName", sProtocolName),
                new MySqlParameter("@ProjectID", nProjectID)
            };

            int ProtocolID = Convert.ToInt32(DBHelper.ExecuteScalar(sql, oParams));

            return ProtocolID != 0;
        }

        public static long AddProtocol(ProjectInfo oProjectInfo, ProtocolInfo oProtocolInfo)
        {
            oProtocolInfo.ProjectID = oProjectInfo.ProjectID;

            string sql = "INSERT INTO `protocols` (`ProtocolName`, `ProtocolSummary`, `ProtocolFields`, `ProjectID`, `SortIndex`, `LockerName`) VALUES (@ProtocolName, @ProtocolSummary, @ProtocolFields, @ProjectID, @SortIndex, '')";

            string fieldsStr = XMLUtil.Serialize(oProtocolInfo);
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolName", oProtocolInfo.ProtocolName),
                new MySqlParameter("@ProtocolSummary", oProtocolInfo.ProtocolSummary),
                new MySqlParameter("@ProtocolFields", fieldsStr),
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID),
                new MySqlParameter("@SortIndex", oProtocolInfo.SortIndex)
            };

            long id = DBHelper.ExecuteLastID(sql, oParams);
            oProtocolInfo.ProtocolID = (int)id;
            return id;
        }

        public static bool UpdateProtocol(ProtocolInfo oProtocolInfo)
        {
            string sql = "UPDATE `protocols` SET `ProtocolName`=@ProtocolName, `ProtocolSummary`=@ProtocolSummary, `ProtocolFields`=@ProtocolFields, `SortIndex`=@SortIndex WHERE ProtocolID=@ProtocolID";

            string fieldsStr = XMLUtil.Serialize(oProtocolInfo);
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID),
                new MySqlParameter("@ProtocolName", oProtocolInfo.ProtocolName),
                new MySqlParameter("@ProtocolSummary", oProtocolInfo.ProtocolSummary),
                new MySqlParameter("@ProtocolFields", fieldsStr),
                new MySqlParameter("@SortIndex", oProtocolInfo.SortIndex)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static bool DelProtocol(ProtocolInfo oProtocolInfo)
        {
            string sql = "DELETE FROM `protocols` WHERE `ProtocolID`=@ProtocolID";
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static void LoadProtocolFields(ProtocolInfo oProtocolInfo)
        {
            string sql = "SELECT `ProtocolFields` FROM `protocols` WHERE `ProtocolID`=@ProtocolID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string fieldsStr = Convert.ToString(dr[0]);

                ProtocolInfo pi = XMLUtil.Deserialize<ProtocolInfo>(fieldsStr);

                for (int i = 0; i < pi.ReqFields.Count; i++)
                {
                    oProtocolInfo.ReqFields.Add(pi.ReqFields[i].Clone() as FieldInfo);
                }

                for (int i = 0; i < pi.ResFields.Count; i++)
                {
                    oProtocolInfo.ResFields.Add(pi.ResFields[i].Clone() as FieldInfo);
                }
            }
        }

        public static string GetProtocolLocker(ProtocolInfo oProtocolInfo)
        {
            string sql = "SELECT `LockerName` FROM `protocols` WHERE `ProtocolID`=@ProtocolID";
            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID)
            };

            object ret = DBHelper.ExecuteScalar(sql, oParams);

            if (ret == null) return null;

            return Convert.ToString(ret);
        }

        public static bool UpdateProtocolLocker(ProtocolInfo oProtocolInfo, string sName)
        {
            string sql = "UPDATE `protocols` SET `LockerName`=@LockerName WHERE ProtocolID=@ProtocolID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID),
                new MySqlParameter("@LockerName", sName)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static void ClearProtocolLocker(ProtocolInfo oProtocolInfo, string sName)
        {
            string sql = "UPDATE `protocols` SET `LockerName`='' WHERE ProtocolID=@ProtocolID AND `LockerName`=@LockerName";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProtocolID", oProtocolInfo.ProtocolID),
                new MySqlParameter("@LockerName", sName)
            };

            DBHelper.ExecuteSql(sql, oParams);
        }

        public static void ReloadProtocolList(ProjectInfo oProjectInfo, IList<ProtocolInfo> list)
        {
            string sql = "SELECT `ProtocolID`,`ProtocolName`,`ProtocolSummary`, `ProtocolFields`, `SortIndex`, `ProjectID` FROM `protocols` WHERE `ProjectID`=@ProjectID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    string fieldsStr = Convert.ToString(dr[3]);

                    ProtocolInfo oProtocolInfo = XMLUtil.Deserialize<ProtocolInfo>(fieldsStr);
                    
                    oProtocolInfo.ProtocolID = Convert.ToInt32(dr[0]);
                    oProtocolInfo.ProtocolName = Convert.ToString(dr[1]);
                    oProtocolInfo.ProtocolSummary = Convert.ToString(dr[2]);
                    oProtocolInfo.SortIndex = Convert.ToInt32(dr[4]);
                    oProtocolInfo.ProjectID = Convert.ToInt32(dr[5]);

                    list.Add(oProtocolInfo);
                }
            }
        }

        #endregion

        #region 生成代码相关

        public static bool CheckGeneratorSetting(ProjectInfo oProjectInfo, string sGeneratorType)
        {
            string sql = "SELECT `GeneratorID` FROM `generators` WHERE `ProjectID`=@ProjectID AND GeneratorType=@GeneratorType";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID),
                new MySqlParameter("@GeneratorType", sGeneratorType)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count != 0) return ds.Tables[0].Rows.Count != 0;

            return false;
        }

        public static long AddGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            string sql = "INSERT INTO `generators` (`GeneratorType`, `ProjectID`, `ContentFormat1`, `ContentFormat2`) VALUES (@GeneratorType, @ProjectID, @ContentFormat1, @ContentFormat2)";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oGeneratorSetting.ProjectID),
                new MySqlParameter("@GeneratorType", oGeneratorSetting.GeneratorType),
                new MySqlParameter("@ContentFormat1", oGeneratorSetting.ContentFormat1),
                new MySqlParameter("@ContentFormat2", oGeneratorSetting.ContentFormat2)
            };

            long id = DBHelper.ExecuteLastID(sql, oParams);

            oGeneratorSetting.GeneratorID = (int)id;

            return id;
        }

        public static bool UpdateGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            string sql = "UPDATE `generators` SET `GeneratorType`=@GeneratorType,`ContentFormat1`=@ContentFormat1, `ContentFormat2`=@ContentFormat2 WHERE `GeneratorID`=@GeneratorID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@GeneratorID", oGeneratorSetting.GeneratorID),
                new MySqlParameter("@GeneratorType", oGeneratorSetting.GeneratorType),
                new MySqlParameter("@ContentFormat1", oGeneratorSetting.ContentFormat1),
                new MySqlParameter("@ContentFormat2", oGeneratorSetting.ContentFormat2)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static bool DelGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            string sql = "DELETE FROM `generators` WHERE `GeneratorID`=@GeneratorID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@GeneratorID", oGeneratorSetting.GeneratorID)
            };

            int rows = DBHelper.ExecuteSql(sql, oParams);

            return rows != 0;
        }

        public static GeneratorSetting LoadGeneratorSetting(ProjectInfo oProjectInfo, string sGeneratorType)
        {
            string sql = "SELECT `GeneratorID`,`GeneratorType`,`ProjectID`,`ContentFormat1`,`ContentFormat2` FROM `generators` WHERE `ProjectID`=@ProjectID AND GeneratorType=@GeneratorType";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID),
                new MySqlParameter("@GeneratorType", sGeneratorType)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables[0].Rows.Count == 0) return null;

                DataRow dr = ds.Tables[0].Rows[0];

                GeneratorSetting oGeneratorSetting = new GeneratorSetting();

                oGeneratorSetting.GeneratorID = Convert.ToInt32(dr[0]);
                oGeneratorSetting.GeneratorType = Convert.ToString(dr[1]);
                oGeneratorSetting.ProjectID = Convert.ToInt32(dr[2]);
                oGeneratorSetting.ContentFormat1 = Convert.ToString(dr[3]);
                oGeneratorSetting.ContentFormat2 = Convert.ToString(dr[4]);

                return oGeneratorSetting;
            }

            return null;
        }

        public static List<GeneratorSetting> LoadGeneratorSetting(ProjectInfo oProjectInfo)
        {
            List<GeneratorSetting> list = new List<GeneratorSetting>();

            string sql = "SELECT `GeneratorID`,`GeneratorType`,`ProjectID`,`ContentFormat1`,`ContentFormat2` FROM `generators` WHERE `ProjectID`=@ProjectID";

            MySqlParameter[] oParams = new MySqlParameter[] {
                new MySqlParameter("@ProjectID", oProjectInfo.ProjectID)
            };

            DataSet ds = DBHelper.Query(sql, oParams);

            if (ds.Tables.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    GeneratorSetting oGeneratorSetting = new GeneratorSetting();
                    oGeneratorSetting.GeneratorID = Convert.ToInt32(dr[0]);
                    oGeneratorSetting.GeneratorType = Convert.ToString(dr[1]);
                    oGeneratorSetting.ProjectID = Convert.ToInt32(dr[2]);
                    oGeneratorSetting.ContentFormat1 = Convert.ToString(dr[3]);
                    oGeneratorSetting.ContentFormat2 = Convert.ToString(dr[4]);
                    list.Add(oGeneratorSetting);
                }
            }

            return list;
        }

        #endregion
    }
}
