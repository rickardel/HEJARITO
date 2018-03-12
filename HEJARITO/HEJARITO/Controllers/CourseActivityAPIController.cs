using HEJARITO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HEJARITO.Controllers
{
    // Data Transfer Object.
    // For use to send object from Ajax to API instead of int id
    public class CourseActivityAPIDto
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public int ActivityTypeId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
    public class CourseActivityAPIController : ApiController
    {
        private ApplicationDbContext _context;

        public CourseActivityAPIController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddActivityToModule(CourseActivityAPIDto dto) {
            var activity = new Activity()
            {
                Name = dto.Name,
                Description = dto.Description,
                ModuleId = dto.ModuleId,
                ActivityTypeId = dto.ActivityTypeId,
                StartDate = dto.StartDate,
                DeadlineDate = dto.DeadlineDate,
                EndDate = dto.EndDate
            };
            _context.Activities.Add(activity);
            _context.SaveChanges();
            return Ok(activity);

        }
    }
}
