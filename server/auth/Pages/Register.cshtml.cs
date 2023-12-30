using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using net8auth.model;

namespace net8auth.auth.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<LoginModel> _logger;

        public RegisterModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public RegisterInputModel Model { get; set; } = new RegisterInputModel();

        public IActionResult OnGetAsync(string returnUrl)
        {
            Model = new RegisterInputModel();
            ModelState.Clear();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string button)
        {
            _logger.LogInformation("OnPostAsync");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {
                    UserName = Model.Username
                };
                var result = await _userManager.CreateAsync(user, Model.Password);
                if (!result.Succeeded) {
                    foreach (var error in result.Errors)
                        _logger.LogError(error.Description);
                }
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
}