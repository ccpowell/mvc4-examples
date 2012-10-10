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
    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Email / Username Required")]
        [DataType(DataType.Text)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string ConfirmPassword { get; set; }
    }
}
