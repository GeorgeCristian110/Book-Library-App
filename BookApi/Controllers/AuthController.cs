using Microsoft.AspNetCore.Mvc;
using BookApi.Data;
using BookApi.DTOs.Auth;
using BookApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookApi.Services;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, PasswordHasher<User>
    passwordHasher, TokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async  Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync
                    (u => u.Email == request.Email);
        
        if(existingUser != null)
        {
            return Conflict("A user with this email is already registerd!");
        }

        var existingUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if(existingUsername != null)
        {
            return Conflict("This username is already taken try again. ");
        }

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = string.Empty,
            Role = Roles.User
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var response = new AuthResponse
        {
            Id = newUser.Id,
            Username = newUser.Username,
            Email = newUser.Email,
            Token = _tokenService.GenerateToken(newUser)
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync
                    (u => u.Email == request.Email);
        
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if(result == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid email or password");
        }

        var response = new AuthResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = _tokenService.GenerateToken(user)
        };

        return Ok(response);
    }
}