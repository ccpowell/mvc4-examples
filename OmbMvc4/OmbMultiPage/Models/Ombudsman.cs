using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OmbMultiPage.Models
{
    public class Ombudsman
    {
        public int OmbudsmanId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}