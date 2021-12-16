using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ERP.Furacao.Application.DTOs.Account;
using ERP.Furacao.Application.Services;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Infrastructure.Crosscutting.Interfaces.Logs;
using System;
using System.Threading.Tasks;

namespace ERP.Furacao.WebApi.Controllers.v1.Accounts
{
    /// <summary>
    /// Account Controller
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [ControllerName("Account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, ILogService logService)
            : base(logService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = await DoInLog(async () =>
             await _accountService.AuthenticateAsync(request, IpAddress),
              request,
              "authenticate"
            );

            if (response.IsNotNull())
            {
                this.SetTokenCookie(response.To<Application.Wrappers.Response<AuthenticationResponse>>().Data.RefreshToken);
                return Ok(response);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var response = await DoInLog(async () =>
               await _accountService.RegisterAsync(request, Request.Headers["origin"], IpAddress, ApiVersion),
               request,
               "register"
            );

            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(UpdateRequest request)
        {
            var response = await DoInLog(async () =>
               await _accountService.UpdateAsync(request),
               request,
               "update"
            );

            return Ok(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var response = await DoInLog(async () =>
               await _accountService.DeleteAsync(id),
               id,
               "delete"
            );

            return Ok(response);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            var response = await DoInLog(async () =>
              await _accountService.GetAll(),
              "get-all",
              "list"
           );

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await DoInLog(async () =>
               await _accountService.GetById(id),
               id,
               "get-by-id"
            );

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            var response = await DoInLog(async () =>
               await _accountService.ConfirmEmailAsync(userId, code),
               new { userId, code },
               "confirm-email"
            );

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            var response = await DoInLog(async () =>
               await _accountService.ForgotPassword(model, Request.Headers["origin"], ApiVersion),
               model,
               "forgot-password"
            );

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            var response = await DoInLog(async () =>
              await _accountService.ResetPassword(model),
              model,
              "reset-password"
            );

            return Ok(response);
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken(ValidadeTokenRequest model)
        {
            var response = await DoInLog(async () =>
                await _accountService.IsTokenExpired(model),
                model,
                "validate-token"
            );

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var response = await DoInLog(async () =>
                await _accountService.RefreshToken(refreshToken, IpAddress),
                refreshToken,
                "refresh-token"
            );

            var result = response.To<Application.Wrappers.Response<AuthenticationResponse>>();
            SetTokenCookie(result.Data.RefreshToken);
            return Ok(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeTokenRequest request)
        {
            var token = request.RefreshToken ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            if (!_accountService.OwnsToken(token) && User.IsInRole("Admin"))
                return Unauthorized(new { message = "Unauthorized" });

            var response = await DoInLog(async () =>
                await _accountService.RevokeToken(token, IpAddress),
                request,
                "revoke-token"
            );

            return Ok(response);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
