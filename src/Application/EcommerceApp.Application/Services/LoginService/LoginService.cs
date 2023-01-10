using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Domain.Entities;
using EcommerceApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Services.LoginService
{
    public class LoginService :ILoginService
    {
        private readonly IEmployeeRepo _employeeRepo;
        public LoginService(IEmployeeRepo employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }


        Task<Employee> ILoginService.Login(LoginDTO loginDTO)
        {
            var employee = _employeeRepo.GetDefault(x => x.EmailAddress == loginDTO.EmailAddress && x.Password == loginDTO.Password);

            return employee;
        }
    }
}
