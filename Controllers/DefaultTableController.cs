using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using juliWebApi.model;
using juliWebApi.entity;
using Microsoft.AspNetCore.Authorization;
using juliWebApi.auth;

namespace juliWebApi.Controllers;


//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DefaultTableController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<RatingController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public DefaultTableController(
        ILogger<RatingController> logger,
        UserManager<IdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

}