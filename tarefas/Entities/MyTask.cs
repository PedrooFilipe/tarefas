
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarefas.Entities;

[Table("tasks")]
public class MyTask {

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [MaxLength(100)]
    public string Title {get; set;}
    
    public string Description {get; set;}

    [DefaultValue(false)]
    public bool IsDone {get; set;}

    [ForeignKey("Category")]
    public int CategoryId {get; set;}

    public DateTime? CompletionDate {get; set;}

    public DateTime CreatedAt {get; set;}

    public virtual Category Category {get; set;}

}