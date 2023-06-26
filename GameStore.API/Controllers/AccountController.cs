using GameStore.API.Extensions;
using GameStore.API.Helpers;
using GameStore.Domain.Constants;
using GameStore.Domain.Enums;
using GameStore.Domain.Helpers;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Account;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationViewModel registrationModel)
        {
            try
            {
                var response = new Response<bool>()
                {
                    Data = true, 
                    Status = HttpStatusCode.Ok, 
                    Message = "Регистрация прошла успешно"
                };
                
                if (!ModelState.IsValid)
                {
                    response.Status = HttpStatusCode.ValidationError;
                    response.Message = "Ошибка валидации";
                    response.Errors = ModelState.AllErrors();

                    return BadRequest(response);
                }
                
                var responseByLogin = await _userService.GetUserByLoginAsync(registrationModel.Login);
                if (responseByLogin.Data != null)
                {
                    ModelState.AddModelError(nameof(registrationModel.Login), "Такой логин уже занят");
                    response.Status = HttpStatusCode.ValidationError;
                    response.Message = "Ошибка валидации";
                    response.Errors = ModelState.AllErrors();
                
                    return BadRequest(response);
                }

                var hash = AccountHelper.HashPassword(registrationModel.Password, registrationModel.Login);
                registrationModel.Password = hash;
                registrationModel.ConfirmPassword = hash;
            
                response = await _userService.CreateUserAsync(registrationModel);
                
                if ((int)response.Status >= 300)
                {
                    return StatusCode((int)response.Status, response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, AccountController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
        {
            try
            {
                var response = new Response<bool>();
                if (!ModelState.IsValid)
                {
                    response.Status = HttpStatusCode.ValidationError;
                    response.Message = "Ошибка валидации";
                    response.Errors = ModelState.AllErrors();
                    return BadRequest(response);
                }

                var user = await _userService.GetUserByLoginAsync(loginModel.Login);
                if (user.Status == HttpStatusCode.NotFound ||
                    !AccountHelper.CheckCorrectPassword(user.Data, loginModel.Password, user.Data.Login))
                {
                    ModelState.AddModelError("singin", "Неверный логин или пароль");
                    response.Status = HttpStatusCode.ValidationError;
                    response.Message = "Ошибка входа в аккаунт";
                    response.Errors = ModelState.AllErrors();

                    return Unauthorized(response);
                }

                if ((int)user.Status >= 300)
                {
                    return StatusCode((int)user.Status, response);
                }
                
                var tokenResponse = new ResponseJwt()
                {
                    Token = _userService.CreateToken(user.Data, user.Data.Role),
                    Status = HttpStatusCode.Ok,
                    Role = user.Data.Role.ToString()
                };

                return Ok(tokenResponse);
            }
            catch (Exception exception)
            {
                var response = Catcher.CatchError<bool?, AccountController>(exception, _logger);
                return StatusCode((int)response.Status, response);
            }
        }
    }
}
