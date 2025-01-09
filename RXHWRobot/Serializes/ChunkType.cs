using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Serializes
{
    public enum ChunkType
    {
        FILE_START = 0xA0,
        FILE_LAST = 0xAF,

        MAP_PROP = 0xB0,
        MAP_BLOCK = 0xB1,
        MAP_SHADE = 0xB2,
        MAP_NPC = 0xB3,
        MAP_EVENT = 0xB6,
        MAP_EFFECT = 0xB5,
        MAP_MONSTER = 0xB4,
        MAP_INFO = 0xB7
    }
}
