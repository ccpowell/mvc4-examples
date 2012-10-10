//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/2/2009 3:55:45 PM
// Description:
//
//======================================================
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel;
using System.Web.Security;
using DRCOG.Domain.Security;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using DRCOG.Common.Services.MemberShipServiceSupport;
using System.Security.Principal;

namespace DRCOG.Domain.Models
{
    #region Models
    //[PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //public class ChangePasswordModel
    //{
    //    [Required]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Current password")]
    //    public string OldPassword { get; set; }

    //    [Required]
    //    [ValidatePasswordLength]
    //    [DataType(DataType.Password)]
    //    [DisplayName("New password")]
    //    public string NewPassword { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Confirm new password")]
    //    public string ConfirmPassword { get; set; }
    //}

    //public class LogOnModel
    //{
    //    [Required]
    //    //[RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", ErrorMessage = "Not a valid email")]
    //    [DisplayName("User name (e-mail address)")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Password")]
    //    public string Password { get; set; }

    //    [DisplayName("Remember me?")]
    //    public bool RememberMe { get; set; }
    //}

    //[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    //public class RegisterModel
    //{
    //    //[Required]
    //    //[DisplayName("User name")]
    //    //public string UserName { get; set; }

    //    [Required]
    //    [DataType(DataType.EmailAddress)]
    //    //[RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", ErrorMessage = "Not a valid email")]
    //    [DisplayName("Email address")]
    //    public string Email { get; set; }

    //    [Required]
    //    [ValidatePasswordLength]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Password")]
    //    public string Password { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Confirm password")]
    //    public string ConfirmPassword { get; set; }
    //}

    //[PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    //public class PasswordRecoveryModel
    //{
    //    //[Required]
    //    //[DisplayName("User name")]
    //    //public string UserName { get; set; }

    //    [Required(ErrorMessage = "Email / Username Required")]
    //    [DataType(DataType.EmailAddress)]
    //    //[RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", ErrorMessage = "Not a valid email")]
    //    [DisplayName("Email address")]
    //    public string Email { get; set; }

    //    [DisplayName("Question")]
    //    public string Question { get; set; }

    //    [Required]
    //    [DisplayName("Answer")]
    //    public string Answer { get; set; }

    //    [Required]
    //    [ValidatePasswordLength]
    //    [DataType(DataType.Password)]
    //    [DisplayName("New Password")]
    //    public string Password { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [DisplayName("Confirm password")]
    //    public string ConfirmPassword { get; set; }
    //}
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    //public interface IMembershipService
    //{
    //    int MinPasswordLength { get; }
    //    Guid PersonGuid { get; }

    //    bool ValidateUser(string userName, string password);
    //    MembershipCreateStatus CreateUser(string userName, string password, string email, string question, string answer);
    //    bool DeleteUser(string username, bool deleteAllRelatedData);
    //    bool ChangePassword(string userName, string oldPassword, string newPassword);
    //    bool ResetPassword(string userName, string newPassword, string answer);
    //    void UpdateUser(Guid guid);
    //    string GetUserName(Guid guid);
    //}

    //public class AccountMembershipService : IMembershipService
    //{
    //    private readonly MembershipProvider _provider;

    //    public AccountMembershipService()
    //        : this(null)
    //    {
    //    }

    //    public AccountMembershipService(MembershipProvider provider)
    //    {
    //        _provider = provider ?? Membership.Provider;
    //    }



    //    public int MinPasswordLength
    //    {
    //        get
    //        {
    //            return _provider.MinRequiredPasswordLength;
    //        }
    //    }


    //    public Guid PersonGuid
    //    {
    //        get
    //        {
    //            MembershipUser user = Membership.GetUser();
    //            if (user != null)
    //            {
    //                return (Guid)user.ProviderUserKey;
    //            }
    //            else { return Guid.Empty; }
    //        }
    //    }




    //    public bool ValidateUser(string userName, string password)
    //    {
    //        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
    //        if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

    //        return _provider.ValidateUser(userName, password);
    //    }

    //    public void UpdateUser(Guid guid)
    //    {
    //        MembershipUser user = _provider.GetUser(guid, false);
    //        user.IsApproved = false;
    //        _provider.UpdateUser(user);
    //    }

    //    public void UpdateUser(string userName, string email, string question, string answer)
    //    {
    //    }

    //    public string GetUserName(Guid guid)
    //    {
    //        MembershipUser user = _provider.GetUser(guid, false);
    //        return user.UserName;
    //    }

    //    public MembershipCreateStatus CreateUser(string userName, string password, string email, string question, string answer)
    //    {
    //        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
    //        if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
    //        if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");
    //        if (String.IsNullOrEmpty(question)) throw new ArgumentException("Value cannot be null or empty.", "question");
    //        if (String.IsNullOrEmpty(answer)) throw new ArgumentException("Value cannot be null or empty.", "answer");

    //        MembershipCreateStatus status;
    //        _provider.CreateUser(userName, password, email, question, answer, true, null, out status);
    //        return status;
    //    }

    //    public bool DeleteUser(string username, bool deleteAllRelatedData)
    //    {

    //        return _provider.DeleteUser(username, deleteAllRelatedData);
    //    }

    //    public bool ChangePassword(string userName, string oldPassword, string newPassword)
    //    {
    //        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
    //        if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
    //        if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

    //        // The underlying ChangePassword() will throw an exception rather
    //        // than return false in certain failure scenarios.
    //        try
    //        {
    //            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
    //            return currentUser.ChangePassword(oldPassword, newPassword);
    //        }
    //        catch (ArgumentException)
    //        {
    //            return false;
    //        }
    //        catch (MembershipPasswordException)
    //        {
    //            return false;
    //        }
    //    }

    //    public bool ResetPassword(string userName, string newPassword, string answer)
    //    {
    //        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
    //        if (String.IsNullOrEmpty(answer)) throw new ArgumentException("Value cannot be null or empty.", "answer");
    //        if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

    //        // The underlying ChangePassword() will throw an exception rather
    //        // than return false in certain failure scenarios.
    //        try
    //        {
    //            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
    //            string oldPassword = currentUser.ResetPassword(answer);
    //            if (currentUser.ChangePassword(oldPassword, newPassword))
    //            {
    //                currentUser.IsApproved = true;
    //                Membership.UpdateUser(currentUser);
    //                return true;
    //            }
    //            return false;
    //        }
    //        catch (ArgumentException)
    //        {
    //            return false;
    //        }
    //        catch (MembershipPasswordException exc)
    //        {
    //            return false;
    //        }
    //    }


    //}

    //public interface IFormsAuthenticationService
    //{
    //    void SignIn(string userName, bool createPersistentCookie);
    //    void SignOut();
    //}

    //public class FormsAuthenticationService : IFormsAuthenticationService
    //{
    //    public void SignIn(string userName, bool createPersistentCookie)
    //    {
    //        if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

    //        FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
    //    }

    //    public void SignOut()
    //    {
    //        FormsAuthentication.SignOut();
    //    }
    //}

    #endregion

    #region Validation
    //public static class AccountValidation
    //{
    //    public static string ErrorCodeToString(MembershipCreateStatus createStatus)
    //    {
    //        // See http://go.microsoft.com/fwlink/?LinkID=177550 for
    //        // a full list of status codes.
    //        switch (createStatus)
    //        {
    //            case MembershipCreateStatus.DuplicateUserName:
    //                return "Username already exists. Please enter a different user name.";

    //            case MembershipCreateStatus.DuplicateEmail:
    //                return "A username for that e-mail address already exists. Please enter a different e-mail address.";

    //            case MembershipCreateStatus.InvalidPassword:
    //                return "The password provided is invalid. Please enter a valid password value.";

    //            case MembershipCreateStatus.InvalidEmail:
    //                return "The e-mail address provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidAnswer:
    //                return "The password retrieval answer provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidQuestion:
    //                return "The password retrieval question provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidUserName:
    //                return "The user name provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.ProviderError:
    //                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

    //            case MembershipCreateStatus.UserRejected:
    //                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

    //            default:
    //                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
    //        }
    //    }
    //}

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

    //public interface IPerson
    //{
    //    bool IsInRole(string role);
    //    void Save();
    //}

    //public partial class Person : IPerson
    //{
    //    public Profile profile;
    //    //protected AuthenticationService.AuthenticationServiceClient AuthenticationService;

    //    public IMembershipService MembershipService { get; set; }
    //    public ProfileService ProfileService { get; set; }
    //    public UserService

    //    public Person()
    //    {
    //        Initialize();
    //    }

    //    public Person(string userName)
    //    {
    //        Initialize();
    //        profile.Email = userName;
    //    }

    //    private void Initialize()
    //    {
    //        profile = new Profile();
    //        if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
    //        if (ProfileService == null) { ProfileService = new ProfileService(); }
    //        //AuthenticationService = new DRCOG.Domain.AuthenticationService.AuthenticationServiceClient();
    //    }

    //    public virtual bool IsInRole(string role)
    //    {
    //        foreach (string s in profile.Roles)
    //        {
    //            if (s.ToLower().Equals(role.ToLower()))
    //            {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }

    //    public void Load()
    //    {
    //        //profile = AuthenticationService.GetPerson(profile.Email);

    //        Profile person = new Profile() { Email = profile.Email };
    //        ProfileService.Load(ref person);
    //        profile = person;
    //    }

    //    public void LoadRoles(IPrincipal user)
    //    {
    //        profile.Roles = ((RolePrincipal)user).GetRoles();
    //    }

    //    public bool ValidateUser(string userName, string password)
    //    {
    //        //return AuthenticationService.ValidateUser(userName, password);
    //        return MembershipService.ValidateUser(userName, password);
    //    }

    //    public virtual void Save()
    //    {
    //        try
    //        {
    //            //Your objects PersonGUID should be populated before running this process
    //            if (!this.profile.PersonGUID.Equals(""))
    //            {
    //                string sqlConn = ConfigurationManager.ConnectionStrings["DRCOG"].ToString();
    //                using (SqlConnection conn = new SqlConnection(sqlConn))
    //                {
    //                    conn.Open();

    //                    //Save Person information (UserInfo & Address)

    //                    using (SqlCommand cmd = new SqlCommand("[dbo].[CreatePerson]", conn) { CommandType = CommandType.StoredProcedure })
    //                    {
    //                        cmd.Parameters.AddWithValue("@PersonGUID", this.profile.PersonGUID);
    //                        cmd.Parameters.AddWithValue("@FirstName", this.profile.FirstName);
    //                        cmd.Parameters.AddWithValue("@LastName", this.profile.LastName);

    //                        SqlParameter param = new SqlParameter("@PersonID", SqlDbType.Int);
    //                        param.Direction = ParameterDirection.Output;
    //                        cmd.Parameters.Add(param);

    //                        try
    //                        {
    //                            int rowsAffected = cmd.ExecuteNonQuery();
    //                            if (rowsAffected < 1) { this.profile.Success = false; throw new Exception("Save to Person table was not successful"); }
    //                            this.profile.Success = true;
    //                            this.profile.PersonID = (int)cmd.Parameters["@PersonID"].Value;

    //                        }
    //                        catch (Exception exc)
    //                        {

    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //throw new Exception("Error during 'Person.Save()' process. Error:" + ex.Message);
    //        }
    //    }

    //}

    }

