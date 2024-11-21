using Microsoft.EntityFrameworkCore;
using tarefas.Entities;
using tarefas.Interfaces;

namespace tarefas.Repositories;

public class TaskRepository : IMyTaskRepository
{

    private readonly Context _context;
    private readonly DateTime _currentTime = DateTime.UtcNow;

    public TaskRepository(Context context) 
    {
        _context = context;
    }


    public async Task Create(MyTask myTask) 
    {
        myTask.CreatedAt = _currentTime;
        _context.Tasks.Add(myTask);
        await _context.SaveChangesAsync();
    }


    public async Task<MyTask?> FindAsync(int id, bool tracking) 
    {
        var query = _context.Tasks.Where(x => x.Id == id).AsQueryable();

        if(!tracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync();
    }


    public async Task Update(int id, MyTask myTask) 
    {
        var task = await this.FindAsync(id, false);

        if(task == null)
        {
            throw new Exception("Tarefa não encontrada");
        }

        myTask.Id = task.Id;

        _context.Entry(myTask).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<MyTask>> List(bool? done, DateTime? initialDate, DateTime? finalDate) 
    {
        var query = _context.Tasks.AsQueryable();
        
        if(done.HasValue)
        {
            query = query.Where(x => x.IsDone == done.Value);
        }

        if(initialDate.HasValue)
        {
            query = query.Where(x => x.CompletionDate.Value.Date >= initialDate.Value.Date && x.CompletionDate.Value.Date <= finalDate.Value.Date);
        }
        
        return await query.Include(x => x.Category).ToListAsync();
    }

    public async Task Delete(int id)
    {
        var task = await this.FindAsync(id, true);

        if(task == null)
        {
            throw new Exception("Tarefa não encontrada");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

}