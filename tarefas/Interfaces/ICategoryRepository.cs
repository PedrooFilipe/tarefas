using tarefas.Entities;

public interface ICategoryRepository 
{

    Task<List<Category>> ListAsync(string description);

}