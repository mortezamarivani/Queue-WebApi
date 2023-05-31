using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gx.Core.Services.Interfaces;
using Gx.DataLayer.Entities.Account;
using Gx.DataLayer.Repository;
using Gx.Core.Security;
using Gx.Core.DTOs.Account;
using System.Linq;

namespace Gx.Core.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constructor

        private IGenericRepository<Users> userRepository;
        private IPasswordHelper passwordHelper;

        public UserService(IGenericRepository<Users> userRepository, IPasswordHelper passwordHelper)
        {
            this.userRepository = userRepository;
            this.passwordHelper = passwordHelper;
        }

        #endregion

        #region User Section

        public async Task<List<Users>> GetAllUsers()
        {
            return await userRepository.GetEntitiesQuery().ToListAsync();
        }

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            //if (IsUserExistsByEmail(register.Email))
            //    return RegisterUserResult.EmailExists;

            if (IsUserExistsByUserName(register.UserName))
                return RegisterUserResult.UserNameExist;

            var user = new Users
            {
                Email = register.Email.SanitizeText(),
                Address = register.Address.SanitizeText(),
                FirstName = register.FirstName.SanitizeText(),
                LastName = register.LastName.SanitizeText(),
                EmailActiveCode = Guid.NewGuid().ToString(),
                Password = passwordHelper.EncodePasswordMd5(register.Password),
                UserName = register.UserName.SanitizeText(),
                Status = true,
                Phone = register.Phone,
                IsActivated=true
            };

            await userRepository.AddEntity(user);

            await userRepository.SaveChanges();

            return RegisterUserResult.Success;
        }

        public bool IsUserExistsByEmail(string email)
        {
            return userRepository.GetEntitiesQuery().Any(s => s.Email == email.ToLower().Trim());
        }

        public bool IsUserExistsByUserName(string username)
        {
            return userRepository.GetEntitiesQuery().Any(s => s.UserName.ToLower().Trim() == username.ToLower().Trim());
        }

        public async Task<LoginUserResult> LoginUser(LoginUserDTO login)
        {
            var password = passwordHelper.EncodePasswordMd5(login.Password);

            var user = await userRepository.GetEntitiesQuery()
                .SingleOrDefaultAsync(s => s.UserName.ToLower().Trim() == login.UserName.ToLower().Trim() 
                && s.Password.ToLower().Trim() == password.ToLower().Trim());

            if (user == null) return LoginUserResult.IncorrectData;

            if (!user.IsActivated) return LoginUserResult.NotActivated;

            return LoginUserResult.Success;
        }

        public async Task<Users> GetUserByEmail(string email)
        {
            return await userRepository.GetEntitiesQuery().SingleOrDefaultAsync(s => s.Email == email.ToLower().Trim());
        }

        public async Task<Users> GetUserByUserName(string username)
        {
            return await userRepository.GetEntitiesQuery().SingleOrDefaultAsync(s => s.UserName.ToLower().Trim() == username.ToLower().Trim());
        }

        public async Task<Users> GetUserByUserId(long userId)
        {
            return await userRepository.GetEntityById(userId);
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            userRepository?.Dispose();
        }

        #endregion
    }
}
