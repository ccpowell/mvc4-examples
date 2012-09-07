using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConModels
{
    public class ContactList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public List<User> Users { get; set; }
    }
}
