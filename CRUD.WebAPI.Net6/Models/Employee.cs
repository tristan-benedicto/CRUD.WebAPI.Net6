using System.ComponentModel.DataAnnotations;

namespace CRUD.WebAPI.Net6.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Temperature { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
