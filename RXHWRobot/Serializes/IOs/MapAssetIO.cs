using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Serializes.Chunks;

namespace RXHWRobot.Serializes.IOs
{
    public class MapAssetIO : RpgDataIO
    {
        public MapAssetIO()
        {
            addChunkIO(new FileStartChunkIO());
            addChunkIO(new MapPropChunkIO());
            addChunkIO(new MapBlockChunkIO());
            addChunkIO(new MapShadeChunkIO());
            addChunkIO(new FileEndChunkIO());
        }

        public override string Flag
        {
            get
            {
                return "MapData";
            }
        }
    }
}
