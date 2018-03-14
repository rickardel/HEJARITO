using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class CourseEditor
    {
        public Course Course { get; set; }

        public CourseEditor(Course course)
        {
            this.Course = course;
            this.Module = new Module() { CourseId = course.Id, StartDate = DateTime.Now, EndDate = DateTime.Now };
            this.Activity = new Activity() { StartDate = DateTime.Now, DeadlineDate = DateTime.Now, EndDate = DateTime.Now };
            this.Student = new ApplicationUser() { CourseId = course.Id };
        }

        public Module Module { get; set; }
        public Activity Activity { get; set; }
        public ApplicationUser Student { get; set; }
    }
    
}