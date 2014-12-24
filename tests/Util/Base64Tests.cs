using NUnit.Framework;
using SourceMapNet.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceMapDotNetTests.Util
{
    [TestFixture]
    public class Base64Tests
    {
        [Test]
        public void Decode_ThrowsArgumentOutOfRangeException_IfCharBelowBase64Range()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => Base64.Decode(char.MinValue));
        }

        [Test]
        public void Decode_ThrowsArgumentOutOfRangeException_IfCharAboveBase64Range()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => Base64.Decode(char.MaxValue));
        }

        [Test]
        public void Decode_ReturnsCorrectDecodes()
        {
            char[] BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToArray();
            
            for (var i = 0; i < BASE64_CHARS.Length; i++)
            {
                Assert.That(Base64.Decode(BASE64_CHARS[i]), Is.EqualTo(i));
            }
        }

        [Test]
        public void Encode_ThrowsArgumentOutOfRangeException_IfValueGreaterThanOrEqualTo64()
        {
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => Base64.Encode(64));
        }

        [Test]
        public void Encode_ReturnsCorrectlyEncodedValues()
        {
            char[] BASE64_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToArray();

            for (var i = 0; i < BASE64_CHARS.Length; i++)
            {
                Assert.That(Base64.Encode((byte) i), Is.EqualTo(BASE64_CHARS[i]));
            }
        }
    }
}
