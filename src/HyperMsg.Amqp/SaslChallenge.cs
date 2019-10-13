namespace HyperMsg.Amqp
{
    public class SaslChallenge : DescribedList
    {
        public SaslChallenge() : base(KnownDescriptors.SaslChallenge)
        { }

        public byte[] Challenge
        {
            get => Get<byte[]>(0);
            set => Set(value, 0);
        }
    }
}
