using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RXHWRobot.Serializes.Chunks;

namespace RXHWRobot.Serializes.IOs
{
    public class MapDataIO : RpgDataIO
    {
        public MapDataIO()
        {
            addChunkIO(new FileStartChunkIO());
            addChunkIO(new MapPropChunkIO());
            addChunkIO(new MapBlockChunkIO());
            addChunkIO(new MapShadeChunkIO());
            addChunkIO(new MapNPCChunkIO());
            addChunkIO(new MapEventChunkIO());
            addChunkIO(new MapEffectChunkIO());
            addChunkIO(new MapMonsterChunkIO());
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
