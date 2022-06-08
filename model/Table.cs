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
        public string? Description {get; set;}
        public DateTime CreateDate { get; set; }
        public User? UserOwner { get; set; }    
        public Boolean isDeleted{get; set;} = false;


        public virtual List<User> SharedWith { get; set; } = new List<User>();
        

        public virtual List<Rating> Ratings {get; set;} = new List<Rating>();
  
    }
}