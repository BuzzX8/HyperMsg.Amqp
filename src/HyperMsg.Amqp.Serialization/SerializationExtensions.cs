using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections;
using System.Text;

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

        public static void WriteUuid(this IBufferWriter<byte> writer, Guid value) => writer.WriteBytesAndAdvance(TypeCodes.Uuid, value.ToByteArray());

        public static void WriteFloat(this IBufferWriter<byte> writer, float value) => writer.WriteBytesAndAdvance(TypeCodes.Float, BitConverter.GetBytes(value));

        public static void WriteDouble(this IBufferWriter<byte> writer, double value) => writer.WriteBytesAndAdvance(TypeCodes.Double, BitConverter.GetBytes(value));

        public static void WriteChar(this IBufferWriter<byte> writer, char value) => writer.WriteBytesAndAdvance(TypeCodes.Char, Encoding.UTF8.GetBytes(value.ToString()));

        public static void WriteSymbolic(this IBufferWriter<byte> writer, Symbol value) => WriteString(writer, value, TypeCodes.Sym8, TypeCodes.Sym32, Encoding.ASCII);

        public static void WriteTimestamp(this IBufferWriter<byte> writer, DateTime value)
        {
            var unixTime = value.Subtract(new DateTime(1970, 1, 1));
            writer.WriteBytesAndAdvance(TypeCodes.Timestamp, BitConverter.GetBytes((ulong)unixTime.TotalMilliseconds));
        }

        public static void WriteList(this IBufferWriter<byte> writer, IList value)
        {
            if (value.Count == 0)
            {
                writer.WriteCodeAndAdvance(TypeCodes.List0);
                return;
            }
        }

        private static void WriteString(IBufferWriter<byte> writer, string value, byte code8, byte code32, Encoding encoding)
        {
            var strBytes = encoding.GetBytes(value);
            int length = value.Length;
            Span<byte> span;
            
            if (length <= byte.MaxValue)
            {
                span = writer.GetSpan(strBytes.Length + 2);
                span[0] = code8;
                span[1] = (byte)value.Length;
            }
            else
            {
                //writer.Write(code32);
                //writer.Write(length);
            }
            
            //writer.Write(strBytes, 0, strBytes.Length);
        }

        private static void WriteBytesAndAdvance(this IBufferWriter<byte> writer, byte code, byte[] value)
        {
            var span = GetSpanForValue(writer, code, value.Length);
            value.CopyTo(span);
            writer.Advance(value.Length + 1);
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