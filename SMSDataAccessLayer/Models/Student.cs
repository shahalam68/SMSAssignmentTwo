using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDataAccessLayer.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
