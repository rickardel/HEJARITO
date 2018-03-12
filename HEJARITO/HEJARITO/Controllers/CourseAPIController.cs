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
    public class CourseAPIDto {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }

    // To Do . validering av icke å ä ö
    //[Authorize(Roles ="Teacher")]
    public class CourseAPIController : ApiController
    {
        private ApplicationDbContext _context;

        public CourseAPIController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddModuleToCourse(CourseAPIDto dto) {
            var module = new Models.Module()
            {
                CourseId = dto.CourseId,
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            _context.Modules.Add(module);
            _context.SaveChanges();
            return Ok(module);
        }

    }
}
