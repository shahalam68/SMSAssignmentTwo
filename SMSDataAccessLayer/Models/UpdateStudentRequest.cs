using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDataAccessLayer.Models
{
    public class UpdateStudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime EnrolmentDate { get; set; }

    }
}
