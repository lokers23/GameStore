
using GameStore.Domain.Models;
using GameStore.Domain.Response;
using GameStore.Domain.ViewModels.Account;

namespace GameStore.Service.Interfaces
{
    public interface IUserService
    {
        public Task<Response<bool>> CreateUserAsync(User user);
        public Task<Response<bool>> DeleteUserAsync(int id);
        public Task<Response<bool>> UpdateUserAsync(User user);
        public Task<Response<User?>> GetUserByIdAsync(int id);
        public Task<Response<User?>> GetUserByLoginAsync(string login);
        public Task<Response<List<User>>> GetUsersAsync();
        public string CreateToken(User user, string role);

    }
}
