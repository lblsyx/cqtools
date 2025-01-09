using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RXHWRobot.Datas
{
    public class MapObject
    {
        public ObjectGuidInfo ObjectID = new ObjectGuidInfo();

        public ObjectGuidInfo OwnerID = new ObjectGuidInfo();

        public uint Level;

        public uint MapID;

        public uint MapX;

        public uint MapY;

        public uint Camp;
    }
}
