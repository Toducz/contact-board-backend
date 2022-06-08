using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using juliWebApi.model;
using Microsoft.AspNetCore.Authorization;
using juliWebApi.auth;
using System.ComponentModel.DataAnnotations;
using juliWebApi.Request;

/*
    public int? Id { get; set; }  -- general automaticaly
    public string? TableName { get; set; }
    public string? UserIdOwner { get; set; }
*/

public class RequestAddRating
{
    [Required(ErrorMessage = "required UserOwner")]
    public string UserOwnerEmail{ get; set; }

    
    [Required(ErrorMessage = "required tableId")]
    public int tableId { get; set; }

    [Required(ErrorMessage = "required rating")]
    public Rating Rating { get; set; }
}

public class RequestAddTable
{

    [Required(ErrorMessage = "required TableName")]
    public string TableName { get; set; }

    [Required(ErrorMessage = "required Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "required CreateDate")]
    public DateTime CreateDate { get; set; }

    [Required(ErrorMessage = "required UserOwner")]
    public string UserOwnerEmail { get; set; }

    public List<string> SharedWithEmails { get; set; }

}

public class ResponseUsersName{
    public string? lastName {get; set;}
    public string? userEmail {get; set;}
}



[ApiController]
[Route("api/[controller]/[action]")]
public class TableController : ControllerBase
{

    private readonly UserManager<IdentityUser> _userManager;

    private readonly ILogger<TableController> _logger;

    private readonly ApplicationDbContext _dbContext;

    public TableController(
        ILogger<TableController> logger,
        UserManager<IdentityUser> userManager,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<Table>> AddRating(RequestAddRating requestAddRating)
    {

        var table = await _dbContext.Tables.FindAsync(requestAddRating.tableId);

        if (table == null)
        {
            return StatusCode(404, new Response { Status="Error", Message="Table is not found!"});
        }

        var userOwner = await _dbContext.Users.FindAsync(requestAddRating.UserOwnerEmail);;

        if (userOwner == null)
        {
            return StatusCode(404, "User is not found!");
        }

        var rating = new Rating{
            Description = requestAddRating.Rating.Description,
            CreationDate = requestAddRating.Rating.CreationDate,
            RatingValue = requestAddRating.Rating.RatingValue,
            User = userOwner
        };

        try
        {
            table.Ratings.Add(rating);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }

        return table;
    }

    [HttpPost]
    public async Task<ActionResult<Table>> AddTable(RequestAddTable requestAddTable)
    {
        Console.WriteLine("--8--");
        Console.WriteLine(requestAddTable.CreateDate);
        Console.WriteLine("--8--");

        //var userOwner = await _userManager.FindByEmailAsync(requestAddTable.UserOwnerEmail);
        var userOwner = await _dbContext.Users.FindAsync(requestAddTable.UserOwnerEmail);

        if (userOwner == null)
        {
            return StatusCode(404, "User owner not exist!");
        }

        List<User> sharedWith = new List<User>();

        if (requestAddTable.SharedWithEmails is not null)
        {
            foreach (var sharedWithEmail in requestAddTable.SharedWithEmails)
            {
                var sharedUser = await _dbContext.Users.FindAsync(sharedWithEmail);

                if(sharedUser is null){
                    return StatusCode(404, new Response { Status="Error", Message = "Invited person's email not exist!" } );
                }

                if(sharedUser.Email == requestAddTable.UserOwnerEmail){
                    return StatusCode(404, new Response { Status="Error", Message =  "You cannot invite yourself!" }); 
                }

                sharedWith.Add(sharedUser);
            }
        }
        

        var table = new Table{
            TableName = requestAddTable.TableName,
            Description = requestAddTable.Description,
            CreateDate = requestAddTable.CreateDate,
            UserOwner = userOwner,
            SharedWith = sharedWith,
            isDeleted = false
        };

        try{
            _dbContext.Tables.Add(table);
            await _dbContext.SaveChangesAsync();
        }catch(Exception ex){
            Console.WriteLine(ex.Message);
            return StatusCode(500, $"Some internal server error! {ex.Message}" );
        }

        return table;
    }

    [HttpGet]


    public async Task<ActionResult<Table>> GetTable(int TableId){

        var table = await _dbContext.Tables
            .Include(t => t.Ratings)
            .Include(t => t.SharedWith)
            .Include(t => t.UserOwner)
            .Where( t => t.isDeleted == false)
            .Where(t => t.Id == TableId)
            .FirstOrDefaultAsync();

        
        if(table is null){
            return NotFound();
        }
        
        return table;
    }

    [HttpGet]
    public async Task<ActionResult<List<Table>>> GetMyTable(string UserEmail){

        
        //var userOwner = await _userManager.FindByEmailAsync(UserEmail);
        var userOwner = await _dbContext.Users.FindAsync(UserEmail);

        if (userOwner == null)
        {
            return StatusCode(404, "User owner not exist!");
        }


        try{
            var result = await _dbContext.Tables
            .Where( 
                t => t.UserOwner.Email == UserEmail ||
                t.SharedWith.Contains(userOwner)
            )
            .Where(t => t.isDeleted == false)
            .Include(t => t.SharedWith)
            .Include(t => t.Ratings)
            .Include(t => t.UserOwner)
            .ToListAsync();

            return result;
        }catch(Exception ex){
             return StatusCode(500, $"Some internal server error! {ex.Message}" ); 
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTable(int TableId){
        
        var table = await _dbContext.Tables.FindAsync(TableId);

        if (table == null)
        {
            return StatusCode(404, "TableId is not found!");
        }

        table.isDeleted = true;

        try{
            await _dbContext.SaveChangesAsync();
        }catch(Exception ex){
            return StatusCode(500, $"Some internal server error! {ex.Message}");
        }

        return Ok();
    }


    [HttpPost]

    public async Task<ActionResult> SetDefaultTable( RequestSetDefaultTable request){

        var table = await _dbContext.Tables.FindAsync(request.TableId);

        if (table == null)
        {
            return StatusCode(404, new Response { Status="Erorr", Message="Table is not found!" });
        }

        var user = await _dbContext.Users.FindAsync(request.UserEmail);

        if (user == null)
        {
            return StatusCode(404, new Response { Status="Erorr", Message="User owner not exist!" });
        }


        user.Table = table;

        try{
            await _dbContext.SaveChangesAsync();
        }catch(Exception ex){
            return StatusCode(500, new Response { Status="Error", Message="Some internal server error"});
        }

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<Table>> GetDefaultTable( string UserEmail){

        var user = await _dbContext.Users
            .Where( u => u.Email == UserEmail)
            .Include( u => u.Table)
            .Include( u => u.Table.Ratings)
            .Include( u => u.Table.SharedWith)
            .Include( u => u.Table.UserOwner)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return StatusCode(404, new Response { Status="Erorr", Message="User owner not exist!" });
        }

        if (user.Table == null){
            return StatusCode(404, new Response { Status="Erorr", Message="No default table!" });
        }

        return user.Table;
    }


    [HttpPost]
    public async Task<ActionResult<List<ResponseUsersName>>> getUsers( String[] usersId){

        
        var response = await this._dbContext.Users
            .Where(u => usersId.Contains(u.Email))
            .Select(  u => new ResponseUsersName{
                lastName = u.LastName,
                userEmail = u.Email
            })
            .ToListAsync();

        return response;
    }


    
}
