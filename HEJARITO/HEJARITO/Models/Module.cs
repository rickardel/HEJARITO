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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Startdatum")]
        [CheckModuleStartDateVSCourseStartDate]
        [CheckModuleStartAndEndDateVSAllModulesStartAndEndDates1]   // Hör ihop med motsvarande check för "EndDate" nedan!
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "En modul måste ha ett slutdatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Slutdatum")]
        [CheckModuleEndDateVSModuleStartDate]
        [CheckModuleEndDateVSCourseEndDate]
        [CheckModuleStartAndEndDateVSAllModulesStartAndEndDates2]   // Hör ihop med motsvarande check för "StartDate" ovan!
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "KursId")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        //TM 2018-03-09 11:51 Navigational property för att kunna lista alla aktiviteter inom aktuell modul
        public virtual ICollection<Activity> Activities { get; set; } //TM (denna NP har funnits redan från början)

        public virtual ICollection<Document> Documents { get; set; }
    }

    public class CheckModuleEndDateVSModuleStartDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Module typedObjectInstance = (Module)validationContext.ObjectInstance;
            DateTime moduleStartDate = typedObjectInstance.StartDate;
            DateTime moduleEndDate = (DateTime)value;

            int result = DateTime.Compare(moduleEndDate, moduleStartDate);

            if (result > 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Modulens slutdatum måste vara senare än eller lika med modulens startdatum!");
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

            if (result > 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Modulens startdatum måste vara senare än eller lika med kursens startdatum!");
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

            int result = DateTime.Compare(moduleEndDate, courseEndDate);

            if (result < 0 || result == 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Modulens slutdatum måste vara tidigare än eller lika med kursens slutdatum!");
            }
        }
    }

    public class CheckModuleStartAndEndDateVSAllModulesStartAndEndDates1 : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Module typedObjectInstance = (Module)validationContext.ObjectInstance; // Sätt typen för validContext.ObjectInstance till "Module"
            DateTime IncommingModuleStartDate = typedObjectInstance.StartDate; // Föreslaget startdatum för modul som ska skapas
            DateTime IncommingModuleEndDate = typedObjectInstance.EndDate; // Föreslaget slutdatum för modul som ska skapas
            int moduleId = typedObjectInstance.Id; // Id för inkommande modul
            int result1 = 0; // Används för att lagra resultatet av datumjämförelser
            int result2 = 0; // Används för att lagra resultatet av datumjämförelser
            bool success1 = false;
            bool success2 = false;
            bool moduleSuccess = false;
            bool courseSuccess = true;
            string temporaryTestErrorMessage = null;

            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Course course = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId); // Den aktuella kursen

            List<Module> modules = new List<Module>(); // Skapa en tom lista för att kunna lagra alla moduler i
            foreach (var module in course.Modules) // Bygg upp en lista med alla moduler för den aktuella kursen
            {
                if (module.Id != moduleId)
                {
                    modules.Add(module);
                }
            }

            // |<---------------------Course------------------->|
            // |CS                                            CE|
            // |   |<-M1->|<-M2->|<-M3->|          |<-M4->|     |
            // |...|S1  E1|S2  E2|S3  E3|..........|S4  E4|.....|
            //          X  X    X      X            X    X
            //   |<-M5->|  |<M6>|      |<-M7->|   |<--M8-->|
            //    Case 1   Case 2       Case 3      Case 4

            // I = Incomming module, C = Current module, S = StartDate, E = EndDate
            // Loopa igenom alla befintliga moduler inom den aktuella kursen och kontrollera:
            // Case 1, 2, 3 och 4: OK if (IS < CS och IE < CS) eller (IS > CE och IE > CE)
            // Om OK för ALLA moduler => Success! Annars skriv ut felmeddelande ang. felaktigt start- och/eller slutdatum!

            foreach (var module in modules)
            {
                success1 = false;
                success2 = false;
                moduleSuccess = false;
                result1 = DateTime.Compare(IncommingModuleStartDate, module.StartDate);
                if (result1 < 0)
                {
                    result2 = DateTime.Compare(IncommingModuleEndDate, module.StartDate);
                    if (result2 < 0)
                    {
                        success1 = true;
                    }
                    else
                    {
                        success1 = false;
                        temporaryTestErrorMessage = "IS < CS och IE < CS ej uppfyllt!";
                    }
                }

                result1 = DateTime.Compare(IncommingModuleStartDate, module.EndDate);
                if (result1 > 0)
                {
                    result2 = DateTime.Compare(IncommingModuleEndDate, module.EndDate);
                    if (result2 > 0)
                    {
                        success2 = true;
                    }
                    else
                    {
                        success2 = false;
                        temporaryTestErrorMessage = "IS > CE och IE > CE ej uppfyllt!";
                    }
                }

                moduleSuccess = success1 || success2; // Jämförelsen med denna modul har gått bra om success1 eller success2 är sant

                courseSuccess = courseSuccess && moduleSuccess; // Jämförelsen med alla moduler har gått bra om moduleSuccess är sant för varje modul
            }

            if (courseSuccess)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Något datum för den nya modulen överlappar annan moduls datum!");
            }
        }
    }

    public class CheckModuleStartAndEndDateVSAllModulesStartAndEndDates2 : ValidationAttribute
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Module typedObjectInstance = (Module)validationContext.ObjectInstance; // Sätt typen för validContext.ObjectInstance till "Module"
            DateTime IncommingModuleStartDate = typedObjectInstance.StartDate; // Föreslaget startdatum för modul som ska skapas
            DateTime IncommingModuleEndDate = typedObjectInstance.EndDate; // Föreslaget slutdatum för modul som ska skapas
            int moduleId = typedObjectInstance.Id; // Id för inkommande modul
            int result1 = 0; // Används för att lagra resultatet av datumjämförelser
            int result2 = 0; // Används för att lagra resultatet av datumjämförelser
            bool success1 = false;
            bool success2 = false;
            bool moduleSuccess = false;
            bool courseSuccess = true;
            string temporaryTestErrorMessage = null;

            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            Course course = applicationDbContext.Courses.FirstOrDefault(c => c.Id == typedObjectInstance.CourseId); // Den aktuella kursen

            List<Module> modules = new List<Module>(); // Skapa en tom lista för att kunna lagra alla moduler i
            foreach (var module in course.Modules) // Bygg upp en lista med alla moduler för den aktuella kursen
            {
                if (module.Id != moduleId)
                {
                    modules.Add(module);
                }
            }

            // |<---------------------Course------------------->|
            // |CS                                            CE|
            // |   |<-M1->|<-M2->|<-M3->|          |<-M4->|     |
            // |...|S1  E1|S2  E2|S3  E3|..........|S4  E4|.....|
            //          X  X    X      X            X    X
            //   |<-M5->|  |<M6>|      |<-M7->|   |<--M8-->|
            //    Case 1   Case 2       Case 3      Case 4

            // I = Incomming module, C = Current module, S = StartDate, E = EndDate
            // Loopa igenom alla befintliga moduler inom den aktuella kursen och kontrollera:
            // Case 1, 2, 3 och 4: OK if (IS < CS och IE < CS) eller (IS > CE och IE > CE)
            // Om OK för ALLA moduler => Success! Annars skriv ut felmeddelande ang. felaktigt start- och/eller slutdatum!

            foreach (var module in modules)
            {
                success1 = false;
                success2 = false;
                moduleSuccess = false;
                result1 = DateTime.Compare(IncommingModuleStartDate, module.StartDate);
                if (result1 < 0)
                {
                    result2 = DateTime.Compare(IncommingModuleEndDate, module.StartDate);
                    if (result2 < 0)
                    {
                        success1 = true;
                    }
                    else
                    {
                        success1 = false;
                        temporaryTestErrorMessage = "IS < CS och IE < CS ej uppfyllt!";
                    }
                }

                result1 = DateTime.Compare(IncommingModuleStartDate, module.EndDate);
                if (result1 > 0)
                {
                    result2 = DateTime.Compare(IncommingModuleEndDate, module.EndDate);
                    if (result2 > 0)
                    {
                        success2 = true;
                    }
                    else
                    {
                        success2 = false;
                        temporaryTestErrorMessage = "IS > CE och IE > CE ej uppfyllt!";
                    }
                }

                moduleSuccess = success1 || success2; // Jämförelsen med denna modul har gått bra om success1 eller success2 är sant

                courseSuccess = courseSuccess && moduleSuccess; // Jämförelsen med alla moduler har gått bra om moduleSuccess är sant för varje modul
            }

            if (courseSuccess)
            {
                return ValidationResult.Success;
            }
            else
            {
                
                return new ValidationResult("Något datum för den nya modulen överlappar annan moduls datum!");
            }
        }
    }
}