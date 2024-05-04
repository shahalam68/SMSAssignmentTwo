using Microsoft.EntityFrameworkCore;
using SMSDataAccessLayer.Contacts;
using SMSDataAccessLayer.Models;
using StudenMangementSystem.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StudentRepository: IStudentRepository
    {
        StudentAPIDbContext _context;
        public StudentRepository(StudentAPIDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Student> GetByUserName(string userName)
        {
            Student student= await _context.Students.FirstOrDefaultAsync(i => i.UserName ==  userName);
            return student;
        }
        public async Task<List<Student>> GetAllStudents(int pageNumber, int pageSize, string sortBy)
        {
            int skip = (pageNumber - 1) * pageSize;
            var query = _context.Students.AsQueryable();

            switch (sortBy)
            {
                case "Name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "UserName":
                    query = query.OrderBy(p => p.UserName);
                    break;
                default:
                    query = query.OrderBy(p => p.Id);
                    break;
            }

            List<Student> student = await query.Skip(skip).Take(pageSize).ToListAsync();
            return student;
        }
        public async Task<Student> GetStudent(Guid id)
        {
            Student student = await _context.Students.FindAsync(id); 
            return student;
        }
    }
}
