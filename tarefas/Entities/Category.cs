using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categories")]
public class Category 
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [MaxLength(255)]
    public string Description {get; set;}

}