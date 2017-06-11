using System;
using NUnit.Framework;

namespace RNGesus.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        #region UInt32

        [Test]
        public void GenerateInt_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateInt());
        }

        [Test]
        public void GenerateInt_Maximum_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateInt(99));
        }

        [Test]
        public void GenerateInt_MaximumAndMinimum_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateInt(999, 9999));
        }

        #endregion UInt32

        #region UInt64

        [Test]
        public void GenerateLong_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateLong());
        }

        [Test]
        public void GenerateLong_Maximum_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateLong(999999999999));
        }

        [Test]
        public void GenerateLong_MaximumAndMinimum_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateLong(999999999999, 999999999999999));
        }

        #endregion UInt64

        #region Float

        [Test]
        public void GenerateFloat_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateFloat());
        }

        #endregion Float

        #region Double

        [Test]
        public void GenerateDouble_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateDouble());
        }

        #endregion Double

        #region Boolean

        [Test]
        public void GenerateBool_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateBool());
        }

        #endregion Boolean

        #region Byte Array

        [Test]
        public void GenerateByteArray_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateByteArray(999));
        }

        #endregion Byte Array

        #region Strings

        [Test]
        public void GenerateString_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateString(999));
        }

        [Test]
        public void GenerateString_ValidCharactersString_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateString("12345678", 999));
        }

        [Test]
        public void GenerateString_ValidCharactersString_Throws()
        {
            Assert.Throws<ArgumentException>(() => Generator.GenerateString("1", 999));
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateString("12345678", 999, false));
        }

        [Test]
        public void GenerateString_ValidCharactersString_RemoveDuplicatesFalse_Throws()
        {
            Assert.Throws<ArgumentException>(() => Generator.GenerateString("111", 999, false));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateString(new char[] { '1', '2', '3', '4', '5', '6', '7', '8' }, 999));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_Throws()
        {
            Assert.Throws<ArgumentException>(() => Generator.GenerateString(new char[] { '1' }, 999));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => Generator.GenerateString(new char[] { '1', '2', '3', '4', '5', '6', '7', '8' }, 999, false));
        }

        [Test]
        public void GenerateString_ValidCharactersCharArray_RemoveDuplicatesFalse_Throws()
        {
            Assert.Throws<ArgumentException>(() => Generator.GenerateString(new char[] { '1', '1', '1' }, 999, false));
        }

        #endregion Strings
    }
}
