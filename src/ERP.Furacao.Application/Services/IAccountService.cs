using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ERP.Furacao.Application.DTOs.Account;
using ERP.Furacao.Application.Wrappers;

namespace ERP.Furacao.Application.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin, string ipAddress, string version);
        Task<Response<string>> UpdateAsync(UpdateRequest request);
        Task<Response<string>> DeleteAsync(Guid id);
        Task<Response<UpdateRequest>> GetById(Guid id);
        Task<Response<List<ApplicationUserRequest>>> GetAll();
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task<Response<string>> ForgotPassword(ForgotPasswordRequest model, string origin, string version);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<bool>> IsTokenExpired(ValidadeTokenRequest model);
        Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress);
        Task<Response<string>> RevokeToken(string token, string ipAddress);
        bool OwnsToken(string token);
    }
}
