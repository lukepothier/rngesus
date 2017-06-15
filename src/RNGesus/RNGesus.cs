// RNGesus.cs
// Copyright(c) 2017 Luke Pothier
// RNGesus is licensed using the MIT License
// Author: Luke Pothier <lukepothier@gmail.com>

using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Luke.RNG
{
    /// <summary>
    /// Secure pseudorandom value generator
    /// </summary>
    public class RNGesus : IDisposable
    {
        #region Init/Ctor

        const int DEFAULT_SHARED_BUFFER_LENGTH = 1024;
        static RNGCryptoServiceProvider _prng;
        byte[] _sharedBuffer;
        uint _currentBufferIndex;
        readonly int _bufferLength;

        /// <summary>
        /// Secure pseudorandom value generator.
        /// The parameterless constructor initialises RNGesus with up to 1024 byte operations supported. If you require outputs larger than 1024 bytes, consider using the overload.
        /// </summary>
        public RNGesus()
            : this(DEFAULT_SHARED_BUFFER_LENGTH)
        {
        }

        /// <summary>
        /// Secure pseudorandom value generator.
        /// <param name="sharedBufferSize">The maximum size of outputs you expect to need to generate. Larger buffer size will result in higher memory use.</param>
        /// </summary>
        public RNGesus(int sharedBufferSize)
        {
            if (sharedBufferSize < 1)
            {
                Trace.TraceWarning("Invalid buffer size requested. Initialising RNGesus with 1024-byte buffer.");
                sharedBufferSize = DEFAULT_SHARED_BUFFER_LENGTH;
            }

            _prng = new RNGCryptoServiceProvider();
            _bufferLength = sharedBufferSize;
        }

        void InitSharedBuffer()
        {
            if (_sharedBuffer == null || _sharedBuffer.Length != _bufferLength)
                _sharedBuffer = new byte[_bufferLength];

            _prng.GetBytes(_sharedBuffer);
            _currentBufferIndex = 0;
        }

        void RequestBuffer(uint bytesRequired)
        {
            if (_sharedBuffer == null)
                InitSharedBuffer();

            if (bytesRequired > _bufferLength)
                throw new ArgumentOutOfRangeException(nameof(bytesRequired), "Number of bytes required exceeds the size of the shared buffer. Re-initialise RNGesus with a larger buffer size.");

            if ((_sharedBuffer.Length - _currentBufferIndex) < bytesRequired)
                InitSharedBuffer();
        }

        #endregion Init/Ctor

        #region Boolean

        /// <summary>
        /// Generates a cryptographically secure random boolean
        /// </summary>
        /// <returns>
        /// A random boolean
        /// </returns>
        public bool GenerateBool()
            => GenerateInt() % 2 == 0;

        #endregion Boolean

        #region UInt32

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer
        /// </summary>
        /// <returns>
        /// A random unsigned 32-bit random integer
        /// </returns>
        public int GenerateInt()
        {
            lock (this)
            {
                RequestBuffer(sizeof(uint));

                var result = BitConverter.ToUInt32(_sharedBuffer, (int)_currentBufferIndex);

                _currentBufferIndex += sizeof(uint);

                return (int)result >= 0
                    ? (int)result
                    : (int)result * -1;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer equal to or below the provided maximum
        /// </returns>
        public int GenerateInt(uint maximum)
            => GenerateInt(uint.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer equal to or below a provided maximum.
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer equal to or below the provided maximum
        /// </returns>
        public int GenerateInt(int maximum)
        {
            if (maximum < 0)
                return GenerateInt((uint)(maximum * -1));

            return GenerateInt((uint)(maximum));
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer within the provided range
        /// </returns>
        public int GenerateInt(uint minimum, uint maximum)
        {
            if (minimum > maximum)
                throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

            if (minimum == maximum)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs");

            ulong difference = maximum - minimum;

            lock (this)
            {
                RequestBuffer(sizeof(uint));

                var randomInteger = BitConverter.ToUInt32(_sharedBuffer, (int)_currentBufferIndex);

                const ulong max = (ulong)uint.MaxValue + 1;
                var remainder = max % difference;

                var result = (uint)(minimum + (randomInteger % difference));

                _currentBufferIndex += sizeof(uint);

                return (int)result >= 0
                    ? (int)result
                    : (int)result * -1;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 32-bit random integer within a provided range
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer within the provided range
        /// </returns>
        public int GenerateInt(int minimum, int maximum)
        {
            var uintMinimum = (uint)Math.Abs(minimum);
            var uintMaximum = (uint)Math.Abs(maximum);

            // If minimums and maximums have been switched due to Math.Abs, correct it to avoid exceptions
            if (uintMinimum > uintMaximum)
            {
                var temp = uintMinimum;
                uintMinimum = uintMaximum;
                uintMaximum = temp;
            }

            return GenerateInt(uintMinimum, uintMaximum);
        }

        #endregion UInt32

        #region UInt64

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit random integer
        /// </summary>
        /// <returns>
        /// A random unsigned 64-bit random integer
        /// </returns>
        public long GenerateLong()
        {
            lock (this)
            {
                RequestBuffer(sizeof(ulong));

                var result = BitConverter.ToUInt64(_sharedBuffer, (int)_currentBufferIndex);

                _currentBufferIndex += sizeof(ulong);

                return (long)result >= 0
                    ? (long)result
                    : (long)result * -1;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit random integer equal to or below a provided maximum
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer equal to or below the provided maximum
        /// </returns>
        public long GenerateLong(ulong maximum)
            => GenerateLong(ulong.MinValue, maximum);

        /// <summary>
        /// Generates a cryptographically secure unsigned 64-bit random integer equal to or below a provided maximum
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer equal to or below the provided maximum
        /// </returns>
        public long GenerateLong(long maximum)
        {
            if (maximum < 0)
                return GenerateLong((ulong)(maximum * -1));

            return GenerateLong((ulong)(maximum));
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned random 64-bit integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer within the provided range
        /// </returns>
        public long GenerateLong(ulong minimum, ulong maximum)
        {
            if (minimum > maximum)
                throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

            if (minimum == maximum)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs");

            var difference = maximum - minimum;

            lock (this)
            {
                RequestBuffer(sizeof(ulong));

                var randomInteger = BitConverter.ToUInt64(_sharedBuffer, (int)_currentBufferIndex);

                var remainder = ulong.MaxValue % difference;

                var result = minimum + (randomInteger % difference);

                _currentBufferIndex += sizeof(ulong);

                return (long)result >= 0
                    ? (long)result
                    : (long)result * -1;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure unsigned random 64-bit integer within a provided range
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer within the provided range
        /// </returns>
        public long GenerateLong(long minimum, long maximum)
        {
            var ulongMinimum = (ulong)Math.Abs(minimum);
            var ulongMaximum = (ulong)Math.Abs(maximum);

            // If minimums and maximums have been switched due to Math.Abs, correct it to avoid exceptions
            if (ulongMinimum > ulongMaximum)
            {
                var temp = ulongMinimum;
                ulongMinimum = ulongMaximum;
                ulongMaximum = temp;
            }

            return GenerateLong(ulongMinimum, ulongMaximum);
        }

        #endregion UInt64

        #region Float

        /// <summary>
        /// Generates a cryptographically secure positive random 32-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>
        /// A random 32-bit positive floating point number between 0 and 1
        /// </returns>
        public float GenerateFloat()
            => (float)GenerateDouble();

        #endregion Float

        #region Double

        /// <summary>
        /// Generates a cryptographically secure positive random 64-bit floating point number between 0 and 1
        /// </summary>
        /// <returns>
        /// A random 64-bit positive floating point number between 0 and 1
        /// </returns>
        public double GenerateDouble()
            => GenerateInt() / (uint.MaxValue + 1d);

        #endregion Double

        #region Byte Array

        /// <summary>
        /// Generates a cryptographically secure random byte array
        /// </summary>
        /// <param name="length">The length of the byte array required</param>
        /// <returns>
        /// An array of random bytes
        /// </returns>
        public byte[] GenerateByteArray(uint length)
        {
            if (length == 0)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(length));

            lock (this)
            {
                RequestBuffer(length);

                var result = _sharedBuffer.Take((int)length).ToArray();

                _currentBufferIndex += length;

                return result;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random byte array
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="length">The length of the byte array required</param>
        /// <returns>
        /// An array of random bytes
        /// </returns>
        public byte[] GenerateByteArray(int length)
        {
            if (length < 0)
                length = length * -1;

            return GenerateByteArray((uint)length);
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
        public string GenerateString(uint length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
            return GenerateString(length, validCharacters);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <returns>
        /// A random string containing upper- and lower-case (ISO basic Latin) alphabetical letters, numbers, hyphens, and underscores of the requested length
        /// </returns>
        public string GenerateString(int length)
        {
            if (length < 0)
                length = length * -1;

            return GenerateString((uint)length);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <param name="validCharacters">A string wrapping the character set of the output.</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>,
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public string GenerateString(uint length, string validCharacters, bool removeDuplicates = true)
        {
            var validCharacterArray = validCharacters.ToCharArray();

            return GenerateString(length, validCharacterArray, removeDuplicates);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// If a negative argument is received, it will be treated as it's equivalent positive value.
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <param name="validCharacters">A string wrapping the character set of the output.</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>,
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public string GenerateString(int length, string validCharacters, bool removeDuplicates = true)
        {
            if (length < 0)
                length = length * -1;

            return GenerateString((uint)length, validCharacters, removeDuplicates);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <param name="validCharacters">The character set of the output</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>,
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public string GenerateString(uint length, char[] validCharacters, bool removeDuplicates = true)
        {
            var distinctValidCharacterArray = validCharacters.Distinct().ToArray();

            if (distinctValidCharacterArray.Length <= 1)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(validCharacters));

            if (length == 0)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(length));

            if (removeDuplicates)
                validCharacters = distinctValidCharacterArray;

            var validCharactersLength = validCharacters.Length;

            if (256 % validCharactersLength != 0)
                Trace.TraceWarning("The number of valid characters supplied cannot be evenly divided by 256. The entropy of the output is compromised.");

            var stringBuilder = new StringBuilder();

            var tempLength = length;

            lock (this)
            {
                RequestBuffer(tempLength);

                while (length-- > 0)
                {
                    // It's necessary to recreate these bytes during iterating
                    _prng.GetBytes(_sharedBuffer);
                    var index = BitConverter.ToUInt32(_sharedBuffer, (int)_currentBufferIndex);
                    stringBuilder.Append(validCharacters[(int)(index % (uint)validCharactersLength)]);
                }

                var result = stringBuilder.ToString();

                _currentBufferIndex += tempLength;

                return result;
            }
        }

        /// <summary>
        /// Generates a cryptographically secure random string
        /// If a negative argument is received, it will be treated as it's equivalent positive value. 
        /// </summary>
        /// <param name="length">The length of the output required</param>
        /// <param name="validCharacters">The character set of the output</param>
        /// <param name="removeDuplicates">Whether or not to sanitise the input string to remove duplicate characters</param>
        /// <returns>
        /// A random string containing only characters present in <paramref name="validCharacters"/> of the requested length
        /// </returns>
        /// <remarks>
        /// WARNING: If 256 cannot evenly divide the number of characters (or number of distinct characters if <paramref name="removeDuplicates"/> is true) in <paramref name="validCharacters"/>,
        /// then the entropy of the output is compromised. For best results, ensure that 256 evenly divides the number of valid characters.
        /// </remarks>
        public string GenerateString(int length, char[] validCharacters, bool removeDuplicates = true)
        {
            if (length < 0)
                length = length * -1;

            return GenerateString((uint)length, validCharacters, removeDuplicates);
        }

        #endregion Strings

        #region GC

        /// <summary>
        /// Disposes the instance of RNGesus
        /// </summary>
        public void Dispose()
            => GC.SuppressFinalize(this);

        #endregion GC
    }
}
