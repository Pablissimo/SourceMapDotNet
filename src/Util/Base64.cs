using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Util
{
    public static class Base64
    {
        private static Dictionary<char, byte> _decodeTable = GetDecodeTable();

        private static Dictionary<char, byte> GetDecodeTable()
        {
            var table = new Dictionary<char, byte>();

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            for (var idx = 0; idx < chars.Length; idx++)
            {
                table[chars[idx]] = (byte)idx;
            }

            return table;
        }

        public static byte Decode(char base64char)
        {
            return _decodeTable[base64char];
        }
    }
}
