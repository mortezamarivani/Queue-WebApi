using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gx.Core.DTOs.Account;
using Gx.DataLayer.Entities.Account;

namespace Gx.Core.Services.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<List<Users>> GetAllUsers();
        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        bool IsUserExistsByEmail(string email);
        Task<LoginUserResult> LoginUser(LoginUserDTO login);
        Task<Users> GetUserByEmail(string email);
        Task<Users> GetUserByUserName(string userName);
        Task<Users> GetUserByUserId(long userId);
    }
}