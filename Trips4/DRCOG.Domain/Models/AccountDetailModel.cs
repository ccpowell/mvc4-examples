using System;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.Models
{
    public class AccountDetailModel
    {
        /// <summary>
        /// Base constructor that just creates a new empty
        /// Account
        /// </summary>
        public AccountDetailModel()
        {
            AccountDetail = new Person();
            this.CanEdit = false;
        }
        /// <summary>
        /// The current account detail
        /// </summary>
        public Person AccountDetail { get; set; }


        /// <summary>
        /// Get the full name
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            return this.AccountDetail.profile.LastName + ", " + this.AccountDetail.profile.FirstName;
        }

        /// <summary>
        /// Can the current user edit this model
        /// </summary>
        public bool CanEdit { get; set; }


    }
}
