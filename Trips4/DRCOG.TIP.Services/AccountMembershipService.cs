using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using DRCOG.Common.Security;
using System.Security.Principal;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain;
using DRCOG.Domain.Security;
using DRCOG.Domain.Models;
using DRCOG.Common.DesignByContract;
//using DRCOG.Domain.Caching;
//using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using DRCOG.Domain.Interfaces;
using System.Web.UI;
using System.Web.Security;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.TIP.Services
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }



        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }


        public Guid PersonGuid
        {
            get
            {
                MembershipUser user = Membership.GetUser();
                if (user != null)
                {
                    return (Guid)user.ProviderUserKey;
                }
                else { return Guid.Empty; }
            }
        }




        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public void UpdateUser(Guid guid)
        {
            MembershipUser user = _provider.GetUser(guid, false);
            user.IsApproved = false;
            _provider.UpdateUser(user);
        }

        public void UpdateUser(string userName, string email, string question, string answer)
        {
        }

        public string GetUserName(Guid guid)
        {
            MembershipUser user = _provider.GetUser(guid, false);
            return user.UserName;
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email, string question, string answer)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
            if (String.IsNullOrEmpty(question)) throw new ArgumentException("Value cannot be null or empty.", "question");
            if (String.IsNullOrEmpty(answer)) throw new ArgumentException("Value cannot be null or empty.", "answer");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, question, answer, true, null, out status);
            return status;
        }

        public bool DeleteUser(string username, bool deleteAllRelatedData)
        {

            return _provider.DeleteUser(username, deleteAllRelatedData);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public bool ResetPassword(string userName, string newPassword, string answer)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(answer)) throw new ArgumentException("Value cannot be null or empty.", "answer");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                string oldPassword = currentUser.ResetPassword(answer);
                if (currentUser.ChangePassword(oldPassword, newPassword))
                {
                    currentUser.IsApproved = true;
                    Membership.UpdateUser(currentUser);
                    return true;
                }
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException exc)
            {
                return false;
            }
        }


    }
}
