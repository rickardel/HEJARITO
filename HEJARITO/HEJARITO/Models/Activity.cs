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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startdatum")]
        [CheckActivityStartDateVSModuleStartDate]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Deadlinedatum")]
        
        //TM 2018-03-09 10:04 Nullable DateTime!
        public Nullable<DateTime> DeadlineDate { get; set; }

        [Required(ErrorMessage = "En aktivitet måste ha ett slutdatum")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Slutdatum")]
        [CheckActivityEndDateVSActivityStartDate]
        [CheckActivityEndDateVSModuleEndDate]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "En aktivitet måste kopplas till en module")]
        public int ModuleId { get; set; }
        public virtual Module Module { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<StudentDocument> StudentDocuments { get; set; }

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

    public class CheckActivityStartDateVSModuleStartDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Activity typedObjectInstance = (Activity)validationContext.ObjectInstance;
            DateTime activityStartDate = (DateTime)value;
            DateTime moduleStartDate = applicationDbContext.Modules.FirstOrDefault(m => m.Id == typedObjectInstance.ModuleId).StartDate;

            int result = DateTime.Compare(activityStartDate, moduleStartDate);

            if (result > 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Aktivitetens startdatum måste vara senare än eller lika med modulens startdatum!");
            }
        }
    }

    public class CheckActivityEndDateVSModuleEndDate : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Activity typedObjectInstance = (Activity)validationContext.ObjectInstance;
            DateTime activityEndDate = (DateTime)value;
            DateTime moduleEndDate = applicationDbContext.Modules.FirstOrDefault(m => m.Id == typedObjectInstance.ModuleId).EndDate;

            int result = DateTime.Compare(activityEndDate, moduleEndDate);

            if (result < 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Aktivitetens slutdatum måste vara tidigare än eller lika med modulens slutdatum!");
            }
        }
    }
}