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

public class Maci
{

    public static Boolean maci(Boolean? Confirmed)
    {
        if (Confirmed == null)
        {
            return false;
        }
        if (Confirmed == false)
            return false;
        return true;
    }

}



//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InvitationController : ControllerBase
{
    private readonly UserManager<MyIdentityUser> _userManager;
    private readonly ILogger<RatingController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public InvitationController(
        ILogger<RatingController> logger,
        UserManager<MyIdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }



    [HttpGet]
    public async Task<ActionResult<List<InvitationResponse>>> Get(string UserEmail)
    {
        var user = await _userManager.FindByEmailAsync(UserEmail);

        if (user is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "the email address is not registered!" });
        }

        var invitations = _dbContext.Invitations
            .Where(x => x.InvitedPersonEmail == UserEmail)
            .Where(x => x.Confirmed != null && x.Confirmed == false)
            .ToList();

        var tables = _dbContext.Tables
            .Where(x => invitations.Select(x => x.TableId).ToList().Contains(x.Id))
            .ToList();

        var invitationResponses = new List<InvitationResponse>();

        foreach (var invitation in invitations)
        {
            foreach (var table in tables)
            {

                if (invitation.TableId == table.Id)
                {

                    var invitationResponse = new InvitationResponse
                    {

                        InvitationId = invitation.Id,
                        SentInvitationEmail = invitation.SentInvitationEmail,

                        TableName = table.TableName,

                        Description = table.Descriptions,

                        Confirmed = false
                    };

                    invitationResponses.Add(invitationResponse);
                }
            }
        }

        return invitationResponses;
    }
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
