using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gx.Core.DTOs.Account;
using Gx.Core.Services.Interfaces;
using Gx.Core.Utilities.Common;
using Gx.Core.Utilities.Extensions.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Gx.WebApi.Controllers
{

    public class AccountController : SiteBaseController
    {
        #region costructor

        private IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        #endregion

        #region Register

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO register)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.RegisterUser(register);

                switch (res)
                {
                    //case RegisterUserResult.EmailExists:
                    //    return JsonResponseStatus.Error(new { info = "Email Exist" });
                    
                    case RegisterUserResult.UserNameExist:
                        return JsonResponseStatus.Error(new { info = "UserName Exist" });

                }
            }

            return JsonResponseStatus.Success();
        }

        #endregion

        #region Login

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO login)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.LoginUser(login);

                switch (res)
                {
                    case LoginUserResult.IncorrectData:
                        return JsonResponseStatus.NotFound(new { message = "کاربری با مشخصات وارد شده یافت نشد" });

                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error(new { message = "حساب کاربری شما فعال نشده است" });

                    case LoginUserResult.Success:
                        var user = await userService.GetUserByUserName(login.UserName);
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:44381",
                            claims: new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            },
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        return JsonResponseStatus.Success(new
                        {
                            token = tokenString,
                            expireTime = 30,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            userId = user.Id,
                            email = user.Email,
                            userName=user.UserName
                        });
                }
            }

            return JsonResponseStatus.Error();
        }


        [HttpPost("login-otp")]
        public async Task<IActionResult> Loginotp([FromBody] OtpDTO OtpDTO)
        {
            if (ModelState.IsValid)
            {
                var res = await userService.LoginUserWithOtp(OtpDTO);

                switch (res)
                {
                    case LoginUserResult.IncorrectData:
                        return JsonResponseStatus.NotFound(new { message = "کد وارد شده معتبر نمی باشد" });

                    case LoginUserResult.NotActivated:
                        return JsonResponseStatus.Error(new { message = "حساب کاربری شما فعال نشده است" });

                    case LoginUserResult.Success:
                        var user = await userService.GetUserByPhone(OtpDTO.Phone);
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AngularEshopJwtBearer"));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokenOptions = new JwtSecurityToken(
                            issuer: "https://localhost:44381",
                            claims: new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                            },
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: signinCredentials
                        );

                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                        return JsonResponseStatus.Success(new
                        {
                            token = tokenString,
                            expireTime = 30,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            userId = user.Id,
                            email = user.Email,
                            userName = user.UserName
                        });
                }
            }

            return JsonResponseStatus.Error();
        }


        #endregion

        #region Check User Authentication

        [HttpPost("check-auth")]
        public async Task<IActionResult> CheckUserAuth()
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

                var user = await userService.GetUserByUserId(User.GetUserId());
                return JsonResponseStatus.Success(new
                {
                    userId = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    address = user.Address,
                    email = user.Email,
                });

        }

        #endregion

        #region Sign Out

        [HttpGet("sign-out")]
        public async Task<IActionResult> LogOut()
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

           await HttpContext.SignOutAsync();
           return JsonResponseStatus.Success();

        }

        #endregion
    }
}