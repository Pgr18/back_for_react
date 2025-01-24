using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My_WebApp.Models.Employees;

namespace My_WebApp.Models.Departments
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public List<Employee> Employees { get; set; }
    }
}
