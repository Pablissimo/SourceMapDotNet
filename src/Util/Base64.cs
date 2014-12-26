using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Util
{
    internal static class Base64
    {
        private const string BASE64_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        private static Dictionary<char, byte> _decodeTable = GetDecodeTable();

        private static Dictionary<char, byte> GetDecodeTable()
        {
            var table = new Dictionary<char, byte>();

            for (var idx = 0; idx < BASE64_ALPHABET.Length; idx++)
            {
                table[BASE64_ALPHABET[idx]] = (byte)idx;
            }

            return table;
        }

        public static byte Decode(char value)
        {
            byte toReturn;
            if (!_decodeTable.TryGetValue(value, out toReturn))
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return toReturn;
        }

        public static char Encode(byte value)
        {
            if (value >= BASE64_ALPHABET.Length)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            return BASE64_ALPHABET[value];
        }
    }
}
