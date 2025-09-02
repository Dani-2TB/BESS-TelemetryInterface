using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class Module {

    public  Guid Id {get; set; }
    [StringLength(120)]
    public required string Name {get; set;}


}
