using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapNet.Util
{
    internal static class VlqDecoder
    {
        private const byte VLQ_BASE_SHIFT = 5;
        private const byte VLQ_BASE = 1 << VLQ_BASE_SHIFT;
        private const byte VLQ_BASE_MASK = VLQ_BASE - 1;
        private const byte VLQ_CONTINUATION_BIT = VLQ_BASE;

        public static int Decode(ref string encoded)
        {
            int result = 0;
            var encodedLength = encoded.Length;
            var encodedIdx = 0;
            var isContinued = true;
            var shift = 0;

            do
            {
                if (encodedIdx >= encodedLength)
                {
                    throw new ArgumentException("Too few characters in supplied encoded value");
                }

                var b = Base64.Decode(encoded[encodedIdx++]);
                isContinued = (b & VLQ_CONTINUATION_BIT) == VLQ_CONTINUATION_BIT;

                var digit = b & VLQ_BASE_MASK;
                result = result + (digit << shift);
                shift += VLQ_BASE_SHIFT;
            }
            while (isContinued);

            encoded = encoded.Substring(encodedIdx);
            return FromVlqSigned(result);
        }

        private static int FromVlqSigned(int signedValue)
        {
            var isNegative = ((signedValue & 0x1) == 0x1);
            var shifted = signedValue >> 1;

            return isNegative ? -shifted : shifted;
        }
    }
}
