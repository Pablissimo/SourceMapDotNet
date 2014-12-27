using NUnit.Framework;
using SourceMapDotNet.Model;
using SourceMapDotNet.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceMapDotNetTests.Util.Comparers
{
    [TestFixture]
    public class SourceReferenceEqualityComparerTests
    {
        [Test]
        public void GetHashCode_ReturnsSameHashCodes_ForInstancesWithSameLineNumberAndFile()
        {
            var ref1 = new SourceReference 
            {
                File = "File1",
                LineNumber = 12
            };

            var ref2 = new SourceReference
            {
                File = "File1",
                LineNumber = 12
            }; 
            
            var comparer = new SourceReferenceEqualityComparer();

            Assert.That(comparer.GetHashCode(ref1), Is.EqualTo(comparer.GetHashCode(ref2)));
        }

        [Test]
        public void Equals_ReturnsTrue_ForEquivalentInstances()
        {
            var ref1 = new SourceReference
            {
                File = "File1",
                LineNumber = 12
            };

            var ref2 = new SourceReference
            {
                File = "File1",
                LineNumber = 12
            };

            var comparer = new SourceReferenceEqualityComparer();

            Assert.That(comparer.Equals(ref1, ref2));
        }

        [Test]
        public void Equals_ReturnsFalse_ForReferencesWithDifferentFilenames()
        {
            var ref1 = new SourceReference
            {
                File = "File1",
                LineNumber = 12
            };

            var ref2 = new SourceReference
            {
                File = "File2",
                LineNumber = 12
            };

            var comparer = new SourceReferenceEqualityComparer();

            Assert.That(comparer.Equals(ref1, ref2), Is.False);
        }

        [Test]
        public void Equals_ReturnsFalse_ForReferencesWithDifferentLineNumbers()
        {
            var ref1 = new SourceReference
            {
                File = "File1",
                LineNumber = 12
            };

            var ref2 = new SourceReference
            {
                File = "File1",
                LineNumber = 82
            };

            var comparer = new SourceReferenceEqualityComparer();

            Assert.That(comparer.Equals(ref1, ref2), Is.False);
        }
    }
}
