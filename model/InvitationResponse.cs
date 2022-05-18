using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class InvitationResponse
    {
        public int? InvitationId { get; set; }
        public string? SentInvitationEmail {get; set;}

        public string? TableName {get; set;}

        public string? Description {get; set;}

        public Boolean? Confirmed {get; set;} = false;
    }
}