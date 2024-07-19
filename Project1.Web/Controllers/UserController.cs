using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Application.Common;
using Project1.Application.Constants;
using Project1.Application.DTO.Users;
using Project1.Application.Exceptions;
using Project1.Application.Input_Models;
using Project1.Application.Services.Interface;
using Project1.Domain.Contracts;
using Project1.Domain.Models;
using Project1.Infrastructure.Data;
using System.Net;

namespace Project1.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly IAuthService _authService;
        protected APIResponse _response;

        public UsersController(IUserService usersService, IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
            _response = new APIResponse();
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<APIResponse>> Register(Register register)
        {
            try
            {
               if(!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommanMessage.RegistrationFailed);
                    return _response;
                }

               var result = await _authService.Register(register);

                _response.IsSuccess = true;
                _response.statusCode=HttpStatusCode.Created;
                _response.DisplayMessage=CommanMessage.RegistrationSuccess;
                _response.Result = result;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommanMessage.SystemError);
            }
            return _response;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<APIResponse>> Login(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.AddError(ModelState.ToString());
                    _response.AddWarning(CommanMessage.LoginFailed);
                    return _response;
                }

                var result = await _authService.Login(login);

                if(result is string)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommanMessage.LoginFailed;
                }

                _response.IsSuccess = true;
                _response.statusCode = HttpStatusCode.OK;
                _response.DisplayMessage = CommanMessage.LoginSuccess;
                _response.Result = result;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommanMessage.SystemError);
            }
            return _response;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                var users = await _usersService.GetAllAsync();
                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = users;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommanMessage.SystemError);
            }
            return _response;
        }


        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<APIResponse>> Get(string name)
        {
            try
            {
                var users = await _usersService.GetByNameAsync(name);
                if (users == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommanMessage.RecordNotFound;
                    return Ok(_response);
                }
                _response.statusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = users;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.AddError(CommanMessage.SystemError);
            }
            return _response;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommanMessage.CreateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var createduser = await _usersService.CreateAsync(createUserDto);
                _response.statusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommanMessage.CreateOperationSuccess;
                _response.Result = createduser;

            }
            catch (BadRequestException ex)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommanMessage.CreateOperationFailed;
                _response.AddError(CommanMessage.SystemError);
                _response.Result = ex.ValidationError;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommanMessage.CreateOperationFailed;
                _response.AddError(CommanMessage.SystemError);
            }
            return _response;
        }


        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommanMessage.UpdateOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }
                var user = await _usersService.GetByIdAsync(updateUserDto.Id);
                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommanMessage.UpdateOperationFailed;
                }
                await _usersService.UpdateAsync(updateUserDto);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommanMessage.UpdateOperationSuccess;

            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommanMessage.UpdateOperationFailed;
                _response.AddError(CommanMessage.SystemError);
            }

            return Ok(_response);
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.DisplayMessage = CommanMessage.DeleteOperationFailed;
                    _response.AddError(ModelState.ToString());
                    return Ok(_response);
                }

                var user = await _usersService.GetByIdAsync(id);

                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.DisplayMessage = CommanMessage.DeleteOperationFailed;
                    return Ok(_response);
                }
                await _usersService.DeleteAsync(id);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _response.DisplayMessage = CommanMessage.DeleteOperationSuccess;
            }
            catch (Exception)
            {
                _response.statusCode = HttpStatusCode.InternalServerError;
                _response.DisplayMessage = CommanMessage.DeleteOperationFailed;
                _response.AddError(CommanMessage.SystemError);
            }
            return NoContent();
        }
    }
}
