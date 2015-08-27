using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Glinterion.DAL;
using Glinterion.DAL.IRepository;
using Glinterion.Models;
using Glinterion.ViewModels;

namespace Glinterion.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        private IGenericRepository<User> usersDb;
        private IGenericRepository<Account> accountsDb;
        private IGenericRepository<Role> rolesDb;

        public CustomMembershipProvider()
        {
            var uof = DependencyResolver.Current.GetService<IUnitOfWork>();
            usersDb = uof.Repository<User>();
            accountsDb = uof.Repository<Account>();
            rolesDb = uof.Repository<Role>();

        }

        public MembershipUser CreateUser(RegisterViewModel register)
        {
            if (usersDb.Get(u => u.Login == register.Login) != null)
            {
                return null;
            }

            var user = Mapper.Map<RegisterViewModel, User>(register);
            user.Account = accountsDb.Get(acc => acc.Name == "Base");
            user.Role = rolesDb.Get(u => u.Name == "user");
            user.Password = GetMD5Hash(user.Password);

            usersDb.Add(user);
            usersDb.Save();

            return GetUser(user.Login, false);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
           // ValidatePasswordEventArgs args =
           //new ValidatePasswordEventArgs(username, password, true);
           // OnValidatingPassword(args);

           // if (args.Cancel)
           // {
           //     status = MembershipCreateStatus.InvalidPassword;
           //     return null;
           // }

           // if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
           // {
           //     status = MembershipCreateStatus.DuplicateEmail;
           //     return null;
           // }

           // MembershipUser user = GetUser(username, true);

           // if (user == null)
           // {
           //     var userObj = new AppUser();
           //     userObj.Username = username;
           //     userObj.Password = GetMD5Hash(password);
           //     userObj.Email = email;
           //     userObj.DateCreated = DateTime.UtcNow;
           //     userObj.LastActivityDate = DateTime.UtcNow;

           //     entities.AppUsers.Add(userObj);
           //     entities.SaveChanges();

           //     status = MembershipCreateStatus.Success;

           //     return GetUser(username, true);
           // }
           // else
           // {
           //     status = MembershipCreateStatus.DuplicateUserName;
           // }

            throw new NotImplementedException();
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string userLogin, string password)
        {
            string sha1Pswd = GetMD5Hash(password);

            var userObj = usersDb.Get(u => u.Login == userLogin && u.Password == sha1Pswd);

            if (userObj != null)
                return true;
            return false;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string userLogin, bool userIsOnline)
        {
            User user = usersDb.Get(u => u.Login == userLogin);

            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("CustomMembershipProvider", userLogin, user.UserId, user.Email, string.Empty, string.Empty,
                                        true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                return memUser;
            }

            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}