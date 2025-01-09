using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot
{
    public enum ServerType
    {
        CacheServerType = 1,
        WorldServerType = 2,
        MapServerType = 4,
        GatewayServerType = 8,
        LoginServerType = 16,
        ServiceWarServerType = 32,
        PlayerClientType = 64,
        ServerWarMapServerType = 128,
        CenterServerType = 256
    }
}
