namespace HyperMsg.Amqp
{
    public class SaslResponse : DescribedList
    {
        public SaslResponse() : base(KnownDescriptors.SaslResponse)
        { }

        public byte[] Response
        {
            get => Get<byte[]>(0);
            set => Set(value, 0);
        }
    }
}
