namespace HyperMsg.Amqp
{
    public class Attach : DescribedList
    {
        public Attach() : base(KnownDescriptors.Attach)
        { }

        public string Name
        {
            get => Get<string>(0);
            set => Set(value, 0);
        }

        public uint Handle
        {
            get => Get<uint>(1);
            set => Set(value, 1);
        }

        public bool Role
        {
            get => Get<bool>(2);
            set => Set(value, 2);
        }

        public object Source
        {
            get => Get<object>(5);
            set => Set(value, 5);
        }

        public object Target
        {
            get => Get<object>(6);
            set => Set(value, 6);
        }
    }
}