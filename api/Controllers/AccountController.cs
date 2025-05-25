using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using api.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, 
    SignInManager<AppUser> signInManager) : ControllerBase {
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto) {
        try {

            var appUser = new AppUser {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await userManager.CreateAsync(appUser, registerDto.Password);

            if (!createdUser.Succeeded) return BadRequest(createdUser.Errors);

            var roleResult = await userManager.AddToRoleAsync(appUser, "User");
            
            return roleResult.Succeeded 
                ? Ok(new NewUserDto {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = tokenService.CreateToken(appUser)
                }) 
                : BadRequest(roleResult.Errors);
        }
        catch (Exception e) {
            return StatusCode(500, e);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto) {
        // 1. Attempt to find user in ASP.NET Identity user store by username
        //    - Queries the AspNetUsers table using the provided username
        var user = await userManager.FindByNameAsync(loginDto.Username);  
        
        // 2. User not found
        if (user == null) return Unauthorized("Invalid username!");
        
        // 3. Verify password and account status using ASP.NET Identity's sign-in service
        //    - CheckPasswordSignInAsync handles password verification and account lockout checks
        //    - Third parameter (lockoutOnFailure: false) disables temporary account lockouts
        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        // 4. If password check failed or account is locked/disabled
        if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

        // 5. Successful authentication - return user details with JWT
        //    - NewUserDto is a Data Transfer Object containing safe user information
        //    - TokenService generates a JWT containing user claims (email, username, roles)
        return Ok(
            new NewUserDto {
                UserName = user.UserName,  // Return normalized username
                Email = user.Email,        // Return registered email
                Token = tokenService.CreateToken(user)  // Generate JWT access token
            }
        );
    }
}