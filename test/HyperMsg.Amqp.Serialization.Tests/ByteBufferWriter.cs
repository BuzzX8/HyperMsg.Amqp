using System;
using System.Buffers;

namespace HyperMsg.Amqp.Serialization
{
    public class ByteBufferWriter : IBufferWriter<byte>
    {
        private readonly Memory<byte> buffer;

        public ByteBufferWriter(Memory<byte> buffer)
        {
            this.buffer = buffer;
        }

        public int Position { get; private set; }

        public void Advance(int count)
        {
            Position += count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0) => buffer;

        public Span<byte> GetSpan(int sizeHint = 0) => buffer.Span;
    }
}