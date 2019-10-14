using System;
using System.Buffers;
using System.Buffers.Binary;

namespace HyperMsg.Amqp.Serialization
{
    public static class SerializationExtensions
    {
        private static readonly byte[] FalseValue = new[] { TypeCodes.FalseValue };
        private static readonly byte[] TrueValue = new[] { TypeCodes.TrueValue };
        private static readonly byte[] NullValue = new[] { TypeCodes.Null };

        public static void WriteFalse(this IBufferWriter<byte> writer) => writer.Write(FalseValue);

        public static void WriteTrue(this IBufferWriter<byte> writer) => writer.Write(TrueValue);

        public static void WriteNull(this IBufferWriter<byte> writer) => writer.Write(NullValue);

        public static void WriteBoolean(this IBufferWriter<byte> writer, bool value)
        {
            var span = writer.GetSpan(2);
            span[0] = TypeCodes.Boolean;
            span[1] = Convert.ToByte(value);
            writer.Advance(2);
        }

        public static void WriteUByte(this IBufferWriter<byte> writer, byte value)
        {
            var span = writer.GetSpan(2);
            span[0] = TypeCodes.UByte;
            span[1] = value;
            writer.Advance(2);
        }

        public static void WriteByte(this IBufferWriter<byte> writer, sbyte value)
        {
            var span = writer.GetSpan(2);
            span[0] = TypeCodes.Byte;
            span[1] = (byte)value;
            writer.Advance(2);
        }

        public static void WriteShort(this IBufferWriter<byte> writer, short value)
        {
            var span = writer.GetSpan(sizeof(short) + 1);
            span[0] = TypeCodes.Short;
            BinaryPrimitives.WriteInt16BigEndian(span.Slice(1), value);
            writer.Advance(sizeof(short) + 1);
        }
    }
}
