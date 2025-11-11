using ToDoList.Persistence;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
    builder.Services.AddControllers();
    builder.Services.AddDbContext<ToDoItemsContext>(options => { });
}
var app = builder.Build();
{
    app.MapControllers();
}

app.Run();

