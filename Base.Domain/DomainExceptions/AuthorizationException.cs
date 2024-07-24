using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.DomainExceptions
{
    public class AuthorizationException : DomainException
    {
        public AuthorizationException(string message) : base(message)
        {
        }

        public AuthorizationException() : base("Do not have access to this operation!")
        {
        }
    }
}
