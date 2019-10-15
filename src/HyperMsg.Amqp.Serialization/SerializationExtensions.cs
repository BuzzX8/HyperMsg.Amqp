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

        public static void WriteBoolean(this IBufferWriter<byte> writer, bool value) => writer.WriteByteValueAndAdvance(TypeCodes.Boolean, Convert.ToByte(value));

        public static void WriteUByte(this IBufferWriter<byte> writer, byte value) => writer.WriteByteValueAndAdvance(TypeCodes.UByte, value);

        public static void WriteByte(this IBufferWriter<byte> writer, sbyte value) => writer.WriteByteValueAndAdvance(TypeCodes.Byte, (byte)value);

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
                writer.WriteByteValueAndAdvance(TypeCodes.SmallInt, (byte)value);
                return;
            }

            BinaryPrimitives.WriteInt32BigEndian(GetSpanForValue(writer, TypeCodes.Int, sizeof(int)), value);
            writer.Advance(sizeof(int) + 1);
        }

        public static void WriteUInt(this IBufferWriter<byte> writer, uint value)
        {
            if (value == 0)
            {
                writer.WriteCodeAndAdvance(TypeCodes.UInt0);
                return;
            }

            if (value <= byte.MaxValue)
            {
                writer.WriteByteValueAndAdvance(TypeCodes.SmallUInt, (byte)value);
                return;
            }

            BinaryPrimitives.WriteUInt32BigEndian(GetSpanForValue(writer, TypeCodes.UInt, sizeof(uint)), value);
            writer.Advance(sizeof(uint) + 1);
        }

        public static void WriteLong(this IBufferWriter<byte> writer, long value)
        {
            if (value <= sbyte.MaxValue && value >= sbyte.MinValue)
            {
                writer.WriteByteValueAndAdvance(TypeCodes.SmallLong, (byte)value);
                return;
            }

            BinaryPrimitives.WriteInt64BigEndian(GetSpanForValue(writer, TypeCodes.Long, sizeof(long)), value);
            writer.Advance(sizeof(long) + 1);
        }

        public static void WriteULong(this IBufferWriter<byte> writer, ulong value)
        {
            if (value == 0)
            {
                writer.WriteCodeAndAdvance(TypeCodes.ULong0);
                return;
            }

            if (value <= byte.MaxValue)
            {
                writer.WriteByteValueAndAdvance(TypeCodes.SmallULong, (byte)value);
                return;
            }

            BinaryPrimitives.WriteUInt64BigEndian(GetSpanForValue(writer, TypeCodes.ULong, sizeof(ulong)), value);
            writer.Advance(sizeof(ulong) + 1);
        }

        private static void WriteCodeAndAdvance(this IBufferWriter<byte> writer, byte code)
        {
            writer.GetSpan(1)[0] = code;
            writer.Advance(1);
        }

        private static void WriteByteValueAndAdvance(this IBufferWriter<byte> writer, byte code, byte value)
        {
            var span = GetSpanForValue(writer, code, sizeof(byte));
            span[0] = value;
            writer.Advance(sizeof(byte) + 1);
        }

        private static Span<byte> GetSpanForValue(IBufferWriter<byte> writer, byte code, int size)
        {
            var span = writer.GetSpan(size + 1);
            span[0] = code;
            return span.Slice(1);
        }
    }
}