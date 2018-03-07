using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

        //public virtual ICollection<Document> Documents { get; set; }

    }
}