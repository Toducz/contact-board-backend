using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.entity
{
    public class DefaultTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } = default!;
        public string? UserEmail {get; set;}
        public int? TableId { get; set; } = default!;
    }
}