using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett namn")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "ModulNamn")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett startdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Startdatum")]
        [CheckModuleStartDateVSCourseStartDate]
        [CheckModuleStartDateVSAllModulesStartAndEndDates]   // Hör ihop med motsvarande check för "EndDate" nedan!
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett slutdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slutdatum")]
        [CheckModuleEndDateVSModuleStartDate]
        [CheckModuleEndDateVSCourseEndDate]
        [CheckModuleEndDateVSAllModulesStartAndEndDates]   // Hör ihop med motsvarande check för "StartDate" ovan!
        public DateTime EndDate { get; set; }

        [Required]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        //TM 2018-03-09 11:51 Navigational property för att kunna lista alla aktiviteter inom aktuell modul
        public virtual ICollection<Activity> Activities { get; set; } //TM (denna NP har funnits redan från början)

        //public virtual ICollection<Document> Documents { get; set; }
    }

    public class CheckModuleEndDateVSModuleStartDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleStartDate = typedObjectInstance.StartDate;
            DateTime moduleEndDate = (DateTime)value;

            int result = DateTime.Compare(moduleEndDate, moduleStartDate);

            if (result < 0)
            {
                return new ValidationResult("Modulens slutdatum måste vara senare än eller lika med modulens startdatum!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CheckModuleStartDateVSCourseStartDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleStartDate = (DateTime)value;
            DateTime courseStartDate = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId).StartDate;

            int result = DateTime.Compare(moduleStartDate, courseStartDate);

            if (result < 0)
            {
                return new ValidationResult("Modulens startdatum måste vara senare än eller lika med kursens startdatum!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        
        }
    public class CheckModuleEndDateVSCourseEndDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleEndDate = (DateTime)value;
            DateTime courseEndDate = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId).EndDate;

            int result = DateTime.Compare(courseEndDate, moduleEndDate);

            if (result < 0)
            {
                return new ValidationResult("Modulens slutdatum måste vara tidigare än eller lika med kursens slutdatum!");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }

    public class CheckModuleStartDateVSAllModulesStartAndEndDates : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();

            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleStartDate = (DateTime)value;

            Course course = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId);

            List<Module> modules = new List<Module>();
            foreach (var module in course.Modules)
            {
                modules.Add(module);
            }

            //DateTime moduleEndDate = typedObjectInstance.EndDate;

            //DateTime earliestStartDate = applicationDbContext.Modules.OrderBy(s => s.StartDate).FirstOrDefault().StartDate;
            //DateTime latestEndDate = applicationDbContext.Modules.OrderByDescending(s => s.EndDate).FirstOrDefault().EndDate;

            //var modules = applicationDbContext.Modules.ToList();
            foreach (var module in modules)
            {
                DateTime dBModuleEndDate = module.EndDate;
                int result = DateTime.Compare(moduleStartDate, dBModuleEndDate);

                if (result < 0)
                {
                    return new ValidationResult("Modulens startdatum får ej överlappa med en annans moduls slutdatum!");
                }
                //else
                //{
                //    return ValidationResult.Success;
                //}
            }
            return ValidationResult.Success;
        }
    }

    public class CheckModuleEndDateVSAllModulesStartAndEndDates : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();

            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleEndDate = (DateTime)value;

            Course course = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId);

            List<Module> modules = new List<Module>();
            foreach (var module in course.Modules)
            {
                modules.Add(module);
            }

            //DateTime moduleEndDate = typedObjectInstance.EndDate;

            //DateTime earliestStartDate = applicationDbContext.Modules.OrderBy(s => s.StartDate).FirstOrDefault().StartDate;
            //DateTime latestEndDate = applicationDbContext.Modules.OrderByDescending(s => s.EndDate).FirstOrDefault().EndDate;

            //var modules = applicationDbContext.Modules.ToList();
            foreach (var module in modules)
            {
                DateTime dBModuleStartDate = module.StartDate;
                int result = DateTime.Compare(dBModuleStartDate, moduleEndDate);

                if (result < 0)
                {
                    return new ValidationResult("Modulens slutdatum får ej överlappa med en annans moduls startdatum!");
                }
                //else
                //{
                //    return ValidationResult.Success;
                //}
            }
            return ValidationResult.Success;
        }
    }
}