using GameStore.DAL.Interfaces;
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStore.Domain.Constants;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.Account;

namespace GameStore.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        public UserService(IRepository<User> repository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<Response<List<User>>> GetUsersAsync()
        {
            try
            {
                var users = await _repository.GetAll().ToListAsync();

                var response = new Response<List<User>>()
                {
                    Data = users,
                    Status = HttpStatusCode.Ok
                };

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<User>, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<User?>> GetUserByIdAsync(int id)
        {
            try
            {
                var response = new Response<User?>()
                {
                    Status = HttpStatusCode.Ok
                };

                var user = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                response.Data = user;

                if (user == null)
                {
                    response.Message = "Пользователь не найден";
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<User?, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<User?>> GetUserByLoginAsync(string login)
        {
            try
            {
                var user = await _repository.GetAll()
                    .FirstOrDefaultAsync(u => u.Login.Equals(login));

                var response = new Response<User?>()
                {
                    Data = user,
                    Status = HttpStatusCode.Ok
                };

                if (user == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "Пользователя с таким логином не существует";

                    return response;
                }

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<User?, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> CreateUserAsync(RegistrationViewModel viewModel)
        {
            try
            {
                var response = new Response<bool>()
                {
                    Message = "Пользователь успешно создан",
                    Status = HttpStatusCode.Created
                };

                var user = new User()
                {
                    Id = 0,
                    Login = viewModel.Login,
                    Password = viewModel.Password,
                    Mail = viewModel.Mail,
                    Role = AccessRole.User,
                    Balance = 0
                };
                
                await _repository.CreateAsync(user);
                
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var response = new Response<bool>()
                {
                    Status = HttpStatusCode.NoContent
                };

                var user = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    response.Data = false;
                    response.Message = MessageResponse.NotFoundEntity;
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }

                await _repository.DeleteAsync(user);

                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> UpdateUserAsync(User user)
        {
            try
            {
                var response = new Response<bool>();
                var userForUpdate = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == user.Id);

                if (userForUpdate == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                userForUpdate.Login = user.Login;
                userForUpdate.Password = user.Password;
                userForUpdate.Balance = user.Balance;
                userForUpdate.Id = user.Id;
                userForUpdate.Role = user.Role;
                await _repository.UpdateAsync(userForUpdate);

                response.Status = HttpStatusCode.NoContent;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool, UserService>(exception, _logger);
                return response;
            }
        }
        public string CreateToken(User user, AccessRole role)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}