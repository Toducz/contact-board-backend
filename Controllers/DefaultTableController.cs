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
    private readonly UserManager<MyIdentityUser> _userManager;
    private readonly ILogger<RatingController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public DefaultTableController(
        ILogger<RatingController> logger,
        UserManager<MyIdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<Table>> Get(string userEmail)
    {
        var userOwner = await _userManager.FindByEmailAsync(userEmail);

        if (userOwner is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Your email not valid!" });
        }

        var defaultTable = _dbContext.DefaultTables
            .Where(x => x.UserEmail == userEmail)
            .FirstOrDefault();

        if (defaultTable is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "No default table" });
        }

        var table = await _dbContext.Tables.FindAsync(defaultTable.TableId);
        
        if (table is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Table name not found!" });
        }

        return table;
    }


    [HttpPut]
    public async Task<IActionResult> UpdateOrAdd([FromBody] DefaultTable defaultTable)
    {
        var userOwner = await _userManager.FindByEmailAsync(defaultTable.UserEmail);

        if (userOwner is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Your email not valid!" });
        }

        var response = _dbContext.DefaultTables
            .Where(x => x.UserEmail == defaultTable.UserEmail)
            .FirstOrDefault();

        if (response == null)
        {
            try
            {
                await _dbContext.DefaultTables.AddAsync(defaultTable);

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        response.TableId = defaultTable.TableId;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500);;
        }

        return Ok();
    }

}