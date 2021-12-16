using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ERP.Furacao.Application.DTOs.Account;
using ERP.Furacao.Application.DTOs.Mail;
using ERP.Furacao.Application.Enums;
using ERP.Furacao.Application.Exceptions;
using ERP.Furacao.Application.Services;
using ERP.Furacao.Application.Wrappers;
using ERP.Furacao.Domain.Extensions;
using ERP.Furacao.Domain.Settings;
using ERP.Furacao.Infrastructure.Identity.Helpers;
using ERP.Furacao.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Furacao.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private const int MINUTES_VALIDITY_TOKEN = 30;
        private readonly UserManager<IdentityApplicationUser> _userManager;
        private readonly SignInManager<IdentityApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AccountService(UserManager<IdentityApplicationUser> userManager,
                              SignInManager<IdentityApplicationUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptions<JWTSettings> jwtSettings,
                              IEmailService emailService,
                              IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _config = config;
            _emailService = emailService;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.Tokens).FirstOrDefaultAsync(u => u.UserName == request.UserName);

                if (user == null || !user.EmailConfirmed)
                {
                    throw new ApiException("Email or password is incorrect");
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
                    if (!result.Succeeded)
                    {
                        throw new ApiException($"Invalid Credentials for '{request.UserName}'.");
                    }
                }

                var refreshToken = GenerateRefreshToken(ipAddress);

                var auth = await GenerateAuthentication(user, refreshToken);

                return new Response<AuthenticationResponse>(new AuthenticationResponse
                {
                    Id = auth.Id,
                    JWToken = auth.JWToken,
                    UserName = auth.UserName,
                    Email = auth.Email,
                    Roles = auth.Roles,
                    Created = auth.Created,
                    RefreshToken = auth.RefreshToken
                },
                $"Authenticated [{user.UserName}] User.");

            }
            catch (Exception ex)
            {
                return new Response<AuthenticationResponse>(ex.Message, "Error");
            }
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin, string ipAddress, string version)
        {
            try
            {
                var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
                if (userWithSameUserName != null)
                    return new Response<string>($"Username '{request.UserName}' is already taken.", "Error");

                var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
                if (userWithSameEmail.IsNull())
                {
                    var user = new IdentityApplicationUser
                    {
                        Email = request.Email,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        UserName = request.UserName
                    };

                    var roleBasicName = RoleEnum.Basic.ToString();
                    var roleBasic = await _roleManager.FindByNameAsync(roleBasicName);

                    if (roleBasic.IsNull())
                        return new Response<string>($"Unregistered Role '{roleBasicName}'.", "Error");

                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, roleBasicName);
                        var verificationUri = await SendVerificationEmail(user, origin, version);

                        await _emailService.SendAsync(new EmailRequest()
                        {
                            From = _config["MailSettings:EmailFrom"],
                            To = user.Email,
                            Body = $"Please confirm your account by visiting this URL {verificationUri}",
                            Subject = "Confirm Registration"
                        });

                        return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                    }
                    else
                        return new Response<string>($"{string.Join(" / ", result.Errors.Select(e => e.Description))}", "Error");
                }
                else
                    return new Response<string>($"Email [{request.Email }] is already registered.", "Warning");
            }
            catch (Exception ex)
            {
                return new Response<string>(ex.Message, "Error");
            }
        }

        public async Task<Response<string>> UpdateAsync(UpdateRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.UserName = request.UserName;
                user.Email = request.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new Response<string>(user.Id, message: $"User Update.");
                }
                else
                {
                    throw new ApiException($"{result.Errors.FirstOrDefault().Description}");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<string>> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    throw new ApiException($"User not found.");
                }

                var userDeleted = await _userManager.DeleteAsync(user);
                if (userDeleted.Succeeded)
                {
                    return new Response<string>(message: $"Account removed.");
                }
                else
                {
                    throw new ApiException($"Problem to remove account, contact support team.");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<UpdateRequest>> GetById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    var userResponse = new UpdateRequest
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName
                    };

                    return new Response<UpdateRequest>(userResponse, message: "");
                }
                else
                {
                    throw new ApiException($"Account not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<List<ApplicationUserRequest>>> GetAll()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                if (users != null)
                {
                    List<ApplicationUserRequest> listUsers = new List<ApplicationUserRequest>();
                    foreach (var user in users)
                    {
                        var userResponse = new ApplicationUserRequest
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                        };
                        listUsers.Add(userResponse);
                    }

                    return new Response<List<ApplicationUserRequest>>(listUsers, message: "");
                }
                else
                {
                    throw new ApiException($"Account not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        private async Task<JwtSecurityToken> GenerateJWToken(IdentityApplicationUser user)
        {

            try
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                var roleClaims = new List<Claim>();

                for (int i = 0; i < roles.Count; i++)
                {
                    roleClaims.Add(new Claim("roles", roles[i]));
                }

                string ipAddress = IPHelper.GetIpAddress();

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
                .Union(userClaims)
                .Union(roleClaims);

                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ValidForMinutes),
                    signingCredentials: signingCredentials);
                return jwtSecurityToken;
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, user) = GetRefreshToken(token);

            var newRefreshToken = GenerateRefreshToken(ipAddress);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            return new Response<AuthenticationResponse>(await GenerateAuthentication(user, newRefreshToken), $"Updated Token for User [{user.UserName}].");
        }

        private async Task<AuthenticationResponse> GenerateAuthentication(IdentityApplicationUser user, IdentityRefreshToken<string> refreshToken)
        {
            user.RefreshTokens.Add(refreshToken);
            var tokenSecurity = new JwtSecurityTokenHandler().WriteToken(await GenerateJWToken(user));

            if (user.Tokens.Any())
                user.Tokens.Clear();

            user.Tokens.Add(new IdentityApplicationToken
            {
                UserId = user.Id,
                LoginProvider = user.UserName,
                Name = user.FirstName,
                Value = tokenSecurity,
                Created = DateTime.UtcNow,
            });

            await _userManager.UpdateAsync(user);

            return new AuthenticationResponse()
            {
                Id = user.Id,
                JWToken = tokenSecurity,
                UserName = user.UserName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false),
                Created = DateTime.UtcNow,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public async Task<Response<string>> RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, account) = GetRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await _userManager.UpdateAsync(account);

            return new Response<string>("Token Revoked!");
        }

        private async Task<string> SendVerificationEmail(IdentityApplicationUser user, string origin, string version)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var _enpointUri = new Uri($"{origin}/internal/api/v{version}/account/confirm-email/");
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userID", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);

                return result.Succeeded ? new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the Account/authenticate endpoint.")
                                        : throw new ApiException($"An error occured while confirming {user.Email}.");
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        private IdentityRefreshToken<string> GenerateRefreshToken(string ipAddress)
        {
            return new IdentityRefreshToken<string>
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenValidForMinutes),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<Response<string>> ForgotPassword(ForgotPasswordRequest model, string origin, string version)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.Email);

                // always return ok response to prevent email enumeration
                if (account == null)
                    return new Response<string>("Usuário não encontrado.", "Error");

                var code = await _userManager.GeneratePasswordResetTokenAsync(account);
                var _enpointUri = new Uri($"{origin}/internal/api/v{version}/account/reset-password/");
                var emailRequest = new EmailRequest()
                {
                    Body = $"You reset token is - {code}",
                    To = model.Email,
                    Subject = "Reset Password",
                };

                await _emailService.SendAsync(emailRequest);

                return new Response<string>("Solicitação de alteração de senha enviada.");
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var account = await _userManager.FindByEmailAsync(model.Email);
                if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
                var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return new Response<string>(model.Email, message: $"Password Resetted.");
                }
                else
                {
                    throw new ApiException($"Error occured while reseting the password.");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }

        }

        public async Task<Response<bool>> IsTokenExpired(ValidadeTokenRequest model)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.Tokens).FirstOrDefaultAsync(u => u.Id == model.UserId);

                if (user.IsNull())
                    return new Response<bool>($"Usuário não encontrado para o Id {model.UserId}.");

                var userToken = user.Tokens.FirstOrDefault(x => x.Value == model.Token);

                if (userToken.IsNull())
                    return new Response<bool>("Token não encontrado.");
                else
                {
                    var validadeToken = userToken.Created.AddMinutes(MINUTES_VALIDITY_TOKEN);
                    return new Response<bool>(true, validadeToken >= DateTime.UtcNow ? "Token Válido!" : "Token Expirado!");
                }
            }
            catch (Exception ex)
            {
                throw new ApiException($"{ex.Message}");
            }
        }

        public (IdentityRefreshToken<string>, IdentityApplicationUser) GetRefreshToken(string token)
        {
            var userManager = _userManager.Users
                                          .Include(x => x.RefreshTokens)
                                          .Include(x => x.Tokens);

            var account = userManager.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new ApiException("Invalid token!");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new ApiException("Refresh Token Expired!");

            _userManager.UpdateAsync(account).Wait();

            return (refreshToken, account);
        }

        public bool OwnsToken(string token)
        {
            var account = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new ApiException("Invalid token");

            return true;
        }
    }
}
