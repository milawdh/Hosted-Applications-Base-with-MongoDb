using Base.Domain.Enums.Base.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Shared.ViewModels.UserLogin
{
    public class UserIdentitiedDetailsDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public GenderEnum Gender { get; set; }

        public string ImageProfileBase64 { get; set; }

        public string BankCartNumber { get; set; }

        public bool IsBankCartConfirmed { get; set; }
    }
}
