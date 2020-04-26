using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Infrastructure.Library.Interface.Services
{
    [Serializable]
    public class EntityTranslatorException : Exception
    {
        public EntityTranslatorException() : base() { }
        public EntityTranslatorException(string message) : base(message) { }
        public EntityTranslatorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
