//TM 2018-03-09 14:28 En nyss inloggad student ser allt viktigt m.h.a. denna View Model
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class StudentViewModel
    {
        public IList<Course>      Courses     { get; set; }
        public IList<Module>      Modules     { get; set; }
        public IList<Activity>    Activities  { get; set; }
    }
}