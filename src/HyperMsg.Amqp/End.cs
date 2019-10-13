namespace HyperMsg.Amqp
{
    public class End : DescribedList
    {
        public End() : base(KnownDescriptors.End)
        { }

        public Error Error
        {
            get => Get<Error>(0);
            set => Set(value, 0);
        }
    }
}
