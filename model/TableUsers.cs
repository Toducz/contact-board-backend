using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class TableUsers
    {
        // ez itt nem key, csak foreign key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } 
        public int? TableId { get; set; } 
        public string? UserEmail {get; set; }
    }
}