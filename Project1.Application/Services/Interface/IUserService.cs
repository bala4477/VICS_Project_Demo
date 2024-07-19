using Project1.Application.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Services.Interface
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);

        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto> CreateAsync(CreateUserDto createUserDto);

        Task UpdateAsync(UpdateUserDto updateUserDto);

        Task DeleteAsync(int id);

        Task<IEnumerable<UserDto>> GetByNameAsync(string name);
    }
}
