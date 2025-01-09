using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSScriptApp.TemplateCore
{
    public enum GeneratorType
    {
        /// <summary>
        /// C++服务端
        /// </summary>
        CPPServer = 1,
        /// <summary>
        /// AS3客户端
        /// </summary>
        AS3Client = 2,
        /// <summary>
        /// Unity3D客户端
        /// </summary>
        Unity3DClient = 3
    }
}
