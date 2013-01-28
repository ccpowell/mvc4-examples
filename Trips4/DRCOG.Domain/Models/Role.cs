//======================================================
// Author: dbouwman
// Date Created: 7/3/2009 10:00:51 AM
// Description:
//
//======================================================
using System.Runtime.Serialization;
//using DTS.Extensions;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class Role
    {

        public Role()
        {

        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="id">The id.</param>
        public Role(string name, int id, string roleType)
        {
            this.RoleId = id;
            this.Name = name;
            this.RoleType = roleType;
        }

        /// <summary>
        /// Gets or sets the role ID.
        /// </summary>
        /// <value>The id.</value>        
        public  int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        /// <value>The name.</value>        
        public  string Name { get; set; }

        /// <summary>
        /// The type of the role...
        /// </summary>
        public string RoleType { get; set; }
    }
}
