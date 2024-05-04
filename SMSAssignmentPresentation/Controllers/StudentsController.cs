using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSBusinessLayer.Services;
using SMSDataAccessLayer.Contacts;
using SMSDataAccessLayer.Contracts;
using SMSDataAccessLayer.Models;
using StudenMangementSystem.Data.Data;

namespace SMSAssignmentPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentAPIDbContext dbContext;
        IStudentManagementServices _studentManagementServices;
        ILoggerManager _loggerManager;
        IJWTManagerRepository _JWTManagerRepository;

        public StudentsController(
            StudentAPIDbContext dbContext, 
            IStudentManagementServices studentManagementServices, 
            ILoggerManager loggerManager,
            IJWTManagerRepository JWTManagerRepository)
        {
            _studentManagementServices = studentManagementServices;
            this.dbContext = dbContext;
            _loggerManager = loggerManager;
            _JWTManagerRepository = JWTManagerRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(StudentCredential Student)
        {
            var token = await _JWTManagerRepository.Authenticate(Student);
            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetStudents([FromQuery]StudentsQuery query)
        {
            var students = await _studentManagementServices.GetAllStudents(query);
            return Ok(students);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id:guid}")]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            var student = await _studentManagementServices.GetStudent(id);  
            if(student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddStudents(AddStudentsRequest addStudentsRequest)
        {
            _loggerManager.LogInfo("started Post request for create student");
            var IsSuccess = await _studentManagementServices.AddStudents(addStudentsRequest);
            _loggerManager.LogInfo("completed Post request for create student");
            return Ok(IsSuccess?"added":"Failed");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateStudentRequest updateStudentRequest)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student != null)
            {
                student.Name = updateStudentRequest.Name;
                student.Email = updateStudentRequest.Email;
                student.EnrolmentDate = updateStudentRequest.EnrolmentDate;
                await dbContext.SaveChangesAsync();
                return Ok(student);
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student != null)
            {
                 dbContext.Remove(student);
                await dbContext.SaveChangesAsync();
                return Ok(student);
            }
            return NotFound();
        }
    }

    
}
