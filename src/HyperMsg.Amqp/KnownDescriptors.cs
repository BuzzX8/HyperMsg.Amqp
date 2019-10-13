namespace HyperMsg.Amqp
{
    public static class KnownDescriptors
    {
        public static readonly Descriptor Open = new Descriptor(0x0000000000000010, "amqp:open:list");
        public static readonly Descriptor Close = new Descriptor(0x0000000000000018, "amqp:close:list");
        public static readonly Descriptor Begin = new Descriptor(0x0000000000000011, "amqp:begin:list");
        public static readonly Descriptor End = new Descriptor(0x0000000000000017, "amqp:end:list");
        public static readonly Descriptor Attach = new Descriptor(0x00000000000000012, "amqp:attach:list");
        public static readonly Descriptor Detach = new Descriptor(0x0000000000000016, "amqp:detach:list");
        public static readonly Descriptor Flow = new Descriptor(0x0000000000000013, "amqp:flow:list");

        public static readonly Descriptor Error = new Descriptor(0x000000000000001d, "amqp:error:list");

        public static readonly Descriptor SaslMechanisms = new Descriptor(0x0000000000000040, "amqp:sasl-mechanisms:list");
        public static readonly Descriptor SaslInit = new Descriptor(0x0000000000000041, "amqp:sasl-init:list");
        public static readonly Descriptor SaslChallenge = new Descriptor(0x0000000000000042, "amqp:sasl-challenge:list");
        public static readonly Descriptor SaslResponse = new Descriptor(0x0000000000000043, "amqp:sasl-response:list");
        public static readonly Descriptor SaslOutcome = new Descriptor(0x0000000000000044, "amqp:sasl-outcome:list");
    }
}
