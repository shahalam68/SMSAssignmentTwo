using SMSDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSDataAccessLayer.Contacts
{
    public interface IStudentRepository
    {
        Task<bool> CreateStudent(Student student);
        Task<Student> GetByUserName(string userName);
        Task<List<Student>> GetAllStudents(int pageNumber, int pageSize, string sortBy);
        Task<Student> GetStudent(Guid id);
    }
}
