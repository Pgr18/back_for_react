﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_WebApp.DbContexts;
using My_WebApp.Models.DTO;
using My_WebApp.Models.Employees;

namespace My_WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private ApplicationContext _context;

        public EmployeesController()
        {
            _context = new ApplicationContext();
        }
        [HttpGet("employees")]
        [Authorize(Roles = "admin,manager")]

        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees.Include(e => e.WorkExperience).Include(e => e.Education).Include(e=>e.UserFiles).ToList();
        }

        [HttpPost("employee")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult CreateEmployee([FromBody] EmployeeRequestDto employeeDto)
        {
            var department = _context.Departments.Include(d => d.Employees)
                .FirstOrDefault(d => d.Id == employeeDto.DepartmentId);
            if (department != null)
            {
                //var bd = DateTime.Now;
                //DateTime.TryParse(, out bd);

                var _employee = _context.Employees.Add(new Employee
                {
                    BirthDate = employeeDto.BirthDate,
                    Email = employeeDto.Email,
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    MiddleName = employeeDto.MiddleName,
                    PhoneNumber = employeeDto.PhoneNumber,
                });
                _context.SaveChanges();
                if (department.Employees == null)
                {
                    department.Employees = new List<Employee>();
                }
                department.Employees.Add(_employee.Entity);
                _context.SaveChanges();
                return Ok(_employee.Entity.Id);
            }
            return BadRequest("Department not found");
        }

        [HttpPut("employee")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult UpdateEmployee([FromBody] EmployeeUpdateRequestDto employee)
        {
            var _employee = _context.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (_employee != null)
            {
                //var bd = DateTime.Now;
               // DateTime.TryParse(employee.BirthDate, out bd);

                _employee.BirthDate = employee.BirthDate;
                _employee.Email = employee.Email;
                _employee.FirstName = employee.FirstName;
                _employee.LastName = employee.LastName;
                _employee.MiddleName = employee.MiddleName;
                _employee.PhoneNumber = employee.PhoneNumber;
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Employee not found");
        }

        [HttpDelete("employee")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult DeleteEmployee(int id)
        {
            var _employee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (_employee != null)
            {
                _context.Employees.Remove(_employee);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Employee not found");
        }


        [HttpPost("workexperience")]
        [Authorize(Roles = "admin,manager")]

        public IActionResult AddWorkExperience([FromBody]WorkExperienceRequestDto workExperience)
        {
            var _employee = _context.Employees.FirstOrDefault(e=> e.Id == workExperience.EmployeeId);
            if (_employee != null)
            {
                if(_employee.WorkExperience == null)
                {
                    _employee.WorkExperience = new List<WorkExperience>();
                }
                _employee.WorkExperience.Add(new WorkExperience
                {
                    Description = workExperience.Description,
                    WorkedYears = workExperience.WorkedYears,
                });
                _context.SaveChanges();
                var weId = _employee.WorkExperience.LastOrDefault()?.Id ?? 0;
                return Ok(weId);
            }
            return BadRequest("Employee not found");
        }

        [HttpDelete("workexperience")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult DeleteWorkExperience(int id)
        {
            var _workExperience = _context.WorkExperience.FirstOrDefault(we => we.Id == id);
            if (_workExperience != null)
            {
                _context.WorkExperience.Remove(_workExperience);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Work experience not found");
        }


        [HttpPost("education")]
        [Authorize(Roles = "admin,manager")]

        public IActionResult AddEducation([FromBody] EducationRequestDto education)
        {
            var _employee = _context.Employees.FirstOrDefault(e => e.Id == education.EmployeeId);
            if (_employee != null)
            {
                if (_employee.Education == null)
                {
                    _employee.Education = new List<Education>();
                }
                _employee.Education.Add(new Education
                {
                    Description = education.Description,
                    Title = education.Title,
                });
                _context.SaveChanges();
                var weId = _employee.Education.LastOrDefault()?.Id ?? 0;
                return Ok(weId);
            }
            return BadRequest("Employee not found");
        }

        [HttpDelete("education")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult DeleteEducation(int id)
        {
            var _education = _context.Educations.FirstOrDefault(we => we.Id == id);
            if (_education != null)
            {
                _context.Educations.Remove(_education);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest("Education entry not found");
        }


    }
}
