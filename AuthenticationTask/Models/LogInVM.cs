using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationTask.Models
{
    public class LogInVM
    {
      
      [DataType(DataType.EmailAddress)]
       public string Email { get; set; }

        [DataType(DataType.Password)]
       public string Password { get; set; }

       public bool RememberMe { get; set; }
    }
}
