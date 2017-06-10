using System;
using System.Security.Cryptography;

namespace EZRNG
{
    public class EZRandom
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
        /// Generates a cryptographically secure unsigned 32-bit integer
        /// </summary>
        /// <returns>A random unsigned 32-bit integer</returns>
        public static uint GenerateInt()
        {
            var buffer = new byte[sizeof(uint)];
            _prng.GetBytes(buffer);

            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>A random unsigned 32-bit integer equal to or below the provided maximum</returns>
        public static uint GenerateInt(uint maximum)
            => GenerateInt(uint.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>A random unsigned 32-bit integer within the provided range</returns>
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

                const ulong max = ((ulong)uint.MaxValue + 1);
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
        /// Generates a cryptographically secure unsigned 64-bit integer
        /// </summary>
        /// <returns>A random unsigned 64-bit integer</returns>
        public static ulong GenerateLong()
        {
            var buffer = new byte[sizeof(ulong)];
            _prng.GetBytes(buffer);

            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>A random unsigned 64-bit integer equal to or below the provided maximum</returns>
        public static ulong GenerateLong(ulong maximum)
            => GenerateLong(ulong.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>A random unsigned 64-bit integer within the provided range</returns>
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

                const ulong max = (ulong.MaxValue);
                var remainder = max % difference;
                if (randomInteger < max - remainder)
                {
                    return (minimum + (randomInteger % difference));
                }
            }
        }

        #endregion UInt64

        #region Float

        /// <summary>
        /// Generates a cryptographically secure positive 32-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>A random 32-bit positive floating point number between 0 and 1</returns>
        public static float GenerateFloat()
            => (float)GenerateDouble();

        #endregion Float

        #region Double

        /// <summary>
        /// Generates a cryptographically secure positive 64-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>A random 64-bit positive floating point number between 0 and 1</returns>
        public static double GenerateDouble()
            => GenerateInt() / (uint.MaxValue + 1d);

        #endregion Double

        #region Boolean

        public static bool GenerateBool()
        {
            return GenerateInt() % 2 == 0;
        }

        #endregion Boolean
    }
}
