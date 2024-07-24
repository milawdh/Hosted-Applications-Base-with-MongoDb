using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.DomainExceptions
{
    public class InvalidArgumentException : DomainException
    {
        public InvalidArgumentException(string message) : base(message)
        {
        }
        public InvalidArgumentException(List<string> messages) : base(string.Join(";", messages)) { }
    }
}
