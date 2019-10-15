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
            GetSpanForValue(writer, TypeCodes.Boolean, sizeof(byte))[0] = Convert.ToByte(value);
            writer.Advance(sizeof(byte) + 1);
        }

        public static void WriteUByte(this IBufferWriter<byte> writer, byte value)
        {
            GetSpanForValue(writer, TypeCodes.UByte, sizeof(byte))[0] = value;
            writer.Advance(sizeof(byte) + 1);
        }

        public static void WriteByte(this IBufferWriter<byte> writer, sbyte value)
        {
            GetSpanForValue(writer, TypeCodes.Byte, sizeof(sbyte))[0] = (byte)value;
            writer.Advance(sizeof(sbyte) + 1);
        }

        public static void WriteShort(this IBufferWriter<byte> writer, short value)
        {
            BinaryPrimitives.WriteInt16BigEndian(GetSpanForValue(writer, TypeCodes.Short, sizeof(short)), value);
            writer.Advance(sizeof(short) + 1);
        }

        public static void WriteUShort(this IBufferWriter<byte> writer, ushort value)
        {
            BinaryPrimitives.WriteUInt16BigEndian(GetSpanForValue(writer, TypeCodes.UShort, sizeof(ushort)), value);
            writer.Advance(sizeof(ushort) + 1);
        }

        public static void WriteInt(this IBufferWriter<byte> writer, int value)
        {
            if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            {
                GetSpanForValue(writer, TypeCodes.SmallInt, sizeof(byte))[0] = (byte)value;
                writer.Advance(sizeof(byte) + 1);
                return;
            }

            BinaryPrimitives.WriteInt32BigEndian(GetSpanForValue(writer, TypeCodes.Int, sizeof(int)), value);
            writer.Advance(sizeof(int) + 1);
        }

        private static Span<byte> GetSpanForValue(IBufferWriter<byte> writer, byte code, int size)
        {
            var span = writer.GetSpan(size + 1);
            span[0] = code;
            return span.Slice(1);
        }
    }
}