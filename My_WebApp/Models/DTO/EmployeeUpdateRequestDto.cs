﻿namespace My_WebApp.Models.DTO
{
    public class EmployeeUpdateRequestDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
    }
}
