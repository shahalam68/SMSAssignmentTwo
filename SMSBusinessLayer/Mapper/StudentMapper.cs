using AutoMapper;
using SMSDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSBusinessLayer.Services
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<AddStudentsRequest,Student>();
        }
    }
}
