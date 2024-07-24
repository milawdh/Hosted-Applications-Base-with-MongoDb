using AspNetCore.Identity.MongoDbCore.Models;
using Base.Domain.Api;
using Base.Domain.ApplicationSettings.User;
using Base.Domain.Audities;
using Base.Domain.DomainExceptions;
using Base.Domain.Entities.Base.Access;
using Base.Domain.Enums.Base.User;
using Base.Domain.Models.EntityModels.Permissions;
using Base.Domain.RepositoriesApi.Context;
using Base.Domain.Utils.Configuration;
using Base.Domain.Utils.Extentions.Security;

using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Base.Domain.Entities.Base.Users
{

    public partial class UserCollection : IdentityUser<ObjectId>, IDeletetationAuditedEntity<ObjectId>, IModificationAuditedEntity<ObjectId>, IHasCustomMap
    {

        public string Name { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public GenderEnum Gender { get; set; }

        public string ImageProfileBase64 { get; set; }

        public override bool PhoneNumberConfirmed { get; set; } = false;

        public string BankCartNumber { get; set; }

        public List<int> RoleId { get; set; }
        public List<UserRoleCollection> Role { get; set; }

        public List<PermissionListEmbeededModel> CustomPermissions { get; set; } = new();

        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeExpireDate { get; set; }
        public int VerificationDailyRetry { get; set; } = 0;

        #region Full Audited

        public bool IsDeleted { get; set; }
        public UserType? DeleterUserType { get; set; }
        public ObjectId? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        public UserType? ModifierUserType { get; set; }
        public ObjectId? ModifiedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public UserCollection DeleterUser { get; set; }
        public CustomerCollection DeleterCustomer { get; set; }
        public UserCollection ModiferUser { get; set; }
        public CustomerCollection ModiferCustomer { get; set; }

        #endregion



        public void ValidateAdd(DomainValidationContext validationContext)
        {
            if (validationContext.Core.Users.Any(x => x.UserName == UserName))
                throw new InvalidArgumentException("نام کاربری نمیتواند تکراری باشد");

            if (!PhoneNumber.IsNullOrEmpty())
            {
                var phoneRegex = new Regex("@\"^\\+98\\d{10}$\"");
                if (phoneRegex.IsMatch(PhoneNumber))
                    throw new InvalidArgumentException("شماره تلفن وارد شده نامعتبر است!");

                if (validationContext.Core.Users.Any(x => x.PhoneNumber == PhoneNumber))
                    throw new InvalidArgumentException("این شماره تلفن قبلا ثبت شده میباشد!");
            }

            if (Email.IsNullOrEmpty())
                throw new InvalidArgumentException("ایمیل نمیتواند خالی باشد!");

            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            if (!emailRegex.IsMatch(Email))
                throw new InvalidArgumentException("ایمیل کاربر نامعتبر میباشد!");

            if (validationContext.Core.Users.Any(a => a.Email == Email.ToLower()))
                throw new InvalidArgumentException("این ایمیل قبلا ثبت شده میباشد!");

            FullName = Name + LastName;
        }

        public void ValidateDelete(DomainValidationContext validationContext)
        {
        }

        public void ValidateUpdate(DomainValidationContext validationContext)
        {
            if (validationContext.Core.Users.Any(x => x.Id != Id && x.UserName == UserName))
                throw new InvalidArgumentException("نام کاربری نمیتواند تکراری باشد");

            if (validationContext.Core.Users.Any(x => x.Id != Id && x.PhoneNumber == PhoneNumber))
                throw new InvalidArgumentException("این شماره تلفن قبلا ثبت شده میباشد!");

            if (!Email.IsNullOrEmpty())
                if (validationContext.Core.Users.Any(x => x.Id != Id && x.Email == Email.ToLower()))
                    throw new InvalidArgumentException("این ایمیل قبلا ثبت شده میباشد!");

            if (!PhoneNumber.IsNullOrEmpty())
            {
                var phoneRegex = new Regex("@\"^\\+98\\d{10}$\"");
                if (phoneRegex.IsMatch(PhoneNumber))
                    throw new InvalidArgumentException("شماره تلفن وارد شده نامعتبر است!");

                if (validationContext.Core.Users.Any(x => x.PhoneNumber == PhoneNumber))
                    throw new InvalidArgumentException("این شماره تلفن قبلا ثبت شده میباشد!");
            }

            if (Email.IsNullOrEmpty())
                throw new InvalidArgumentException("ایمیل نمیتواند خالی باشد!");

            FullName = Name + LastName;
        }

        public void ConfigMap()
        {
        }

        public void SeedData(DomainSeedContext seedContext)
        {
            seedContext.MainCore.Users.DropCollection();
            var settings = SingleTon<AppSettings>.Instance.Get<UserLoginSettings>();

            seedContext.MainCore.Users.Add(new UserCollection()
            {
                AccessFailedCount = 0,
                Email = "milaad5674@gmail.com",
                FullName = "Milad Hashemi",
                Name = "Milad",
                LastName = "Hashemi",
                UserName = "MilaadHs",
                PasswordHash = settings.AppDefaultPassword.HashData(),
                RoleId = new List<int> { 1 },
                Id = ObjectId.Parse("66966858f505629c7995a3e6"),
                PhoneNumber = "+98924121204",
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                LockoutEnabled = false,
                Gender = GenderEnum.Male,
            });
        }
    }

}
