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
using AutoMapper;
using GameStore.Domain.Constants;
using GameStore.Domain.Dto.User;
using GameStore.Domain.Helpers;
using GameStore.Domain.ViewModels.Account;

namespace GameStore.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository, ILogger<UserService> logger, IConfiguration configuration, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Response<List<UserShortDto>>> GetUsersAsync(int? page, int? pageSize, string? login)
        {
            try
            {
                var response = new Response<List<UserShortDto>>();
                var users =  _repository
                    .GetAll()
                    .Where(user => 
                    (string.IsNullOrEmpty(login) || user.Login.StartsWith(login)));
                
                if (page.HasValue && pageSize.HasValue)
                {
                    var totalDevelopers = await users.CountAsync();
                    var hasNextPage = totalDevelopers > page * pageSize;
                    var hasPreviousPage = page > 1;
                
                    users =  users
                        .Skip((page.Value - 1) * pageSize.Value)
                        .Take(pageSize.Value);
                    
                    response.HasPreviousPage = hasPreviousPage;
                    response.HasNextPage = hasNextPage;
                }
                
                response.Data = await users.Select(user => _mapper.Map<UserShortDto>(user))
                    .ToListAsync();
                response.Status = HttpStatusCode.Ok;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<List<UserShortDto>, UserService>(exception, _logger);
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
                    response.Message = MessageResponse.NotFoundEntity;
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
                response.Data = true;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool, UserService>(exception, _logger);
                return response;
            }
        }
        public async Task<Response<bool>> ChangeRoleUser(AccessRole role, int userId)
        {
            try
            {
                var response = new Response<bool>();
                var user = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                user.Role = role;
                await _repository.UpdateAsync(user);

                response.Status = HttpStatusCode.NoContent;
                response.Data = true;
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


        public async Task<Response<bool>> ChangePassword(string hashPassword, int userId)
        {
            try
            {
                var response = new Response<bool>();
                var user = await _repository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = MessageResponse.NotFoundEntity;
                    return response;
                }

                user.Password = hashPassword;
                await _repository.UpdateAsync(user);

                response.Status = HttpStatusCode.NoContent;
                response.Data = true;
                return response;
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool, UserService>(exception, _logger);
                return response;
            }
        }
    }
}