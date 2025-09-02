using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Models;

public class Bess {
    public Guid IdBess{get; set; }
    [StringLength(120)]
    public string Name {get; set;}


}
