﻿using TechChallenge.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TechChallenge.Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    
    public AuthenticateController(
        IConfiguration configuration,
        UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.UserName);

        if (userExists is not null)
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel { Success = false, Message = "Usuário já existe!" }
            );

        IdentityUser user = new()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            Email = model.Email,    
            UserName = model.UserName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ResponseModel { Success = false, Message = "Erro ao criar usuário" }
            );

        return Ok(new ResponseModel { Message = "Usuário criado com sucesso!" });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password))
        {

            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, user.Id) //Guid.NewGuid().ToString())
            };

            return Ok(new ResponseModel { Data = GetToken(authClaims) });
        }

        return Unauthorized();
    }


    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string GetAuthenticated() => $"Usuário autenticado: {User?.Identity?.Name} ";


    private TokenModel GetToken(List<Claim> authClaims)
    {
        //obtém a chave de assinatura do JWT
        var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        //Monta o TOKEN
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        //Retorna o token + validade
        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ValidTo = token.ValidTo
        };

    }
}