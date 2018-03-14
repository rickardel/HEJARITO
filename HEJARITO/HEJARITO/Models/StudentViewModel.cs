﻿//TM 2018-03-09 14:28 En nyss inloggad student ser allt viktigt m.h.a. denna View Model
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class StudentViewModel
    {
        public ICollection<Course>    Courses     { get; set; }
        public ICollection<Module>    Modules     { get; set; }
        public ICollection<Activity>  Activities  { get; set; }

        //TM 2018-03-13 05:21 Denna property lär behövas för att kunna visa hela vyn
        public ICollection<ApplicationUser> Users { get; set; }
    }
}