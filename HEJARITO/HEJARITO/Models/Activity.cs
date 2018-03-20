using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HEJARITO.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett namn")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Namn måste bestå av minst {2} tecken.", MinimumLength = 2)]
        [Display(Name = "Aktivitetsnamn")]
        public string Name { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha en aktivitetstyp")]
        [Display(Name = "Aktivitetstyp")]
        public int ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett startdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Startdatum")]
        [CheckActivityStartDateVSCourseStartDate]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Deadlinedatum")]
        
        //TM 2018-03-09 10:04 Nullable DateTime!
        public Nullable<DateTime> DeadlineDate { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett slutdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Slutdatum")]
        [CheckActivityEndDateVSActivityStartDate]
        [CheckActivityEndDateVSCourseEndDate]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "En aktivitet måste kopplas till en module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Document> StudentDocuments { get; set; }

    }

    public class CheckActivityEndDateVSActivityStartDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Activity typedObjectInstance = (Activity)validationContext.ObjectInstance;
            DateTime activityStartDate = typedObjectInstance.StartDate;
            DateTime activityEndDate = (DateTime)value;

            int result = DateTime.Compare(activityEndDate, activityStartDate);

            if (result > 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Aktivitetens slutdatum måste vara senare än eller lika med aktivitetens startdatum!");
            }
        }
    }

    public class CheckActivityStartDateVSCourseStartDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Activity typedObjectInstance = (Activity)validationContext.ObjectInstance;
            DateTime activityStartDate = (DateTime)value;
            Module module = applicationDbContext.Modules.FirstOrDefault(m => m.Id == typedObjectInstance.ModuleId);
            DateTime courseStartDate = applicationDbContext.Courses.FirstOrDefault(c => c.Id == module.CourseId).StartDate;

            int result = DateTime.Compare(activityStartDate, courseStartDate);

            if (result > 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Aktivitetens startdatum måste vara senare än eller lika med kursens startdatum!");
            }
        }
    }

    public class CheckActivityEndDateVSCourseEndDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Activity typedObjectInstance = (Activity)validationContext.ObjectInstance;
            DateTime activityEndDate = (DateTime)value;
            Module module = applicationDbContext.Modules.FirstOrDefault(m => m.Id == typedObjectInstance.ModuleId);
            DateTime courseEndDate = applicationDbContext.Courses.FirstOrDefault(c => c.Id == module.CourseId).EndDate;

            int result = DateTime.Compare(activityEndDate, courseEndDate);

            if (result < 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Aktivitetens slutdatum måste vara tidigare än eller lika med kursens slutdatum!");
            }
        }
    }
}