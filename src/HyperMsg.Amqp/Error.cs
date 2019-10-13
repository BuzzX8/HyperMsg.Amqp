namespace HyperMsg.Amqp
{
    public class Error : DescribedList
    {
        public Error() : base(KnownDescriptors.Error)
        { }

        public string Name
        {
            get => Get<string>(0);
	        set => Set(value, 0);
        }

        public string Description
        {
            get => Get<string>(1);
	        set => Set(value, 1);
        }
    }
}
