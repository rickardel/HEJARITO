using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class TeacherViewModel
    {
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}