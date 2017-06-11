using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EZRNG
{
    public static class EZRandom
    {
        #region Init/Ctor

        static RNGCryptoServiceProvider _prng;

        static EZRandom()
        {
            _prng = new RNGCryptoServiceProvider();
        }

        #endregion Init/Ctor

        #region UInt32

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer
        /// </summary>
        /// <returns>
        /// A random unsigned 32-bit random integer
        /// </returns>
        public static uint GenerateInt()
        {
            var buffer = new byte[sizeof(uint)];
            _prng.GetBytes(buffer);

            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer equal to or below the provided maximum
        /// </returns>
        public static uint GenerateInt(uint maximum)
            => GenerateInt(uint.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer within the provided range
        /// </returns>
        public static uint GenerateInt(uint minimum, uint maximum)
        {
            if (minimum > maximum)
                throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

            if (minimum == maximum)
                return minimum;

            ulong difference = maximum - minimum;

            while (true)
            {
                var buffer = new byte[sizeof(uint)];
                _prng.GetBytes(buffer);
                var randomInteger = BitConverter.ToUInt32(buffer, 0);

                const ulong max = (ulong)uint.MaxValue + 1;
                var remainder = max % difference;
                if (randomInteger < max - remainder)
                {
                    return (uint)(minimum + (randomInteger % difference));
                }
            }
        }

        #endregion UInt32

        #region UInt64

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit random integer
        /// </summary>
        /// <returns>
        /// A random unsigned 64-bit random integer
        /// </returns>
        public static ulong GenerateLong()
        {
            var buffer = new byte[sizeof(ulong)];
            _prng.GetBytes(buffer);

            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit random integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer equal to or below the provided maximum
        /// </returns>
        public static ulong GenerateLong(ulong maximum)
            => GenerateLong(ulong.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned random 64-bit integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer within the provided range
        /// </returns>
        public static ulong GenerateLong(ulong minimum, ulong maximum)
        {
            if (minimum > maximum)
                throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

            if (minimum == maximum)
                return minimum;

            var difference = maximum - minimum;

            while (true)
            {
                var buffer = new byte[sizeof(ulong)];
                _prng.GetBytes(buffer);
                var randomInteger = BitConverter.ToUInt64(buffer, 0);

                const ulong max = ulong.MaxValue;
                var remainder = max % difference;
                if (randomInteger < max - remainder)
                {
                    return minimum + (randomInteger % difference);
                }
            }
        }

        #endregion UInt64

        #region Float

        /// <summary>
        /// Generates a cryptographically secure positive random 32-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>
        /// A random 32-bit positive floating point number between 0 and 1
        /// </returns>
        public static float GenerateFloat()
            => (float)GenerateDouble();

        #endregion Float

        #region Double

        /// <summary>
        /// Generates a cryptographically secure positive random 64-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>
        /// A random 64-bit positive floating point number between 0 and 1
        /// </returns>
        public static double GenerateDouble()
            => GenerateInt() / (uint.MaxValue + 1d);

        #endregion Double

        #region Boolean

        /// <summary>
        /// Generates a cryptographically secure random boolean
        /// </summary>
        /// <returns>
        /// A random boolean
        /// </returns>
        public static bool GenerateBool()
        {
            return GenerateInt() % 2 == 0;
        }

        #endregion Boolean

        #region Byte Array

        /// <summary>
        /// Generates a cryptographically secure random byte array
        /// </summary>
        /// <param name="length">The length of the byte array required</param>
        /// <returns>
        /// An array of random bytes
        /// </returns>
        public static byte[] GenerateByteArray(int length)
        {
            var buffer = new byte[length];
            _prng.GetBytes(buffer);
            return buffer;
        }

        #endregion Byte Array

        #region Strings

        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <returns>
        /// A random string containing upper- and lower-case (ISO basic Latin) alphabetical letters, numbers, hyphens, and underscores of the requested length
        /// </returns>
        public static string GenerateString(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
            return GenerateString(validCharacters, length);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        /// <param name="validCharacters">A string wrapping the character set of the output.</param>
        /// <param name="length">The length of the output required</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>, 
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public static string GenerateString(string validCharacters, int length, bool removeDuplicates = true)
        {
            var validCharacterArray = validCharacters.ToCharArray();

            if (removeDuplicates)
                validCharacterArray = validCharacterArray.Distinct().ToArray();

            return GenerateString(validCharacterArray, length, removeDuplicates);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        /// <param name="validCharacters">The character set of the output</param>
        /// <param name="length">The length of the output required</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>, 
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public static string GenerateString(char[] validCharacters, int length, bool removeDuplicates = true)
        {
            if (removeDuplicates)
                validCharacters = validCharacters.Distinct().ToArray();

            var validCharactersLength = validCharacters.Length;

            if (validCharactersLength <= 1)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(validCharacters));

            if (256 % validCharactersLength != 0)
                Trace.TraceWarning("The number of valid characters supplied cannot be evenly divided by 256. The entropy of the output is compromised.");

            var stringBuilder = new StringBuilder();

            var buffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                _prng.GetBytes(buffer);
                var index = BitConverter.ToUInt32(buffer, 0);
                stringBuilder.Append(validCharacters[(int)(index % (uint)validCharactersLength)]);
            }

            return stringBuilder.ToString();
        }

        #endregion Strings
    }
}
