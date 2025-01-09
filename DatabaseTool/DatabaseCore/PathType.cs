using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    public enum PathType
    {
        /// <summary>
        /// 不需要此路径
        /// </summary>
        None,
        /// <summary>
        /// 使用文件路径
        /// </summary>
        FilePath,
        /// <summary>
        /// 使用文件夹路径
        /// </summary>
        FolderPath
    }
}
