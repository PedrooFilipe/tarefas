using Microsoft.EntityFrameworkCore;
using tarefas.Entities;
using tarefas.Interfaces;
using tarefas.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMyTaskRepository, TaskRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

string connection = builder.Configuration.GetConnectionString("mySql");

builder.Services.AddDbContext<Context>(opt => opt.UseMySql(connection, ServerVersion.AutoDetect(connection)));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/tasks", async (bool? done, DateTime? initialDate, DateTime? finalDate, IMyTaskRepository taskRepository) =>
{
    List<MyTask> tasks = await taskRepository.List(done: done, initialDate: initialDate, finalDate: finalDate);

    return tasks;
})
.WithName("ListTasks")
.WithOpenApi();


app.MapGet("/tasks/{id}", async (int id, IMyTaskRepository taskRepository) =>
{
    MyTask myTask = await taskRepository.FindAsync(id: id, tracking: false);

    return myTask;

})
.WithName("GetSpecificTask")
.WithOpenApi();


app.MapPost("/tasks", async (MyTask myTask, IMyTaskRepository taskRepository) =>
{

    try
    {
        await taskRepository.Create(myTask);

        return Results.Created();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }

})
.WithName("CreateTask")
.WithOpenApi();


app.MapPut("/tasks/{id}", async (int id, MyTask myTask, IMyTaskRepository taskRepository) =>
{

    try
    {
        await taskRepository.Update(id, myTask);

        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }


})
.WithName("UpdateTask")
.WithOpenApi();


app.MapPatch("/tasks/{id}", async (int id, IMyTaskRepository taskRepository) =>
{

    try
    {
        var task = await taskRepository.FindAsync(id, false);

        if (task == null)
        {
            throw new Exception("Tarefa não encontrada");
        }

        task.IsDone = true;
        task.CompletionDate = DateTime.UtcNow;
        await taskRepository.Update(id, task);

        return Results.Ok();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("CompleteTask")
.WithOpenApi();

app.MapDelete("/tasks/{id}", async (int id, IMyTaskRepository taskRepository) =>
{

    try
    {
        await taskRepository.Delete(id);

        return Results.NoContent();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }


})
.WithName("DeleteTask")
.WithOpenApi();


app.MapGet("/categories", async (string? description, ICategoryRepository categoryRepository) =>
{

    List<Category> categories = await categoryRepository.ListAsync(description);
    return categories;

})
.WithName("ListCategories")
.WithOpenApi();



app.Run();
