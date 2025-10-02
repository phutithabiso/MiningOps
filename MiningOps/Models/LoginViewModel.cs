using System.ComponentModel.DataAnnotations;

namespace MiningOps.Models
{
    public class LoginViewModel
    {
        [Key]
        public string usernameoremail { get; set; }
       
        public string password { get; set; }

    }
}
