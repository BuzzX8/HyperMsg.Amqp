namespace HyperMsg.Amqp
{
    public class Close : DescribedList
    {
        public Close() : base(KnownDescriptors.Close)
        { }

        public Error Error
        {
            get => Get<Error>(0);
	        set => Set(value, 0);
        }
    }
}
