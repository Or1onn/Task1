using System.ComponentModel.DataAnnotations;

namespace SoapService
{
    public class UserModel
    {
        [Key]
        public int Id{ get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public int Age { get; set; }
    }
}
