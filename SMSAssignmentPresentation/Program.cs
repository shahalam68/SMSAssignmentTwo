
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SMSBusinessLayer.Services;
using StudenMangementSystem.Data.Data;
using Microsoft.Extensions.Configuration;
using NLog;
using LoggerServices;
using Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Repository.Athentication;
using System.Text;
using SMSDataAccessLayer.Contacts;
using SMSDataAccessLayer.Contracts;

namespace SMSAssignmentPresentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var Configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false)
             .Build();

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),"/nlog.config"));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
            builder.Services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

            var IsInMemory = Configuration["DatabaseConfiguration:IsInMemory"];
            var connString = Configuration["DatabaseConfiguration:ConnectionString"];

            builder.Services.AddDbContext<StudentAPIDbContext>(options =>
            {
                if (IsInMemory == "true")
                {
                    options.UseInMemoryDatabase("StudentsDb");
                }
                else
                {
                    options.UseNpgsql(connString);
                }
            }, ServiceLifetime.Singleton
            );


            builder.Services.AddAutoMapper(typeof(StudentMapper));
            builder.Services.AddSingleton<IStudentManagementServices, StudentManagementServices>();
            builder.Services.AddSingleton<ILoggerManager, LoggerManager>(); 
            builder.Services.AddSingleton<IStudentRepository, StudentRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins("https://test.com")
                .WithMethods()
                .WithHeaders());
            });

            var app = builder.Build();

            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
