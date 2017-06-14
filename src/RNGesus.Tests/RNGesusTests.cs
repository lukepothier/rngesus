using System;
using System.Linq;
using NUnit.Framework;

namespace Luke.RNG.Tests
{
    /// <summary>
    /// The unit tests for RNGesus.Generator
    /// </summary>
    /// <remarks>
    /// All tests have method names ending with 'ReturnsNoDuplicates' are not ideal unit tests. For each of them, the method they are testing can theoretically
    /// produces valid duplicate results. However, provided the inputs are sensible, it is extremely unlikely that any duplicates returned are valid.
    /// It's much more likely that there has been some sort of implementation error. Therefore, those tests remain useful.
    /// </remarks>
    [TestFixture]
    public class RNGesusTests
    {
        #region Constructor

        [TestCase(1024, 1025)]
        public void Ctor_Throws_AooREx(int bufferSize, int length)
        {
            using (var rngesus = new RNGesus(bufferSize))
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => rngesus.GenerateString(length));
            }
        }

        [TestCase(0, 999)]
        [TestCase(-999, 999)]
        public void Ctor_Works_Undersized_Buffer(int bufferSize, int length)
        {
            using (var rngesus = new RNGesus(bufferSize))
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length));
            }
        }

        #endregion Constructor

        #region Boolean

        [Test]
        public void GenerateBool_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateBool());
            }
        }

        #endregion Boolean

        #region UInt32

        [Test]
        public void GenerateInt_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt());
            }
        }

        [TestCase(1u)]
        [TestCase(2u)]
        [TestCase(uint.MaxValue)]
        public void GenerateInt_Maximum_DoesNotThrow(uint maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(maximum));
            }
        }

        [TestCase(-999999)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(int.MaxValue)]
        public void GenerateInt_Maximum_DoesNotThrow(int maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(maximum));
            }
        }

        [TestCase(uint.MinValue, uint.MaxValue)]
        [TestCase(0u, 2u)]
        [TestCase(99u, 999u)]
        public void GenerateInt_Minimum_Maximum_DoesNotThrow(uint minimum, uint maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(minimum, maximum));
            }
        }

        [TestCase(-999999, int.MaxValue)]
        [TestCase(-999999, 0)]
        [TestCase(0, int.MaxValue)]
        [TestCase(0, 2)]
        [TestCase(99, 999)]
        public void GenerateInt_Minimum_Maximum_DoesNotThrow(int minimum, int maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(minimum, maximum));
            }
        }

        [TestCase(uint.MaxValue, uint.MinValue)]
        [TestCase(2u, 1u)]
        public void GenerateInt_Minimum_Maximum_Throws_AooREx(uint minimum, uint maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => rngesus.GenerateInt(minimum, maximum));
            }
        }

        [TestCase(uint.MaxValue, uint.MaxValue)]
        [TestCase(1u, 1u)]
        [TestCase(0u, 0u)]
        public void GenerateInt_Minimum_Maximum_Throws_AEx(uint minimum, uint maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateInt(minimum, maximum));
            }
        }

        [TestCase(-999999, -999999)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(1, 1)]
        public void GenerateInt_Minimum_Maximum_Throws_AEx(int minimum, int maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateInt(minimum, maximum));
            }
        }

        [Test]
        public void GenerateInt_ReturnsNoDuplicates()
        {
            var results = new int[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateInt();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateInt_Maximum_ReturnsNoDuplicates()
        {
            var results = new int[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateInt(999999);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateInt_MinimumAndMaximum_ReturnsNoDuplicates()
        {
            var results = new int[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateInt(999, 999999);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateInt_MinimumAndMaximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateInt(999, 999999);

                Assert.That(() => result >= 999);
                Assert.That(() => result <= 999999);
            }
        }

        #endregion UInt32

        #region UInt64

        [Test]
        public void GenerateLong_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong());
            }
        }

        [TestCase(1ul)]
        [TestCase(2ul)]
        [TestCase(ulong.MaxValue)]
        public void GenerateLong_Maximum_DoesNotThrow(ulong maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(maximum));
            }
        }

        [TestCase(-999999999)]
        [TestCase(-2)]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(long.MaxValue)]
        public void GenerateLong_Maximum_DoesNotThrow(long maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(maximum));
            }
        }

        [TestCase(ulong.MinValue, ulong.MaxValue)]
        [TestCase(0ul, 2ul)]
        [TestCase(99ul, 999ul)]
        public void GenerateLong_Minimum_Maximum_DoesNotThrow(ulong minimum, ulong maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(minimum, maximum));
            }
        }

        [TestCase(-999999999, long.MaxValue)]
        [TestCase(-999999999, 0)]
        [TestCase(0, long.MaxValue)]
        [TestCase(0, 2)]
        [TestCase(99, 999)]
        public void GenerateLong_Minimum_Maximum_DoesNotThrow(long minimum, long maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(minimum, maximum));
            }
        }

        [TestCase(ulong.MaxValue, ulong.MinValue)]
        [TestCase(2ul, 1ul)]
        public void GenerateLong_Minimum_Maximum_Throws_AooREx(ulong minimum, ulong maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => rngesus.GenerateLong(minimum, maximum));
            }
        }

        [TestCase(ulong.MaxValue, ulong.MaxValue)]
        [TestCase(1ul, 1ul)]
        [TestCase(0u, 0u)]
        public void GenerateLong_Minimum_Maximum_Throws_AEx(ulong minimum, ulong maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateLong(minimum, maximum));
            }
        }

        [TestCase(-999999999, -999999999)]
        [TestCase(long.MaxValue, long.MaxValue)]
        [TestCase(1, 1)]
        [TestCase(0, 0)]
        public void GenerateLong_Minimum_Maximum_Throws_AEx(long minimum, long maximum)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateLong(minimum, maximum));
            }
        }

        [Test]
        public void GenerateLong_ReturnsNoDuplicates()
        {
            var results = new long[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateLong();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateLong_Maximum_ReturnsNoDuplicates()
        {
            var results = new long[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateLong(999999);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateLong_MinimumAndMaximum_ReturnsNoDuplicates()
        {
            var results = new long[9];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateLong(999, 999999);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateLong_MinimumAndMaximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateLong(999, 999999);

                Assert.That(() => result >= 999);
                Assert.That(() => result <= 999999);
            }
        }

        #endregion UInt64

        #region Float

        [Test]
        public void GenerateFloat_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateFloat());
            }
        }

        [Test]
        public void GenerateFloat_ReturnsNoDuplicates()
        {
            var results = new float[99];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateFloat();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateFloat_ReturnsBetween0And1()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateFloat();

                Assert.That(result >= 0);
                Assert.That(result <= 1);
            }
        }

        #endregion Float

        #region Double

        [Test]
        public void GenerateDouble_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateDouble());
            }
        }

        [Test]
        public void GenerateDouble_ReturnsNoDuplicates()
        {
            var results = new double[99];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateDouble();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateDouble_ReturnsBetween0And1()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateDouble();

                Assert.That(result >= 0);
                Assert.That(result <= 1);
            }
        }

        #endregion Double

        #region Byte Array

        [Test]
        public void GenerateByteArray_Throws_AEx()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateByteArray(0));
            }
        }

        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(999)]
        [TestCase(-999)]
        public void GenerateByteArray_DoesNotThrow(int length)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateByteArray(length));
            }
        }

        [TestCase(1u)]
        [TestCase(999u)]
        public void GenerateByteArray_DoesNotThrow(uint length)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateByteArray(length));
            }
        }

        [Test]
        public void GenerateByteArray_ReturnsNoDuplicates()
        {
            var results = new byte[999][];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateByteArray(999);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateByteArray_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateByteArray(expectedLength);

                Assert.That((result.Length == expectedLength));
            }
        }

        #endregion Byte Array

        #region Strings

        [TestCase(1u)]
        [TestCase(999u)]
        [TestCase(1024u)]
        public void GenerateString_DoesNotThrow(uint length)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length));
            }
        }

        [TestCase(1)]
        [TestCase(-999)]
        [TestCase(1024)]
        public void GenerateString_DoesNotThrow(int length)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length));
            }
        }

        [TestCase(2u, "abc", true)]
        [TestCase(2u, "abc", false)]
        [TestCase(1024u, "abc123", true)]
        [TestCase(1024u, "abc123", false)]
        public void GenerateString_ValidCharactersString_DoesNotThrow(uint length, string validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length, validCharacters, removeDuplicates));
            }
        }

        [TestCase(-1024, "abc", true)]
        [TestCase(-1024, "abc", false)]
        [TestCase(2, "abc123", true)]
        [TestCase(2, "abc123", false)]
        [TestCase(1024, "abc123XYZ", true)]
        [TestCase(1024, "abc123XYZ", false)]
        public void GenerateString_ValidCharactersString_DoesNotThrow(int length, string validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length, validCharacters, removeDuplicates));
            }
        }

        [TestCase(2u, new char[] { 'a', 'b', 'c' }, true)]
        [TestCase(2u, new char[] { 'a', 'b', 'c' }, false)]
        [TestCase(1024u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, true)]
        [TestCase(1024u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, false)]
        public void GenerateString_ValidCharactersCharArray_DoesNotThrow(uint length, char[] validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length, validCharacters, removeDuplicates));
            }
        }

        [TestCase(-1024, new char[] { 'a', 'b', 'c' }, true)]
        [TestCase(-1024, new char[] { 'a', 'b', 'c' }, false)]
        [TestCase(2, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, true)]
        [TestCase(2, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, false)]
        [TestCase(1024, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, true)]
        [TestCase(1024, new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, false)]
        public void GenerateString_ValidCharactersCharArray_DoesNotThrow(int length, char[] validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(length, validCharacters, removeDuplicates));
            }
        }

        [TestCase(0u, new char[] { 'a', 'b', 'c' }, true)]
        [TestCase(1u, new char[] { 'a' }, true)]
        [TestCase(2u, new char[] { 'a', 'a' }, false)]
        public void GenerateString_Throws_AEx(uint length, char[] validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateString(length, validCharacters, removeDuplicates));
            }
        }

        [TestCase(1u)]
        [TestCase(1024u)]
        public void GenerateString_ReturnsCorrectLength(uint length)
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(length);

                Assert.That((result.Length == length));
            }
        }

        [Test]
        public void GenerateString_ReturnsNoDuplicates()
        {
            var results = new string[99];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(99);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [TestCase(999u, "abc123XYZ", true)]
        [TestCase(999u, "abc123XYZ", false)]
        [TestCase(9u, "abcdefghijklmnopqrstuvwxyz1234567890", true)]
        [TestCase(9u, "abcdefghijklmnopqrstuvwxyz1234567890", false)]
        public void GenerateString_ValidCharactersString_ReturnsCorrectLength(uint length, string validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(length, validCharacters, removeDuplicates);

                Assert.That((result.Length == length));
            }
        }

        [TestCase(999u, "abc123XYZ", true)]
        [TestCase(999u, "abc123XYZ", false)]
        [TestCase(9u, "abcdefghijklmnopqrstuvwxyz1234567890", true)]
        [TestCase(9u, "abcdefghijklmnopqrstuvwxyz1234567890", false)]
        public void GenerateString_ValidCharactersString_ReturnsNoDuplicates(uint length, string validCharacters, bool removeDuplicates)
        {
            var results = new string[99];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(length, validCharacters, removeDuplicates);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [TestCase(999u, new char[] { 'a', 'b', 'c', '1', '2', '3', 'X', 'Y', 'Z' }, true)]
        [TestCase(999u, new char[] { 'a', 'b', 'c', '1', '2', '3', 'X', 'Y', 'Z' }, false)]
        [TestCase(9u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, true)]
        [TestCase(9u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, false)]
        public void GenerateString_ValidCharactersString_ReturnsCorrectLength(uint length, char[] validCharacters, bool removeDuplicates)
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(length, validCharacters, removeDuplicates);

                Assert.That((result.Length == length));
            }
        }

        [TestCase(999u, new char[] { 'a', 'b', 'c', '1', '2', '3', 'X', 'Y', 'Z' }, true)]
        [TestCase(999u, new char[] { 'a', 'b', 'c', '1', '2', '3', 'X', 'Y', 'Z' }, false)]
        [TestCase(9u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, true)]
        [TestCase(9u, new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, false)]
        public void GenerateString_ValidCharactersString_ReturnsNoDuplicates(uint length, char[] validCharacters, bool removeDuplicates)
        {
            var results = new string[99];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(length, validCharacters, removeDuplicates);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Strings

        #region Thread Safety

        [Test]
        public void SharedBufferTest()
        {
            const int iterations = 999;
            var integerResults = new int[iterations];
            var longResults = new long[iterations];
            var boolResults = new bool[iterations];
            var floatResults = new float[iterations];
            var doubleResults = new double[iterations];
            var byteResults = new byte[iterations][];
            var stringResults = new string[iterations];

            using (var rngesus = new RNGesus())
            {
                for (var i = 0; i < iterations; i++)
                    integerResults[i] = rngesus.GenerateInt();

                for (var i = 0; i < iterations; i++)
                    longResults[i] = rngesus.GenerateLong();

                for (var i = 0; i < iterations; i++)
                    boolResults[i] = rngesus.GenerateBool();

                for (var i = 0; i < iterations; i++)
                    floatResults[i] = rngesus.GenerateFloat();

                for (var i = 0; i < iterations; i++)
                    doubleResults[i] = rngesus.GenerateDouble();

                for (var i = 0; i < iterations; i++)
                    byteResults[i] = rngesus.GenerateByteArray(99);

                for (var i = 0; i < iterations; i++)
                    stringResults[i] = rngesus.GenerateString(99);
            }

            Assert.That(integerResults.Length == iterations);
            Assert.That(longResults.Length == iterations);
            Assert.That(boolResults.Length == iterations);
            Assert.That(floatResults.Length == iterations);
            Assert.That(doubleResults.Length == iterations);
            Assert.That(byteResults.Length == iterations);
            Assert.That(stringResults.Length == iterations);
        }

        [Test]
        public void LockTest()
        {
            var result = "";

            using (var rngesus = new RNGesus(9999))
            {
                result = rngesus.GenerateString(rngesus.GenerateInt(999, 9999));
            }

            Assert.That(result.Length >= 999);
            Assert.That(result.Length <= 9999);
        }

        #endregion Thread Safety
    }
}
