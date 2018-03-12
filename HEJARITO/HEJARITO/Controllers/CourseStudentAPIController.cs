using HEJARITO.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class CourseStudentAPIDto
    {
        public int CourseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }

    public class CourseStudentAPIController : ApiController
    {

        private ApplicationDbContext _context;

        public CourseStudentAPIController()
        {
            _context = new ApplicationDbContext();
        }

        // To Do : validering av icka å ä ö osv

        [HttpPost]
        public IHttpActionResult AddStudentToCourse(CourseStudentAPIDto dto)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var user = new Models.ApplicationUser()
            {
                CourseId = dto.CourseId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };
            var result = userManager.Create(user, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }
            else
            {
                userManager.AddToRole(user.Id, Role.Student);
            }
            return Ok(user);
        }
    }
}
