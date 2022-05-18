using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } = default!;
        public string? UserEmail {get; set;}

        public string? Description {get; set;}

        public string? RatingValue {get; set;}

        public DateTime CreationDate {get; set;}

        public TableRatings? TableRatings { get; set; }
    }
}