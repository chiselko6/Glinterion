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
        private IUserRepository usersDb;
        private IAccountRepository accountsDb;
        private IRoleRepository rolesDb;

        public CustomMembershipProvider()
        {
            usersDb = (IUserRepository) ((IDependencyScope) GlobalConfiguration.Configuration.DependencyResolver).GetService(typeof (IUserRepository));
            accountsDb = (IAccountRepository)((IDependencyScope)GlobalConfiguration.Configuration.DependencyResolver).GetService(typeof(IAccountRepository));
            rolesDb = (IRoleRepository)((IDependencyScope)GlobalConfiguration.Configuration.DependencyResolver).GetService(typeof(IRoleRepository));

        }

        public MembershipUser CreateUser(RegisterViewModel register)
        {
            if (usersDb.GetUser(u => u.Login == register.Login) != null)
            {
                return null;
            }

            var user = Mapper.Map<RegisterViewModel, User>(register);
            user.Account = accountsDb.GetAccount(acc => acc.Name == "Basic");
            user.Role = rolesDb.GetRole(u => u.Name == "user");
            user.Password = GetMD5Hash(user.Password);

            usersDb.AddUser(user);
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

            var userObj = usersDb.GetUser(u => u.Login == userLogin && u.Password == sha1Pswd);

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
            User user = usersDb.GetUser(u => u.Login == userLogin);

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