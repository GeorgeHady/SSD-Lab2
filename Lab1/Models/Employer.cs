using System.ComponentModel.DataAnnotations;

namespace Lab1.Models;

public class Employer
{
    [Key]
    public int Id { get; set; }


    [Required
    , Display(Prompt = "Enter The Name"
    , Description = "Employer Name")]
    public string Name { get; set; }


    [Phone]
    //[DataType(DataType.PhoneNumber)] wrong result(dosnt work)
    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    
    [Required, DataType(DataType.Url)]
    public string Website { get; set; }


    [DataType(DataType.Date)
    , Display(Name = "Incorporated Date")]
    public string IncorporatedDate { get; set; }
}
