using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Chunks.MapChunks
{
    public class MapDataIO : FileDataIO
    {
        public MapDataIO()
        {
            AddChunkIO(new FileStartChunkIO());
            AddChunkIO(new MapPropChunkIO());
            AddChunkIO(new MapBlockChunkIO());
            AddChunkIO(new FileEndChunkIO());
        }
    }
}
