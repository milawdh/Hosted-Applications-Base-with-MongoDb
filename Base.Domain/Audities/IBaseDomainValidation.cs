using Base.Domain.RepositoriesApi.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Audities
{
    public interface IBaseDomainValidation
    {
        public void ValidateAdd(DomainValidationContext validationContext);

        public void ValidateUpdate(DomainValidationContext validationContext);

        public void ValidateDelete(DomainValidationContext validationContext);
    }
}
