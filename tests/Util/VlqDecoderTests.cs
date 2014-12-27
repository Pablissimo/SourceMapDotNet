using NUnit.Framework;
using SourceMapDotNet.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapDotNetTests.Util
{
    [TestFixture]
    public class VlqDecoderTests
    {
        [Test]
        public void Decode_ThrowsArgumentException_IfNoCharsInInputString()
        {
            string empty = string.Empty;
            Assert.Throws(typeof(ArgumentException), () => VlqDecoder.Decode(ref empty));
        }

        [Test]
        public void Decode_ThrowsArgumentException_IfContinuationTokenButNoRemainingChars()
        {
            var encoded = Base64.Encode(63).ToString();
            Assert.Throws(typeof(ArgumentException), () => VlqDecoder.Decode(ref encoded));
        }

        [Test]
        public void Decode_HandlesSinglePositiveValues()
        {
            var encoded = Base64.Encode(StructureByte(12)).ToString();
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(12));
            Assert.That(encoded, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Decode_HandlesSingleZero()
        {
            var encoded = Base64.Encode(StructureByte(0)).ToString();
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(0));
            Assert.That(encoded, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Decode_HandlesNegativeZero()
        {
            var encoded = Base64.Encode((byte) (StructureByte(0) | 0x1)).ToString();
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(0));
            Assert.That(encoded, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Decode_HandlesMultiDigitPositiveValues()
        {
            // Example encoded value from http://qfox.nl/weblog/281
            var encoded = "6rk2B";
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(886973));
        }

        [Test]
        public void Decode_HandlesMultiDigitNegativeValues()
        {
            var encoded = "xhsuC";
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(-1284120));
        }

        [Test]
        public void Decode_AmendsInputStringForUnprocessedTokens()
        {
            var encoded = "6rk2BxhsuC";
            Assert.That(VlqDecoder.Decode(ref encoded), Is.EqualTo(886973));
            Assert.That(encoded, Is.EqualTo("xhsuC"));
        }

        private byte StructureByte(sbyte value)
        {
            if (value < 0)
            {
                return (byte) ((-value << 1) | 0x1);
            }
            else
            {
                return (byte) (value << 1);
            }
        }
    }
}
