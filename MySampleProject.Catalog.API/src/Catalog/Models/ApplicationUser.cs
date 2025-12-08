using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{

    public string CardNumber { get; set; }

    public string SecurityNumber { get; set; }

    public string CardHolderName { get; set; }

    public int CardType { get; set; }

    //public string Password { get; set; }

   //public string UserName { get; set; }
}