namespace HyperMsg.Amqp
{
    public class Frame
    {
        internal Frame(FrameType type, DescribedList body, ushort channel = 0)
        {
            Type = type;
            Body = body;
            Channel = channel;
        }

        public FrameType Type { get; }

        public ushort Channel { get; }

        public DescribedList Body { get; }

        public static Frame Amqp(DescribedList body, ushort channel = 0)
        {
            return new Frame(FrameType.Amqp, body, channel);
        }

        public static Frame Sasl(DescribedList body)
        {
            return new Frame(FrameType.Sasl, body);
        }
    }

    public enum FrameType { Amqp = 0, Sasl = 1 }
}
