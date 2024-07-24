using Base.Domain.Utils.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ApplicationSettings.User
{
    public class UserLoginSettings : ISetting
    {
        public int AllowedAccessFailedCount { get; set; } = 5;

        public int AccessFailedLockoutMinutes { get; set; } = 5;

        public int ManyAccessFailedLockoutMinutes { get; set; } = 60;

        public int CodeExpireMinute { get; set; } = 5;
        public int CodeDailyRetry { get; set; } = 4;

        public int TempCodeDailyRetry { get; set; } = 2;
        public int TempCodeExpireMinute { get; set; } = 7;

        public string AppDefaultPassword { get; set; } = "123456";
    }
}
