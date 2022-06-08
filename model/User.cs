using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model;

public class User
{
    
    public string ? FirstName { get; set; }
    
    public string ? LastName { get; set; }

    [Key]
    public string ? Email {get; set;}

    [JsonIgnore]
    virtual public string ? UserId {get; set;}

    
    [JsonIgnore]

    public Table ? Table {get; set; }
    

    
    [JsonIgnore]
    public List<Table> ? SharedWith {get; set;}
}