namespace HyperMsg.Amqp
{
    public class Flow : DescribedList
    {
        public Flow() : base(KnownDescriptors.Flow)
        { }

        public uint NextIncomingId
        {
            get => Get<uint>(0);
            set => Set(value, 0);
        }

        public uint IncomingWindow
        {
            get => Get<uint>(1);
            set => Set(value, 1);
        }

        public uint NextOutgoingId
        {
            get => Get<uint>(2);
            set => Set(value, 2);
        }

        public uint OutgoingWindow
        {
            get => Get<uint>(3);
            set => Set(value, 3);
        }

        public uint Handle
        {
            get => Get<uint>(4);
            set => Set(value, 4);
        }

        public uint DeliveryCount
        {
            get => Get<uint>(5);
            set => Set(value, 5);
        }

        public uint LinkCredit
        {
            get => Get<uint>(6);
            set => Set(value, 6);
        }

        public uint Available
        {
            get => Get<uint>(7);
            set => Set(value, 7);
        }

        public bool Drain
        {
            get => Get<bool>(8);
            set => Set(value, 8);
        }

        public bool Echo
        {
            get => Get<bool>(9);
            set => Set(value, 9);
        }
    }
}
