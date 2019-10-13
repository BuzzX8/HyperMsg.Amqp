namespace HyperMsg.Amqp
{
    public class Open : DescribedList
    {
        public Open() : base(KnownDescriptors.Open)
        { }

        public string ContainerId
        {
            get => Get<string>(0);
            set => Set(value, 0);
        }

        public string Hostname
        {
            get => Get<string>(1);
            set => Set(value, 1);
        }

        public uint MaxFrameSize
        {
            get => Get<uint>(2);
            set => Set(value, 2);
        }

        public ushort MaxChannel
        {
            get => Get<ushort>(3);
            set => Set(value, 3);
        }

        public uint IdleTimeout
        {
            get => Get<uint>(4);
            set => Set(value, 4);
        }
    }
}
