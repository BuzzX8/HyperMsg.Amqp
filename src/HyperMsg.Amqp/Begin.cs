namespace HyperMsg.Amqp
{
    public class Begin : DescribedList
    {
        public Begin() : base(KnownDescriptors.Begin)
        { }

        public ushort RemoteChannel
        {
            get => Get<ushort>(0);
	        set => Set(value, 0);
        }

        public uint NextOutgoingId
        {
            get => Get<uint>(1);
	        set => Set(value, 1);
        }

        public uint IncomingWindow
        {
            get => Get<uint>(2);
	        set => Set(value, 2);
        }

        public uint OutgoingWindow
        {
            get => Get<uint>(3);
	        set => Set(value, 3);
        }

        public uint HandleMax
        {
            get => Get<uint>(4);
	        set => Set(value, 4);
        }
    }
}
