using Microsoft.EntityFrameworkCore;
using tarefas.Entities;

public class Context : DbContext 
{

    public Context(DbContextOptions<Context> options) : base(options)
    {
        
    }

    public DbSet<MyTask> Tasks {get; set;}

    public DbSet<Category> Categories {get; set;}


}