using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using zlib;

namespace UnityLight.Utils
{
    public class Util
    {
        public static void Dispose(IDisposable obj)
        {
            if (obj == null) return;
            obj.Dispose();
        }

        public static byte[] CompressZLib(byte[] data)
        {
            return CompressZLib(data, 0, data.Length);
        }

        public static byte[] CompressZLib(byte[] data, int offset, int length)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (ZOutputStream zOut = new ZOutputStream(stream, zlibConst.Z_BEST_COMPRESSION))
                    {
                        zOut.Write(data, offset, length);
                        zOut.finish();
                        return stream.ToArray();
                    }
                }
            }
            catch// (Exception ex)
            {
                return new byte[0];
            }
        }

        public static byte[] UncompressZLib(byte[] data)
        {
            return UncompressZLib(data, 0, data.Length);
        }

        public static byte[] UncompressZLib(byte[] data, int offset, int length)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (ZOutputStream zOut = new ZOutputStream(stream))
                    {
                        zOut.Write(data, offset, length);
                        return stream.ToArray();
                    }
                }
            }
            catch// (Exception ex)
            {
                return new byte[0];
            }
        }
    }
}
