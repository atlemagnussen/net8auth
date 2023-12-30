using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using net8auth.auth.Models;
using net8auth.model;

namespace net8auth.auth.Pages;

public class LoginModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly ILogger<LoginModel> _logger;

    public LoginModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<LoginModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [BindProperty]
    public LoginInputModel Model { get; set; } = new LoginInputModel();

    public IActionResult OnGetAsync(string returnUrl)
    {
        Model = new LoginInputModel {
            ReturnUrl = returnUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string button)
    {
        _logger.LogInformation("OnPostAsync");
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Model.Username, Model.Password, Model.RememberLogin, lockoutOnFailure: true);
            _logger.LogInformation("Result success={Succeeded}", result.Succeeded);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(Model.Username);
                if (user != null)
                    _logger.LogInformation($"user: {user.UserName}");
                return Redirect("~/");
            }

            ModelState.AddModelError(string.Empty, "Error");
        }

        else
        {
            _logger.LogError("Error in Model");
            
            foreach (var field in ModelState)
            {
                if (field.Value.Errors.Count == 0)
                    continue;
                
                foreach (var error in field.Value.Errors)
                    _logger.LogError($"{error.ErrorMessage}");
            }
        }
        return Page();
    }
}
