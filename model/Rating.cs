using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace juliWebApi.model
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IgnoreDataMember]
        public int? Id { get; set; } = default!;
        public string? Description { get; set; }
        public string? RatingValue { get; set; }
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        public string? UserForeignKey { get; set; }
        
        [JsonIgnore]
        public Table? Table { get; set; }
    }
}