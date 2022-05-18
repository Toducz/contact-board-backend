using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class NewTableRequest
    {

        public string? TableName { get; set; } = default!;
        public string? UserEmailOwner {get; set;}
        
        public string? InvitedPersonEmail {get; set;}

        public string? Description {get; set;}

    }
}