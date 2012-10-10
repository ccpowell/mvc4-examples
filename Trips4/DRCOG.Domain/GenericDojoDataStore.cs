//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/22/2009 11:09:52 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Collections;

namespace DRCOG.Domain
{
    /// <summary>
    /// JSON serializable object suitable for instantiating a dojo datastore.
    /// </summary>
    [DataContract]
    public class GenericDojoDataStore<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDojoDataStore"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="data">The data.</param>
        public GenericDojoDataStore(string identifier, string label, IList<T> items)
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
        public IList<T> items { get; set; }
    }
}
