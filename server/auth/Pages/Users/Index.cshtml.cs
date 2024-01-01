using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using net8auth.auth.Services;
using net8auth.model;

namespace net8auth.auth.Pages;

public class UsersModel : PageModel
{
    private readonly ILogger<UsersModel> _logger;
    private readonly UsersService _userService;

    public UsersModel(ILogger<UsersModel> logger, UsersService userService)
    {
        _logger = logger;
        _userService = userService;
        Users = new List<ApplicationUser>();
    }

    public List<ApplicationUser> Users { get; set; }
    public async void OnGet()
    {
        var users = await _userService.GetAll();
        _logger.LogInformation($"Got users count = {users.Count}");
        Users = users;
    }
}

