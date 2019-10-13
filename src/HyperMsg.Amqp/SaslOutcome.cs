namespace HyperMsg.Amqp
{
    public class SaslOutcome : DescribedList
    {
        public SaslOutcome() : base(KnownDescriptors.SaslOutcome)
        { }

        public AuthCode Code
        {
            get => (AuthCode)Get<byte>(0);
            set => Set((byte)value, 0);
        }

        public byte[] AdditionalData
        {
            get => Get<byte[]>(0);
            set => Set(value, 0);
        }

        public bool IsSuccess => Code == AuthCode.Ok;
    }
}
