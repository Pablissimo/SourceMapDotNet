using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapNet.Util
{
    public static class VlqDecoder
    {
        private const byte MASK_CONTINUATION_BIT = 1 << 5;
        private const byte MASK_VALUE = 0x1F;

        // Essentially a port of
        // https://github.com/mozilla/source-map/blob/master/lib/source-map/base64-vlq.js
        public static int Decode(ref string encoded)
        {
            int toReturn = 0;
            var idx = 0;
            var isContinued = true;
            var shift = 0;

            do
            {
                if (idx > encoded.Length)
                {
                    throw new ArgumentException("Too few characters in supplied encoded value");
                }

                var b = Base64.Decode(encoded[idx++]);
                var value = b & MASK_VALUE;
                toReturn = toReturn + (value << shift);
                shift += 5;
                isContinued = (b & MASK_CONTINUATION_BIT) == MASK_CONTINUATION_BIT;
            }
            while (isContinued);

            encoded = encoded.Substring(idx);
            
            if ((toReturn & 0x1) == 0x1)
            {
                return -1 * (toReturn >> 1);
            }
            else
            {
                return toReturn >> 1;
            }
        }
    }
}
