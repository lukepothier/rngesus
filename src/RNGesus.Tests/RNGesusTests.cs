using System;
using System.Linq;
using NUnit.Framework;

namespace RNGesus.Tests
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
        #region UInt32

        [Test]
        public void GenerateInt_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateInt());

        [Test]
        public void GenerateInt_Maximum_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateInt(99));

        [Test]
        public void GenerateInt_MinimumAndMaximum_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateInt(999, 9999));

        [Test]
        public void GenerateInt_MinimumBelowMaximum_Throws()
            => Assert.Throws<ArgumentOutOfRangeException>(() => RNGesus.GenerateInt(9999, 999));

        [Test]
        public void GenerateInt_EqualMinimumAndMaximum_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateInt(999, 999));

        [Test]
        public void GenerateInt_ReturnsValidUnsignedInt()
        {
            var result = RNGesus.GenerateInt();
            Assert.That(() => result >= 0);
            Assert.That(() => result <= uint.MaxValue);
        }

        [Test]
        public void GenerateInt_ReturnsNoDuplicates()
        {
            var results = new uint[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateInt();
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateInt_Maximum_ReturnsInRange()
            => Assert.That(() => RNGesus.GenerateInt(999) <= 999);

        [Test]
        public void GenerateInt_Maximum_ReturnsNoDuplicates()
        {
            var results = new uint[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateInt(999999);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateInt_MinimumAndMaximum_ReturnsInRange()
        {
            var result = RNGesus.GenerateInt(999, 999999);

            Assert.That(() => result >= 999);
            Assert.That(() => result <= 999999);
        }

        [Test]
        public void GenerateInt_MinimumAndMaximum_ReturnsNoDuplicates()
        {
            var results = new uint[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateInt(999, 999999);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion UInt32

        #region UInt64

        [Test]
        public void GenerateLong_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateLong());

        [Test]
        public void GenerateLong_Maximum_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateLong(999999999999));

        [Test]
        public void GenerateLong_MinimumAndMinimum_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateLong(999999999999, 999999999999999));

        [Test]
        public void GenerateLong_MinimumBelowMaximum_Throws()
            => Assert.Throws<ArgumentOutOfRangeException>(() => RNGesus.GenerateLong(999999999999999, 999999999999));

        [Test]
        public void GenerateLong_EqualMinimumAndMaximum_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateLong(999999999999, 999999999999));

        [Test]
        public void GenerateLong_ReturnsValidUnsignedLong()
        {
            var result = RNGesus.GenerateLong();
            Assert.That(() => result >= 0);
            Assert.That(() => result <= ulong.MaxValue);
        }

        [Test]
        public void GenerateLong_ReturnsNoDuplicates()
        {
            var results = new ulong[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateLong();
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateLong_Maximum_ReturnsInRange()
            => Assert.That(() => RNGesus.GenerateLong(999) <= 999);

        [Test]
        public void GenerateLong_Maximum_ReturnsNoDuplicates()
        {
            var results = new ulong[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateLong(999999);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateLong_MinimumAndMaximum_ReturnsInRange()
        {
            var result = RNGesus.GenerateLong(999, 999999);

            Assert.That(() => result >= 999);
            Assert.That(() => result <= 999999);
        }

        [Test]
        public void GenerateLong_MinimumAndMaximum_ReturnsNoDuplicates()
        {
            var results = new ulong[9];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateLong(999, 999999);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion UInt64

        #region Float

        [Test]
        public void GenerateFloat_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateFloat());

        [Test]
        public void GenerateFloat_ReturnsBetween0And1()
        {
            var result = RNGesus.GenerateFloat();

            Assert.That(result >= 0);
            Assert.That(result <= 1);
        }

        [Test]
        public void GenerateFloat_ReturnsNoDuplicates()
        {
            var results = new float[999];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateFloat();
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Float

        #region Double

        [Test]
        public void GenerateDouble_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateDouble());

        [Test]
        public void GenerateDouble_ReturnsBetween0And1()
        {
            var result = RNGesus.GenerateDouble();

            Assert.That(result >= 0);
            Assert.That(result <= 1);
        }

        [Test]
        public void GenerateDouble_ReturnsNoDuplicates()
        {
            var results = new double[999];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateDouble();
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Double

        #region Boolean

        [Test]
        public void GenerateBool_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateBool());

        #endregion Boolean

        #region Byte Array

        [Test]
        public void GenerateByteArray_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateByteArray(999));

        [Test]
        public void GenerateByteArray_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var result = RNGesus.GenerateByteArray(expectedLength);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateByteArray_ReturnsNoDuplicates()
        {
            var results = new byte[999][];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateByteArray(999);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Byte Array

        #region Strings

        [Test]
        public void GenerateString_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateString(999));

        [Test]
        public void GenerateString_ValidCharactersString_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateString(999, "123456789"));

        [Test]
        public void GenerateString_ValidCharactersString_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateString(999, "9"));

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateString(999, "123456789", false));

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateString(999, "999", false));

        [Test]
        public void GenerateString_ValidCharactersCharArray_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateString(999, new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }));

        [Test]
        public void GenerateString_ValidCharactersCharArray_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateString(999, new char[] { '1' }));

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_DoesNotThrow()
            => Assert.DoesNotThrow(() => RNGesus.GenerateString(999, new char[] { '1', '2', '3', '4', '5', '6', '7', '8' }, false));

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_Throws()
            => Assert.Throws<ArgumentException>(() => RNGesus.GenerateString(999, new char[] { '1', '1', '1' }, false));

        [Test]
        public void GenerateString_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var result = RNGesus.GenerateString(expectedLength);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateString_ReturnsNoDuplicates()
        {
            var results = new string[99];
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateString(99);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersString_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            const string validCharacters = "123456789";
            var result = RNGesus.GenerateString(expectedLength, validCharacters);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateString_ValidCharactersString_ReturnsNoDuplicates()
        {
            var results = new string[99];
            const string validCharacters = "123456789";
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateString(99, validCharacters);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            const string validCharacters = "123456789";
            var result = RNGesus.GenerateString(expectedLength, validCharacters, false);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_ReturnsNoDuplicates()
        {
            var results = new string[99];
            const string validCharacters = "123456789";
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateString(99, validCharacters, false);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var result = RNGesus.GenerateString(expectedLength, validCharacters);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_ReturnsNoDuplicates()
        {
            var results = new string[99];
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateString(99, validCharacters);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var result = RNGesus.GenerateString(expectedLength, validCharacters, false);

            Assert.That((result.Length == expectedLength));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_ReturnsNoDuplicates()
        {
            var results = new string[99];
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (var i = 0; i < results.Length; i++)
            {
                results[i] = RNGesus.GenerateString(99, validCharacters, false);
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Strings
    }
}
