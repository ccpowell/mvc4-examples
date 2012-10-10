//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/9/2009 10:50:59 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// JSON serializable object suitable for instantiating a dojo datastore.
    /// </summary>
    [DataContract]
    public class RolesDojoDataStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesDojoDataStore"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="data">The data.</param>
        public RolesDojoDataStore(string identifier, string label, IList<Role> items)
        {
            this.identifier = identifier;
            this.label = label;
            this.items = items;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The error.</value>
        //[DataMember]
        public string identifier { get; set; }

        /// <summary>
        /// Gets or sets the Label.
        /// </summary>
        /// <value>The error.</value>
        //[DataMember]
        public string label { get; set; }

        /// <summary>
        /// Gets or sets the Items.
        /// </summary>
        /// <value>The data.</value>
        [DataMember]
        public IList<Role> items { get; set; }
    }
}
