using Microsoft.EntityFrameworkCore;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);

// Add DI - Add methods or add service

builder.Services.AddDbContext<ToDoDb>(opt => opt.UseInMemoryDatabase("ToDoList")); 

var app = builder.Build();

//Configure pipeline - Use method
app.MapGet("/todoitems", async (ToDoDb db) =>
{
    return await db.Todos.ToListAsync();
});

app.MapGet("/todoitems/{id}", async (int id, ToDoDb db) => await db.Todos.FindAsync(id));

app.MapPost("/todoitems", async (ToDoItem todo, ToDoDb db) => 
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async(int id, ToDoItem inputTodo, ToDoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo == null) return Results.NotFound();
    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, ToDoDb db) => 
{
    if(await db.Todos.FindAsync(id) is ToDoItem todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapGet("/", () => "Hello World!");

app.Run();
