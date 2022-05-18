using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class Invitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } = default!;
        public string? SentInvitationEmail {get; set;}

        public string? InvitedPersonEmail {get; set;}

        public int? TableId {get; set;}

        public Boolean? Confirmed {get; set;} = false;
    }
}