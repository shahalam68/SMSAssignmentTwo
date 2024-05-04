using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMSDataAccessLayer.Contacts;
using SMSDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Athentication
{
    public class JWTManagerRepository : IJWTManagerRepository
	{
        IStudentRepository _studentRepository;
 
        private readonly IConfiguration configuration;
		public JWTManagerRepository(IConfiguration configuration, IStudentRepository studentRepository)
		{
			this.configuration = configuration;
			_studentRepository = studentRepository;
		}
		public async Task<Tokens> Authenticate(StudentCredential studentCredential)
		{

			Student student  = await _studentRepository.GetByUserName(studentCredential.UserName);

            if ((student == null) || (student.Password != studentCredential.Password))
            {
				Tokens ret = new Tokens();
				ret.Token = null;
				ret.RefreshToken = null;
                return ret;
            }


            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
			    Subject = new ClaimsIdentity(new Claim[]
			    {
				  new Claim("StudentName", student.Name),
				  new Claim(ClaimTypes.Role, student.Role),
				  new Claim(ClaimTypes.Email, student.Email),
				  new Claim("UserName", student.UserName),
			    }),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new Tokens { Token = tokenHandler.WriteToken(token) };

		}

	}
}
