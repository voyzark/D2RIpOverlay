using System;
using System.Runtime.Serialization;

namespace Diablo2IpFinder
{
    [Serializable]
    internal class ItemExistsException : Exception
    {
        private object item;

        public ItemExistsException()
        {
        }

        public ItemExistsException(object item)
        {
            this.item = item;
        }

        public ItemExistsException(string message) : base(message)
        {
        }

        public ItemExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ItemExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}