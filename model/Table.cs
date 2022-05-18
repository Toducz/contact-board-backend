using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } 
        public string? TableName { get; set; }

        public string? UserEmailOwner { get; set; }

        public string? Descriptions {get; set;}
    }
}