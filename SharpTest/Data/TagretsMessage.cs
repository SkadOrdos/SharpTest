using WebSharp;
using WebSharp.Services;

namespace WebSharp
{
    internal class TagretsMessage : DispatcherMessage
    {
        public const string CODE = "Targets";
        public override string TypeCode => TagretsMessage.CODE;

        public TagretsMessage(object[] data)
        {
            this.Data = data;
        }

        public object[] Data { get; }
    }
}
