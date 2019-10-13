namespace HyperMsg.Amqp
{
    public class SaslInit : DescribedList
    {
        public SaslInit() : base(KnownDescriptors.SaslInit)
        { }

        public SaslInit(Symbol mechanism) : this()
        {
            Mechanism = mechanism;
        }

        public Symbol Mechanism
        {
            get => Get<Symbol>(0);
            set => Set(value, 0);
        }

        public byte[] InitialResponse
        {
            get => Get<byte[]>(1);
            set => Set(value, 1);
        }

        public string Hostname
        {
            get => Get<string>(2);
            set => Set(value, 2);
        }
    }
}
