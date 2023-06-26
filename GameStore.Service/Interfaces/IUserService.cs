
using GameStore.Domain.Dto.User;
using GameStore.Domain.Enums;
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Account;

namespace GameStore.Service.Interfaces
{
    public interface IUserService
    {
        public Task<Response<bool>> CreateUserAsync(RegistrationViewModel viewModel);
        public Task<Response<bool>> DeleteUserAsync(int id);
        public Task<Response<bool>> ChangeRoleUser(AccessRole role, int userId);
        public Task<Response<User?>> GetUserByIdAsync(int id);
        public Task<Response<User?>> GetUserByLoginAsync(string login);
        public Task<Response<List<UserShortDto>>> GetUsersAsync(int? page, int? pageSize, string? login);
        public string CreateToken(User user, AccessRole role);
        public Task<Response<bool>> ChangePassword(string hashPassword, int userId);

    }
}
