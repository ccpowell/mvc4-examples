using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services.MemberShipServiceSupport;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DRCOG.Common.Services.MemberShipServiceSupport.Validation.Attributes;

namespace DRCOG.Domain.Security
{
    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public class PasswordRecoveryModel
    {
        //[Required]
        //[DisplayName("User name")]
        //public string UserName { get; set; }

        [Required(ErrorMessage = "Email / Username Required")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", ErrorMessage = "Not a valid email")]
        [DisplayName("Email address")]
        public string Email { get; set; }

        [DisplayName("Question")]
        public string Question { get; set; }

        [Required]
        [DisplayName("Answer")]
        public string Answer { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
