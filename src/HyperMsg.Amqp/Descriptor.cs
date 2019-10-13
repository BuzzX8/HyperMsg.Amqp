using System.Text;

namespace HyperMsg.Amqp
{
    public class Descriptor
    {
        public Descriptor(ulong code, string name = "")
        {
            Code = code;
            Name = name;
        }

        public ulong Code { get; }

        public string Name { get; }

        public override int GetHashCode() => Code.GetHashCode() ^ Name.GetHashCode();

        public override bool Equals(object obj) => Equals(obj as Descriptor);

        public bool Equals(Descriptor other) => other == null ? false : Code == other.Code && Name == other.Name;

        public override string ToString()
        {
            var sb = new StringBuilder((Code >> 32).ToString("x8"));
            sb.Append(':');
            sb.Append((Code & 0x00000000ffffffff).ToString("x8"));
            return sb.ToString();
        }

        public static implicit operator Descriptor(ulong code) => new Descriptor(code);

        public static implicit operator Descriptor(string name) => new Descriptor(ulong.MaxValue, name);
    }
}
