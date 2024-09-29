using System.ComponentModel.DataAnnotations;

namespace NavYuvakMandarApi.Models
{
    public class AartiDetails
    {
     

        [Key]
        public int aarti_id { get; set; }
        public string? username { get; set;  }
        public string? name { get; set; }
        public DateTime aartiDate { get; set; }

    }
}
