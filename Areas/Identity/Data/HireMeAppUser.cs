using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HireMeApp.Areas.Identity.Data
{
    public class HireMeAppUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
}
}
