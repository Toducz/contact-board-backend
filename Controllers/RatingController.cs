using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using juliWebApi.model;
using Microsoft.AspNetCore.Authorization;
using juliWebApi.auth;

namespace juliWebApi.Controllers;


//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RatingController : ControllerBase
{
    private readonly UserManager<MyIdentityUser> _userManager;
    private readonly ILogger<RatingController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public RatingController(
        ILogger<RatingController> logger,
        UserManager<MyIdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }
    /*
    [HttpGet]
    public async ActionResult<List<Rating>> Get([FromBody]String UserEmail)
    {
        var user = await _userManager.FindByEmailAsync(UserEmail);
        
        if(user is null)
        {
            return NotFound();
        }

        if(ratings is null)
        {
            return NotFound("asdasd");
        }

        return ratings;
    }*/
/*
    [HttpPost]

    public async Task<ActionResult<Rating>> Post(RatingRequest ratingRequest)
    {

        var user = await _userManager.FindByEmailAsync(ratingRequest.UserEmail);

        if(user is null)
        {
            return NotFound();
        }

        Rating rating = new Rating{
            UserId = user.Id,
            Description = ratingRequest.Description,
            RatingValue = ratingRequest.RatingValue,
            CreationDate = ratingRequest.CreationDate
        };

        // tableRatings tablaba hozzaadni a posztot

        await _dbContext.Ratings.AddAsync(rating);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine("Itt vagyok a vegen!");
        
        return rating;
    }*/
}
