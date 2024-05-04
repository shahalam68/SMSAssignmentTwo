using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDataAccessLayer.Models
{
    public class StudentsQuery
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = string.Empty;
    }
}
