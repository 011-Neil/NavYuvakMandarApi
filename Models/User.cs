using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NavYuvakMandarApi.Models
{
    [Table("RegisterUser")]
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
        public DateTime registration_date { get; set; }

        public string? salt { get; set; }
    }
}
