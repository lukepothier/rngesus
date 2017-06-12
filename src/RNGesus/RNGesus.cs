// RNGesus.cs
// Copyright(c) 2017 Luke Pothier
// RNGesus is licensed using the MIT License
// Author: Luke Pothier <lukepothier@gmail.com>

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Luke.RNG
{
    /// <summary>
    /// Secure pseudorandom value generator
    /// </summary>
    public class RNGesus : IDisposable
    {
        #region Init/Ctor

        bool _disposed;
        readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
        const int DEFAULT_SHARED_BUFFER_LENGTH = 1024;
        static RNGCryptoServiceProvider _prng;
        byte[] _sharedBuffer;
        int _currentBufferIndex;
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

        void RequestBuffer(int bytesRequired)
        {
            if (_sharedBuffer == null)
                InitSharedBuffer();

            if (bytesRequired > _bufferLength)
                throw new ArgumentOutOfRangeException(nameof(bytesRequired), "Number of bytes required exceeds the size of the shared buffer. Re-initialise RNGesus with a larger buffer size.");

            if ((_sharedBuffer.Length - _currentBufferIndex) < bytesRequired)
                InitSharedBuffer();
        }

        #endregion Init/Ctor

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

                _prng.GetBytes(_sharedBuffer);

                var result = BitConverter.ToUInt32(_sharedBuffer, _currentBufferIndex);

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
        /// Generates a cryptographically secure unsigned 32-bit random integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 32-bit random integer within the provided range
        /// </returns>
        public int GenerateInt(uint minimum, uint maximum)
        {
            lock (this)
            {
                RequestBuffer(sizeof(uint));

                if (minimum > maximum)
                    throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

                if (minimum == maximum)
                    throw new ArgumentException("Only one output is possible - output is determinable from inputs");

                ulong difference = maximum - minimum;

                while (true)
                {
                    _prng.GetBytes(_sharedBuffer);
                    var randomInteger = BitConverter.ToUInt32(_sharedBuffer, _currentBufferIndex);

                    const ulong max = (ulong)uint.MaxValue + 1;
                    var remainder = max % difference;
                    if (randomInteger < max - remainder)
                    {
                        var result = (uint)(minimum + (randomInteger % difference));

                        _currentBufferIndex += sizeof(uint);

                        return (int)result >= 0
                            ? (int)result
                            : (int)result * -1;
                    }
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
        public long GenerateLong()
        {
            lock (this)
            {
                RequestBuffer(sizeof(ulong));

                _prng.GetBytes(_sharedBuffer);

                var result = BitConverter.ToUInt64(_sharedBuffer, _currentBufferIndex);

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
        /// Generates a cryptographically secure unsigned random 64-bit integer within a provided range
        /// </summary>
        /// <param name="minimum">The minimum value of the integer required</param>
        /// <param name="maximum">The maximum value of the integer required</param>
        /// <returns>
        /// A random unsigned 64-bit integer within the provided range
        /// </returns>
        public long GenerateLong(ulong minimum, ulong maximum)
        {
            lock (this)
            {
                if (minimum > maximum)
                    throw new ArgumentOutOfRangeException(nameof(minimum), "minimum cannot exceed maximum");

                if (minimum == maximum)
                    throw new ArgumentException("Only one output is possible - output is determinable from inputs");

                RequestBuffer(sizeof(ulong));

                var difference = maximum - minimum;

                while (true)
                {
                    _prng.GetBytes(_sharedBuffer);
                    var randomInteger = BitConverter.ToUInt64(_sharedBuffer, _currentBufferIndex);

                    var remainder = ulong.MaxValue % difference;
                    if (randomInteger < ulong.MaxValue - remainder)
                    {
                        var result = minimum + (randomInteger % difference);

                        _currentBufferIndex += sizeof(ulong);

                        return (long)result >= 0
                            ? (long)result
                            : (long)result * -1;
                    }
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

        #region Byte Array

        /// <summary>
        /// Generates a cryptographically secure random byte array
        /// </summary>
        /// <param name="length">The length of the byte array required</param>
        /// <returns>
        /// An array of random bytes
        /// </returns>
        public byte[] GenerateByteArray(int length)
        {
            lock (this)
            {
                RequestBuffer(length);

                var result = _sharedBuffer.Take(length).ToArray();

                _currentBufferIndex += length;

                return result;
            }
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
        public string GenerateString(int length)
        {
            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
            return GenerateString(length, validCharacters);
        }

        /// <summary>
        /// Generates a cryptographically secure random string
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
            var validCharacterArray = validCharacters.ToCharArray();

            var distinctValidCharacterArray = validCharacterArray.Distinct().ToArray();

            if (distinctValidCharacterArray.Length <= 1)
                throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(validCharacters));

            if (removeDuplicates)
                validCharacterArray = distinctValidCharacterArray;

            return GenerateString(length, validCharacterArray, removeDuplicates);
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
        public string GenerateString(int length, char[] validCharacters, bool removeDuplicates = true)
        {
            lock (this)
            {
                var distinctValidCharacterArray = validCharacters.Distinct().ToArray();

                if (distinctValidCharacterArray.Length <= 1)
                    throw new ArgumentException("Only one output is possible - output is determinable from inputs", nameof(validCharacters));

                if (removeDuplicates)
                    validCharacters = distinctValidCharacterArray;

                var validCharactersLength = validCharacters.Length;

                if (256 % validCharactersLength != 0)
                    Trace.TraceWarning("The number of valid characters supplied cannot be evenly divided by 256. The entropy of the output is compromised.");

                var stringBuilder = new StringBuilder();

                RequestBuffer(length);

                while (length-- > 0)
                {
                    _prng.GetBytes(_sharedBuffer);
                    var index = BitConverter.ToUInt32(_sharedBuffer, _currentBufferIndex);
                    stringBuilder.Append(validCharacters[(int)(index % (uint)validCharactersLength)]);
                }

                var result = stringBuilder.ToString();

                _currentBufferIndex += length;

                return result;
            }
        }

        #endregion Strings

        #region GC

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                _handle.Dispose();

            _disposed = true;
        }

        #endregion GC
    }
}
