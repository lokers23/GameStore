using GameStore.API.Extensions;
using GameStore.API.Helpers;
using GameStore.Domain.Enums;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Account;
using GameStore.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegistrationViewModel registrationModel)
        {
            var response = new Response<bool>() { Data = true, Status = HttpStatusCode.Ok, Message = "Регистрация прошла успешно" };
            if (!ModelState.IsValid)
            {
                response.Status = HttpStatusCode.ValidationError;
                response.Message = "Ошибка валидации";
                response.Errors = ModelState.AllErrors();

                return BadRequest(response);
            }

            var user = await _userService.GetUserByLoginAsync(registrationModel.Login);
            if (user != null)
            {
                ModelState.AddModelError(nameof(registrationModel.Login), "Такой логин уже занят");
                response.Status = HttpStatusCode.ValidationError;
                response.Message = "Ошибка валидации";
                response.Errors = ModelState.AllErrors();

                return BadRequest(response);
            }

            response = await _userService.CreateUserAsync(user.Data);
            if (!response.Data || (int)response.Status >= 500)
            {
                return StatusCode((int)response.Status, response);
            }

            return Ok(response);
        }

        [HttpGet("signin")]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
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
            if (user.Status == HttpStatusCode.NotFound || !AccountHelper.CheckCorrectPassword(user.Data, loginModel.Password))
            {
                ModelState.AddModelError("singin", "Неверный логин или пароль");
                response.Status = HttpStatusCode.AuthorizeError;
                response.Message = "Ошибка входа в аккаунт";
                response.Errors = ModelState.AllErrors();

                return Unauthorized(response);
            }

            if ((int)user.Status >= 300)
            {
                return StatusCode((int)user.Status, response);
            }

            var role = user.Data.RoleId == null ? AccessRole.User : (AccessRole)user.Data.RoleId;
            var tokenResponse = new ResponseJwt()
            {
                Token = _userService.CreateToken(user.Data, AccessRole.User.ToString()),
                Status = HttpStatusCode.Ok,
                Role = role.ToString()
            };

            return Ok(tokenResponse);
        }
    }
}
