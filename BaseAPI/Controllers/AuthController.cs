using System.Diagnostics;
using BaseAPI.Entities.Models;
using BaseAPI.Entities.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using BaseAPI.Interfaces;

namespace BaseAPI.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IEncryptionService _encryptionService;

        public AuthController(IConfiguration config, ITokenService tokenService, ILoggerManager logger, IRepositoryWrapper repository, IEncryptionService encryptionService)
        {
            _configuration = config;
            _tokenService = tokenService;
            _logger = logger;
            _repository = repository;
            _encryptionService = encryptionService;
        }

        [HttpPost]
        [Route("[Action]")]
        public IActionResult login([FromBody] UserLoginDto? user)
        {
            if (user == null)
            {
                _logger.LogError("Invalid User tried logging in");
                return BadRequest("Invalid client request");
            }

            try
            {
                if (user.Email != null)
                {
                    var userFound = _repository.User.GetUserByEmail(user.Email);
                    if (user.Password != null && user.Email.ToLower() == userFound?.Email?.ToLower() && _encryptionService.Verify(user.Password, userFound.Password, userFound.Salt))
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        if (userFound is { Name: { }, Surname: { } })
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, userFound.Email),
                                new Claim(ClaimTypes.Name, userFound.Name),
                                new Claim(ClaimTypes.Surname, userFound.Surname),
                                new Claim(ClaimTypes.Role, userFound.Role),
                            };

                            var accessToken = _tokenService.GenerateAccessToken(claims);
                            var refreshToken = _tokenService.GenerateRefreshToken();

                            userFound.RefreshToken = refreshToken;
                            userFound.RefreshTokenExpiryTime = DateTime.Now.AddDays(90);

                            _repository.User.UpdateUser(userFound);

                            _repository.Save();

                            return Ok(new Token
                            {
                                AccessToken = accessToken,
                                RefreshToken = refreshToken
                            });
                        }
                        else
                        {
                            _logger.LogInfo($"User with email: {user.Email} doesn't have a name or surname.");
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        _logger.LogInfo($"User with email: {user.Email} failed to login.");
                        return Unauthorized();
                    }
                }
                else
                {
                    _logger.LogInfo($"User with email: {user.Email} doesn't exist in the database.");
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Login action: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost, Route("[action]")]
        public IActionResult register(UserRegisterDto? user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                if (user.Email != null)
                {
                    var userFound = _repository.User.GetUserByEmail(user.Email);

                    if (userFound != null)
                    {
                        _logger.LogInfo($"User with email: {user.Email} already exists in the database.");
                        return BadRequest("User already exists");
                    }
                }

                if (user.Password != null)
                {
                    var password = _encryptionService.Encrypt(user.Password, out byte[] salt);

                    var userEntity = new User
                    {
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Password = password,
                        Salt = salt,
                    };

                    _repository.User.CreateUser(userEntity);
                }
                else
                {
                    _logger.LogInfo($"User with email: {user.Email} failed to register.");
                    return BadRequest("Password is required");
                }

                _repository.Save();
                
                _logger.LogInfo($"User with email: {user.Email} successfully registered.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Register action: {ex.Message}");
                return StatusCode(500);
            }
        }
    }
}
