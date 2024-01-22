using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemUI.Models
{
    public class Employee
    {
        public int ID { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public string? Department { get; set; }

        public DateTime DateOfJoining { get; set; }

        public decimal Salary { get; set; }
    }
}
