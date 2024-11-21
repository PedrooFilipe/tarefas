using Microsoft.EntityFrameworkCore;

namespace tarefas.Repositories;

public class CategoryRepository  : ICategoryRepository
{

    private readonly Context _context;

    public CategoryRepository(Context context) 
    {
        _context = context;
    }


    public async Task<List<Category>> ListAsync(string description) 
    {
        var query = _context.Categories.AsNoTracking();

        if(description != null)
        {
            query = query.Where(x => x.Description.Contains(description));
        }

        return await query.ToListAsync();

    }


}