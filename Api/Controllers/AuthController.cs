using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[AllowAnonymous]
public class AuthController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    ILogger<AuthController> logger
) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost]
    [Route("/api/register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel register)
    {
        var user = new IdentityUser
        {
            UserName = register.Email,
            Email = register.Email
        };

        var result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError(error.Description);
            }
            return StatusCode(500);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return Ok();
    }

    [HttpPost]
    [Route("/api/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var result = await _signInManager.PasswordSignInAsync(
            login.Email, login.Password, login.RememberMe, lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            _logger.LogError("Invalid Login Attempt");
            return StatusCode(401);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Authorize]
    [Route("/api/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("index", "home");
    }
}
