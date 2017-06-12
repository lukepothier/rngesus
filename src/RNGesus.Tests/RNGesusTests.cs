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
        #region UInt32

        [Test]
        public void GenerateInt_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt());
            }
        }

        [Test]
        public void GenerateInt_Maximum_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(99));
            }
        }

        [Test]
        public void GenerateInt_MinimumAndMaximum_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateInt(999, 9999));
            }
        }

        [Test]
        public void GenerateInt_MinimumBelowMaximum_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => rngesus.GenerateInt(9999, 999));
            }
        }

        [Test]
        public void GenerateInt_EqualMinimumAndMaximum_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateInt(999, 999));
            }
        }

        [Test]
        public void GenerateInt_ReturnsValidInt()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateInt();
                Assert.That(() => result >= 0);
                Assert.That(() => result <= int.MaxValue);
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
        public void GenerateInt_Maximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.That(() => rngesus.GenerateInt(999) <= 999);
            }
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
        public void GenerateInt_MinimumAndMaximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateInt(999, 999999);

                Assert.That(() => result >= 999);
                Assert.That(() => result <= 999999);
            }
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

        [Test]
        public void GenerateLong_Maximum_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(999999999999));
            }
        }

        [Test]
        public void GenerateLong_MinimumAndMinimum_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateLong(999999999999, 999999999999999));
            }
        }

        [Test]
        public void GenerateLong_MinimumBelowMaximum_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => rngesus.GenerateLong(999999999999999, 999999999999));
            }
        }

        [Test]
        public void GenerateLong_EqualMinimumAndMaximum_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateLong(999999999999, 999999999999));
            }
        }

        [Test]
        public void GenerateLong_ReturnsValidLong()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateLong();
                Assert.That(() => result >= 0);
                Assert.That(() => result <= long.MaxValue);
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
        public void GenerateLong_Maximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.That(() => rngesus.GenerateLong(999) <= 999);
            }
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
        public void GenerateLong_MinimumAndMaximum_ReturnsInRange()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateLong(999, 999999);

                Assert.That(() => result >= 999);
                Assert.That(() => result <= 999999);
            }
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
        public void GenerateFloat_ReturnsBetween0And1()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateFloat();

                Assert.That(result >= 0);
                Assert.That(result <= 1);
            }
        }

        [Test]
        public void GenerateFloat_ReturnsNoDuplicates()
        {
            var results = new float[999];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateFloat();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
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
        public void GenerateDouble_ReturnsBetween0And1()
        {
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateDouble();

                Assert.That(result >= 0);
                Assert.That(result <= 1);
            }
        }

        [Test]
        public void GenerateDouble_ReturnsNoDuplicates()
        {
            var results = new double[999];
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateDouble();
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Double

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

        #region Byte Array

        [Test]
        public void GenerateByteArray_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateByteArray(999));
            }
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

        #endregion Byte Array

        #region Strings

        [Test]
        public void GenerateString_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(999));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(999, "123456789"));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateString(999, "9"));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(999, "123456789", false));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateString(999, "999", false));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(999, new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateString(999, new char[] { '1' }));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_DoesNotThrow()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.DoesNotThrow(() => rngesus.GenerateString(999, new char[] { '1', '2', '3', '4', '5', '6', '7', '8' }, false));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_Throws()
        {
            using (var rngesus = new RNGesus())
            {
                Assert.Throws<ArgumentException>(() => rngesus.GenerateString(999, new char[] { '1', '1', '1' }, false));
            }
        }

        [Test]
        public void GenerateString_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(expectedLength);

                Assert.That((result.Length == expectedLength));
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

        [Test]
        public void GenerateString_ValidCharactersString_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            const string validCharacters = "123456789";
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(expectedLength, validCharacters);

                Assert.That((result.Length == expectedLength));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_ReturnsNoDuplicates()
        {
            var results = new string[99];
            const string validCharacters = "123456789";
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(99, validCharacters);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            const string validCharacters = "123456789";
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(expectedLength, validCharacters, false);

                Assert.That((result.Length == expectedLength));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_ReturnsNoDuplicates()
        {
            var results = new string[99];
            const string validCharacters = "123456789";
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(99, validCharacters, false);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(expectedLength, validCharacters);

                Assert.That((result.Length == expectedLength));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_ReturnsNoDuplicates()
        {
            var results = new string[99];
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(99, validCharacters);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_ReturnsCorrectLength()
        {
            const int expectedLength = 999;
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            using (var rngesus = new RNGesus())
            {
                var result = rngesus.GenerateString(expectedLength, validCharacters, false);

                Assert.That((result.Length == expectedLength));
            }
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_ReturnsNoDuplicates()
        {
            var results = new string[99];
            var validCharacters = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (var i = 0; i < results.Length; i++)
            {
                using (var rngesus = new RNGesus())
                {
                    results[i] = rngesus.GenerateString(99, validCharacters, false);
                }
            }

            Assert.That(results.Distinct().Count() == results.Count());
        }

        #endregion Strings
    }
}
