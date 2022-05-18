using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using juliWebApi.model;
using Microsoft.AspNetCore.Authorization;
using juliWebApi.auth;

/*
    public int? Id { get; set; }  -- general automaticaly
    public string? TableName { get; set; }
    public string? UserIdOwner { get; set; }
*/

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{

    private readonly UserManager<MyIdentityUser> _userManager;

    private readonly ILogger<TableController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public TableController(
        ILogger<TableController> logger,
        UserManager<MyIdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    
    [HttpGet]
    public List<Table> Get(string userEmail)
    {  
        var myTableId = _dbContext.TableUsers
            .Where(x => x.UserEmail == userEmail)
            .Select(x => x.TableId)
            .Distinct() 
            .ToList();
        
        Console.WriteLine("*");
        Console.WriteLine(myTableId.Count());
        Console.WriteLine("*");

        var result = _dbContext.Tables
            .Where(x => myTableId.Contains(x.Id))
            .ToList();
        
        return result;
    }

    [HttpPost]

    public async Task<ActionResult<Table>> Post(NewTableRequest newTableRequest)
    {
        var userOwner = await _userManager.FindByEmailAsync(newTableRequest.UserEmailOwner);
        
        if(userOwner is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Your email not valid!" });
        }

        var userInvited = await _userManager.FindByEmailAsync(newTableRequest.InvitedPersonEmail);
        
        if(userInvited is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Email not exist!" });
        }

        var existingTablename = _dbContext.Tables
            .Where( x => x.UserEmailOwner == newTableRequest.UserEmailOwner)
            .Where(x => x.TableName == newTableRequest.TableName)
            .FirstOrDefault();

        if(existingTablename is not null){
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Tablename already exist!" });
        }

        var table = new Table{
            TableName = newTableRequest.TableName,
            UserEmailOwner = newTableRequest.UserEmailOwner,
            Descriptions = newTableRequest.Description
        };

        await _dbContext.Tables.AddAsync(table);
        await _dbContext.SaveChangesAsync();


        var tableUsers = new TableUsers{
            UserEmail = newTableRequest.UserEmailOwner,
            TableId = table.Id
        };

        await _dbContext.TableUsers.AddAsync(tableUsers);


        var invitation = new Invitation{
            SentInvitationEmail = newTableRequest.UserEmailOwner,
            InvitedPersonEmail = newTableRequest.InvitedPersonEmail,
            TableId = table.Id,
            Confirmed = false
        };

        await _dbContext.Invitations.AddAsync(invitation);
        await _dbContext.SaveChangesAsync();

        return table;
    }

    [HttpDelete]
    public async Task<ActionResult<Table>> Delete(int id)
    {
        try
        {
            var table =  await _dbContext.Tables.FindAsync(id);

            if (table == null)
            {
                return NotFound($"Table with Id = {id} not found");
            }

            _dbContext.Tables.Remove(table);

            await _dbContext.SaveChangesAsync();

            return Ok(table);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
        }

        // ki kell torolni a tableUsersbol es a ratingseket is ami hozza tartozik
    }

}
