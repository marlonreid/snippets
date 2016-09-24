using System;
using System.Text;

namespace CorrelationIdGenerator
{
    public static class CorrelationId
    {
        private const uint NR_CHARS_IN_ENC_ID = 20;
        private const uint RADIX = 85;
        private const int TUPLE_SIZE = 4;
        private const uint NR_BYTES_IN_GUID = 16;
        private const int FIRST_PRINTABLE_CHAR = 33;

        // A guid is always 128bits (16 bytes).
        // A tuple is always 4
        // Using base 85, we can represent 4 bytes using 5 printable ASCII characters.
        // The base85 encoded guid can be textually represented using 20 characters instead
        // of the usual 32 characters. 

        public static string New()
        {
            var byteArray = Guid.NewGuid().ToByteArray();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteArray);
            }

            var base85Array = new uint[NR_CHARS_IN_ENC_ID];
            var count = NR_CHARS_IN_ENC_ID;

            for (var i = 0; i <= NR_BYTES_IN_GUID - 1; i += TUPLE_SIZE)
            {
                var x = BitConverter.ToUInt32(byteArray, i);

                while (x > 0)
                {
                    base85Array[--count] = x%RADIX;
                    x /= RADIX;
                }
            }

            var builder = new StringBuilder();
            foreach (var c in base85Array)
            {
                builder.Append((char) (c + FIRST_PRINTABLE_CHAR));
            }

            return builder.ToString();
        }
    }
}