using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OmbMf.Models
{
    public class Ombudsman
    {
        public int OmbudsmanId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        // this would lead to circular references. 
        // although not impossible to handle, it is simpler not to.
        //public List<Facility> Facilities { get; set; }
    }
}