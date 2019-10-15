using FakeItEasy;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace HyperMsg.Amqp.Serialization
{
    public class SerializationExtensionTests
    {
        private readonly IBufferWriter<byte> writer;
        private readonly Memory<byte> buffer;

        const int BufferSize = 512;

        public SerializationExtensionTests()
        {
            buffer = new byte[BufferSize];
            writer = A.Fake<IBufferWriter<byte>>();
            A.CallTo(() => writer.GetMemory(A<int>._)).Returns(buffer);
        }

        public static IEnumerable<object[]> GetTestCasesForWrite()
        {
            var random = Guid.NewGuid().ToByteArray();

            yield return GetTestCaseForWrite(w => w.WriteFalse(), TypeCodes.FalseValue);
            yield return GetTestCaseForWrite(w => w.WriteTrue(), TypeCodes.TrueValue);
            yield return GetTestCaseForWrite(w => w.WriteNull(), TypeCodes.Null);
            yield return GetTestCaseForWrite(w => w.WriteBoolean(false), TypeCodes.Boolean, 0);
            yield return GetTestCaseForWrite(w => w.WriteBoolean(true), TypeCodes.Boolean, 1);
            yield return GetTestCaseForWrite(w => w.WriteUByte(random[0]), TypeCodes.UByte, random[0]);
            yield return GetTestCaseForWrite(w => w.WriteByte((sbyte)random[1]), TypeCodes.Byte, random[1]);
            yield return GetTestCaseForWrite(w => w.WriteShort(random[3]), TypeCodes.Short, 0, random[3]);
            yield return GetTestCaseForWrite(w => w.WriteUShort(random[4]), TypeCodes.UShort, 0, random[4]);
            yield return GetTestCaseForWrite(w => w.WriteInt((sbyte)random[5]), TypeCodes.SmallInt, random[5]);
            yield return GetTestCaseForWrite(w => w.WriteInt(0x4050), TypeCodes.Int, 0, 0, 0x40, 0x50);
            yield return GetTestCaseForWrite(w => w.WriteUInt(0), TypeCodes.UInt0);
            yield return GetTestCaseForWrite(w => w.WriteUInt(random[5]), TypeCodes.SmallUInt, random[5]);
            yield return GetTestCaseForWrite(w => w.WriteUInt(0x8090), TypeCodes.UInt, 0, 0, 0x80, 0x90);
            yield return GetTestCaseForWrite(w => w.WriteLong((sbyte)random[6]), TypeCodes.SmallLong, random[6]);
            yield return GetTestCaseForWrite(w => w.WriteLong(0x10203040L), TypeCodes.Long, 0, 0, 0, 0, 0x10, 0x20, 0x30, 0x40);
            yield return GetTestCaseForWrite(w => w.WriteULong(0), TypeCodes.ULong0);
            yield return GetTestCaseForWrite(w => w.WriteULong(random[6]), TypeCodes.SmallULong, random[6]);
            yield return GetTestCaseForWrite(w => w.WriteULong(0x10203040L), TypeCodes.ULong, 0, 0, 0, 0, 0x10, 0x20, 0x30, 0x40);

            var uuid = Guid.NewGuid();
            yield return GetTestCaseForWrite(w => w.WriteUuid(uuid), GetUuidBytes(uuid));

            float floatValue = BitConverter.ToSingle(random, 0);
            yield return GetTestCaseForWrite(w => w.WriteFloat(floatValue), GetFloatBytes(floatValue));
            double doubleValue = BitConverter.ToDouble(random, 0);
            yield return GetTestCaseForWrite(w => w.WriteDouble(doubleValue), GetDoubleBytes(doubleValue));

            yield return GetTestCaseForWrite(w => w.WriteChar('G'), GetCharBytes('G'));

            var str8 = Guid.NewGuid().ToString();
            //yield return GetTestCaseForWrite(w => w.WriteSymbolic(str8), GetStringBytes(TypeCodes.Sym8, str8, Encoding.ASCII));
            //yield return GetTestCaseForWrite(w => w.WriteString(str8), GetStringBytes(TypeCodes.Str8, str8, Encoding.UTF8));
            //var str32 = Enumerable.Range(0, byte.MaxValue).Aggregate(str8, (s, i) => s + str8);
            //yield return GetTestCaseForWrite(w => w.WriteSymbolic(str32), GetStringBytes(TypeCodes.Sym32, str32, Encoding.ASCII));
            //yield return GetTestCaseForWrite(w => w.WriteString(str32), GetStringBytes(TypeCodes.Str32, str32, Encoding.UTF8));

            var timestamp = DateTime.UtcNow;
            yield return GetTestCaseForWrite(w => w.WriteTimestamp(timestamp), GetTimestampBytes(timestamp));

            yield return GetTestCaseForWrite(w => w.WriteList(new int[0]), TypeCodes.List0);
            //yield return GetTestCaseForWrite(w => w.WriteList(random.Take(3).ToList()), TypeCodes.List8, 6, 3,
            //    TypeCodes.UByte, random[0], TypeCodes.UByte, random[1], TypeCodes.UByte, random[2]);
        }

        public static object[] GetTestCaseForWrite(Action<IBufferWriter<byte>> write, params byte[] expected) => new object[] { write, expected };

        private static byte[] GetFloatBytes(float value)
        {
            var bytes = new List<byte> { TypeCodes.Float };
            bytes.AddRange(BitConverter.GetBytes(value));
            return bytes.ToArray();
        }

        private static byte[] GetDoubleBytes(double value)
        {
            var bytes = new List<byte> { TypeCodes.Double };
            bytes.AddRange(BitConverter.GetBytes(value));
            return bytes.ToArray();
        }

        private static byte[] GetCharBytes(char c)
        {
            var bytes = new List<byte> { TypeCodes.Char };
            bytes.AddRange(Encoding.UTF8.GetBytes(c.ToString()));
            return bytes.ToArray();
        }

        private static byte[] GetUuidBytes(Guid uuid)
        {
            var bytes = new List<byte> { TypeCodes.Uuid };
            bytes.AddRange(uuid.ToByteArray());

            return bytes.ToArray();
        }

        private static byte[] GetTimestampBytes(DateTime timestamp)
        {
            var unixTime = timestamp.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            return GetSerializedValue(TypeCodes.Timestamp, BitConverter.GetBytes((ulong)unixTime));
        }

        private static byte[] GetStringBytes(byte code, string str, Encoding encoding)
        {
            var bytes = new List<byte> { code };
            int byteCount = encoding.GetByteCount(str);
            if (byteCount <= byte.MaxValue)
            {
                bytes.Add((byte)byteCount);
            }
            else
            {
                bytes.AddRange(BitConverter.GetBytes(byteCount).Reverse());
            }
            bytes.AddRange(encoding.GetBytes(str));
            return bytes.ToArray();
        }

        private static byte[] GetSerializedValue(byte code, params byte[] serialized)
        {
            var bytes = new List<byte> { code };
            bytes.AddRange(serialized);
            return bytes.ToArray();
        }

        [Theory]
        [MemberData(nameof(GetTestCasesForWrite))]
        public void Write_Extension_Correctly_Serializes_Value(Action<IBufferWriter<byte>> write, byte[] expected)
        {
            var writer = new ByteBufferWriter(buffer);
            write.Invoke(writer);

            Assert.Equal(expected, buffer.Slice(0, expected.Length).ToArray());
            Assert.Equal(expected.Length, writer.Position);
        }
    }
}
