using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_WebApp.DbContexts;
using My_WebApp.Models.Departments;

namespace My_WebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private ApplicationContext _context;
        public DepartmentsController()
        {
            _context = new ApplicationContext();
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public IEnumerable<Department> GetDepartments()
        {
            return _context.Departments
                .Include( d=> d.Employees).ThenInclude(e=>e.Education)
                .Include(d=>d.Employees).ThenInclude(e=>e.WorkExperience)
                .Include(d => d.Employees).ThenInclude(e => e.UserFiles)
                .ToList();
        }

        [HttpPost("departmnet")]
        [Authorize(Roles ="admin")]

        public IActionResult AddDepartments(string name, string? description)
        {
            var department = _context.Departments.Add(new Department { Name = name, Description = description });
            _context.SaveChanges();

            return Ok(department.Entity?.Id ?? 0);
        }

        [HttpPut("department")]
        [Authorize(Roles = "admin")]

        public IActionResult UpdateDepartment(int id, string name, string? description)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (department != null)
            {
                department.Name = name;
                department.Description = description;
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Department not found");
        }
        [HttpDelete("department")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.FirstOrDefault(d => d.Id == id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Department not found");
        }
    }
}
