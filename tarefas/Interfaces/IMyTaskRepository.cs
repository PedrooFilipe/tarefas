using tarefas.Entities;

namespace tarefas.Interfaces;

public interface IMyTaskRepository 
{

    Task Create(MyTask myTask);

    Task<MyTask?> FindAsync(int id, bool tracking);

    Task Update(int id, MyTask myTask);

    Task<List<MyTask>> List(bool? done, DateTime? initialDate, DateTime? finalDate);

    Task Delete(int id);
}