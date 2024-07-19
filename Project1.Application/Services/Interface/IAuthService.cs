using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using Project1.Application.Input_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<IEnumerable<IdentityError>> Register(Register register);

        Task<object> Login(Login login);
    }
}
