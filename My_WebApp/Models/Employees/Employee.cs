using My_WebApp.Models.Users;

namespace My_WebApp.Models.Employees
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        //public User User { get; set; }
        public List<Education> Education { get; set; }
        public List<WorkExperience> WorkExperience { get; set; }
        public List<UserFile> UserFiles { get; set; }
    }
}
