namespace Luke.RNG
{
    internal static class Constants
    {
        public const string DEFAULT_STRING_CHARSET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";

        public const string ERROR_BYTES_EXCEEDS_BUFFER = "Number of bytes required exceeds the size of the shared buffer. Re-initialise RNGesus with a larger buffer size.";
        public const string ERROR_MINIMUM_EXCEEDS_MAXIMUM = "Minimum cannot exceed maximum";
        public const string ERROR_DETERMINABLE_OUTPUT = "Only one output is possible - output is determinable from inputs";

        public const string WARNING_INVALID_BUFFER_SIZE = "Invalid buffer size requested. Initialising RNGesus with 1024-byte buffer.";
        public const string WARNING_ENTROPY_COMPROMISED = "The number of valid characters supplied cannot be evenly divided by 256. The entropy of the output is compromised.";
    }
}
