
using Microsoft.AspNetCore.Mvc;
using MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog;

[ApiController]
[Route("api/[Controller]")]
public class AccountController : Controller
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register/")]
    public async Task<ActionResult<ApplicationUser>> Register(RegisterInputModel model)
    {
        try
        {
            var account = await _accountService.CreateAccountAsync(model);
            return Ok(account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {                
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.ToString() });
        } 
    }

    // Login 
    // This method will handle two different scenarios: the initial GET request (showing the form) and the subsequent POST request (handling credentails)
    [HttpPost("login/")]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        try
        {
            var token = await _accountService.LoginUser(model);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.ToString() });
        }

    }

}