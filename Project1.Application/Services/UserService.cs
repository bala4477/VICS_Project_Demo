using AutoMapper;
using Project1.Application.DTO.Users;
using Project1.Application.Exceptions;
using Project1.Application.Services.Interface;
using Project1.Domain.Contracts;
using Project1.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public UserService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }
        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            var validator = new CreateBrandDtoValidator();
            var validationResult = await validator.ValidateAsync(createUserDto);
            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid User Input", validationResult);
            }
            var user = _mapper.Map<Users>(createUserDto);
            var createdEntity = await _usersRepository.CreateAsync(user);
            var entity = _mapper.Map<UserDto>(createUserDto);
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _usersRepository.GetByIdAsync(x => x.Id == id);
            await _usersRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {

            var users = await _usersRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _usersRepository.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetByNameAsync(string name)
        {
            var users = await _usersRepository.GetByNameAsync(name);
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task UpdateAsync(UpdateUserDto updateUserDto)
        {
            var user = _mapper.Map<Users>(updateUserDto);
            await _usersRepository.UpdateAsync(user);

        }
    }
}
