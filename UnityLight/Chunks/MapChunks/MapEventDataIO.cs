using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityExt.Chunks.MapChunks
{
    public class MapEventDataIO : FileDataIO
    {
        public MapEventDataIO()
        {
            AddChunkIO(new FileStartChunkIO());
            AddChunkIO(new MapEventChunkIO());
            AddChunkIO(new FileEndChunkIO());
        }
    }
}
