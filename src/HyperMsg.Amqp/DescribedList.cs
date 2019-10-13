using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HyperMsg.Amqp
{
    public class DescribedList
    {
        private readonly IDictionary<int, object> values;

        public DescribedList(Descriptor descriptor)
        {
            Descriptor = descriptor;
            values = new Dictionary<int, object>();
        }

        public Descriptor Descriptor { get; }

        public object this[int i]
        {
            get
            {
	            if (!values.ContainsKey(i))
	            {
		            return null;
	            }

                return values[i];
            }
            set => values[i] = value;
        }

        public int Length => values.Count == 0 ? 0 : values.Keys.Max() + 1;

        protected T Get<T>(int index) => !(this[index] is T) ? (default) : (T)this[index];

        protected void Set<T>(T value, int index) => this[index] = value;

        public void CopyFrom(IEnumerable<object> list)
        {
            int i = 0;

            foreach(var item in list)
            {
                this[i] = item;
                i++;
            }
        }

        public IList ToList() => values.Values.ToArray();

        internal void SetValues(IList values)
        {
            this.values.Clear();

            for (int i = 0; i < values.Count; i++)
            {
                this[i] = values[i];
            }
        }
    }
}
