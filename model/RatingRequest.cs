using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace juliWebApi.model
{
    public class RatingRequest
    {
        public int? TableId {get; set;}

        public string? UserEmail {get; set;}

        public string? Description {get; set;}

        public string? RatingValue {get; set;}

        public DateTime CreationDate {get; set;}

    }
}