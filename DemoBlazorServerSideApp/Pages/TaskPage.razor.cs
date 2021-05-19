using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBlazorServerSideApp.Pages
{
    public class EmployeeDetails
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public bool Gender { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime? DOB { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Range(0, 20, ErrorMessage = "The Experience range should be 0 to 20")]
        public decimal? TotalExperience { get; set; }
    }

    public partial class TaskPage
    {
    }
}
