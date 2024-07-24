using Base.Domain.Utils.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ApplicationSettings.Security
{
    public class JwtSettings : ISetting
    {
        public string Key { get; set; } = "Enter Key";
        //TODO : You should creat a open ssl file and append it to the path you have specified
        private string _filePath = @"\App_Data\Security\" + "certificate.pfx";
        public string FilePath
        {
            get
            {
                return Environment.CurrentDirectory + _filePath;
            }
            set
            {
                _filePath = value;
            }
        }

        public string PEMKey { get; set; } = "";

        public string ExportKey { get; set; } = "";

        public string Issuer { get; set; } = "";

        public int ExpireTimeCount { get; set; } = 4;
        public TimeSpan ExpireTime
        {
            get
            {
                return TimeSpan.FromHours(ExpireTimeCount);
            }
        }

        public int RememberMeDayCount { get; set; } = 7;

        public string Audience { get; set; } = "";

        public string HashAlgorithm { get; set; } = SecurityAlgorithms.RsaSha256;
    }
}
