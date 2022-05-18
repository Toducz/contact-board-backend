using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
   
    public class TableRatings
    {
        [Key]
        public int? TableId { get; set; } 
        public ICollection<Rating> Ratings {get; set;} = new List<Rating>();
    }
}