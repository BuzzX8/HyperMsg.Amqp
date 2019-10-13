namespace HyperMsg.Amqp
{
    public class SaslMechanisms : DescribedList
    {
        public SaslMechanisms() : base(KnownDescriptors.SaslMechanisms)
        { }

        public Symbol[] Mechanisms => Get<Symbol[]>(0);
    }
}
