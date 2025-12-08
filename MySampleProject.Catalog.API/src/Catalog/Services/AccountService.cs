
using Microsoft.EntityFrameworkCore;
using MyEshopOnContainers.MySampleProject.Catalog.API.src.Catalog;
using Microsoft.AspNetCore.Identity;

public class AccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult>  CreateAccountAsync(RegisterInputModel model)
    {
        var newUser = new ApplicationUser
        {
            CardHolderName = model.CardHolderName,
            SecurityNumber = model.SecurityNumber,
            CardNumber = model.CardNumber,
            CardType = model.CardType,
            UserName = model.Username
        };

           // The AccountService.cs approach
        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded)
        {
            // If validation fails, we manually aggregate the errors and throw a specific
            // InvalidOperationException to bubble up the details to the API controller.
            var errors = result.Errors
                .Select(e => $"Code: {e.Code}, Description: {e.Description}")
                .Aggregate((current, next) => current + "; " + next);

            throw new InvalidOperationException($"Account creation failed. Identity Errors: {errors}");
        }
        // If succeeded, return result.
        return result;
    }

    public async Task<SignInResult> LoginUser(LoginInputModel model)
    {
        var token = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);
        return token;
    }
}