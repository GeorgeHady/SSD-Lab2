using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Lab1.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }


    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }


    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public string BirthDate { get; set; }


}
