using AutoMapper;
using BaseAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BaseAPI.Entities.DTO;
using Microsoft.AspNetCore.Http;
using BaseAPI.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaseAPI.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public TokenController(ITokenService tokenService, ILoggerManager logger, IRepositoryWrapper repository)
        {
            _tokenService = tokenService;
            _logger = logger;
            _repository = repository;
        }

        [HttpPost, Route("[action]")]
        public IActionResult refresh(TokenDto? token)
        {
            if (token == null)
            {
                _logger.LogError("Invalid User tried refreshing their token");
                return BadRequest("Invalid client request");
            }

            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var email = principal.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

            var user = _repository.User.GetUserByEmail(email);

            if (user == null) 
                return BadRequest("Invalid client request");
            
            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogError($"User {email} tried to refresh their token but their refresh token was invalid");
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _repository.Save();
            _logger.LogInfo($"User {email} refreshed their token");

            return Ok(new Token()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });

        }

        [HttpPost, Authorize, Route("[action]")]
        public IActionResult revoke()
        {
            var email = User.Identity?.Name;

            if (email != null)
            {
                var user = _repository.User.GetUserByEmail(email);
                if (user != null)
                {
                    user.RefreshToken = null;
                    _repository.User.UpdateUser(user);
                }
                else
                {
                    _logger.LogError($"User {email} tried to revoke their refresh token but they were not found in the database");
                    return BadRequest("Invalid client request");
                }

                _logger.LogInfo($"User {email} revoked their refresh token");
            }

            _repository.Save();
            return NoContent();
        }
    }
}