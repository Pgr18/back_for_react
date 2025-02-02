﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_WebApp.DbContexts;
using My_WebApp.Models.DTO;
using My_WebApp.Models.Employees;

namespace My_WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class FilesController : ControllerBase
    {
        const string FILES_DIRECTORY = "filesStorage";
        private ApplicationContext _context;

        public FilesController()
        {
            _context = new ApplicationContext();
        }

        [HttpPost("download")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult DownloadFile([FromBody] DownloadFileRequestDto downloadFileRequestDto)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                if (!Directory.Exists(path))
                {
                    return BadRequest("File not found");
                }
                var fileBytes = System.IO.File.ReadAllBytes(Path.Combine(path, downloadFileRequestDto.SystemName));
                return File(fileBytes, "application/octet-stream", downloadFileRequestDto.DisplayName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("upload")]
        [Authorize(Roles ="admin,manager")]
        public IActionResult UploadFile([FromBody] FileRequestDto fileDto)
        {
            try
            {
                var employee = _context.Employees.Include(e => e.UserFiles)
                    .FirstOrDefault(e => e.Id == fileDto.EmployeeId);
                if (employee == null)
                {
                    return BadRequest("User not found");
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var fileBytes = Convert.FromBase64String(fileDto.FileString);
                var systemFileName = Guid.NewGuid().ToString();
                System.IO.File.WriteAllBytes(Path.Combine(path, systemFileName), fileBytes);

                if (employee.UserFiles == null)
                {
                    employee.UserFiles = new List<UserFile>();
                }
                employee.UserFiles.Add(new UserFile
                {
                    DisplayName = fileDto.FileName,
                    SystemName = systemFileName
                });
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("delete")]
        [Authorize(Roles = "admin,manager")]
        public IActionResult DeleteFile(int id)
        {
            try
            {
                var file = _context.UserFiles.FirstOrDefault(f => f.Id == id);
                if (file != null)
                {
                    var systemName = file.SystemName;
                    _context.UserFiles.Remove(file);
                    _context.SaveChanges();

                    var path = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIRECTORY);
                    if (!Directory.Exists(path))
                    {
                        return BadRequest("File not found in file system");
                    }
                    System.IO.File.Delete(Path.Combine(path, systemName));

                    return Ok();
                }
                else
                {
                    return BadRequest("File not found in data base");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
