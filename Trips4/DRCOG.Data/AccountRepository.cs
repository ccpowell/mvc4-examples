using System;
using DRCOG.Domain.Interfaces;
using System.Web.Security;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using System.Web.Profile;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Data
{
    /// <summary>
    /// Account Repository
    /// </summary>
    /// <remarks>Assumes unique UserName and unique Email.</remarks>
    public class AccountRepository : BaseRepository, IAccountRepository
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public System.Web.Security.MembershipCreateStatus CreateUserWithProfile(Common.Services.MemberShipServiceSupport.Profile profile, bool p)
        {
            try
            {
                // TODO: use TransactionScope
                var user = Membership.CreateUser(profile.UserName, profile.Password, profile.RecoveryEmail);
                user.IsApproved = p;
                Membership.UpdateUser(user);

                // TODO: profile properties
                // copy from ProfileService in Common
                var mp = (MemberProfile)ProfileBase.Create(user.UserName, user.IsApproved);
                mp.HomeAddress = profile.Address;
                mp.Save();
            }
            catch (MembershipCreateUserException cex)
            {
                Logger.ErrorException("failed to create user", cex);
                return cex.StatusCode;
            }
            return MembershipCreateStatus.Success;
        }

        public void AddUserToRole(string userName, string role)
        {
            throw new NotImplementedException();
        }

        public Common.Services.MemberShipServiceSupport.Profile GetUserByName(string userName, bool loadRoles)
        {
            throw new NotImplementedException();
        }

        public Common.Services.MemberShipServiceSupport.Profile GetUserByID(Guid guid, bool loadRoles)
        {
            throw new NotImplementedException();
        }

        public Common.Services.MemberShipServiceSupport.Profile GetUserByEmail(string emailAddress, bool loadRoles)
        {
            var found = Membership.FindUsersByEmail(emailAddress);
            if (found.Count == 0)
            {
                throw new Exception("Email not found.");
            }
            if (found.Count > 1)
            {
                Logger.Error("Multiple users found with email address {0}", emailAddress);
                throw new Exception("Multiple users found with email address " + emailAddress);
            }
        }
        public void UpdateUserApproval(Guid userId, bool isApproved)
        {
            var found = Membership.GetUser(userId);
            if (found == null)
            {
                throw new Exception("User not found.");
            }
            if (found.IsApproved != isApproved)
            {
                found.IsApproved = isApproved;
                Membership.UpdateUser(found);
            }
        }

        public void ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            var found = Membership.GetUser(id);
            if (found == null)
            {
                throw new Exception("User not found.");
            }
            var result = found.ChangePassword(oldPassword, newPassword);
            if (!result)
            {
                throw new Exception("Failed to change password.");
            }
        }

        public string ResetPassword(Guid id)
        {
            var found = Membership.GetUser(id);
            if (found == null)
            {
                throw new Exception("User not found.");
            }
            return found.ResetPassword();
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }
    }
}
