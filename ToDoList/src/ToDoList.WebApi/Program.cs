using ToDoList.Persistence;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddScoped<IRepository<ToDoItem>, ToDoItemsRepository>();
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<ToDoItemsContext>(options => { });
}

var app = builder.Build();
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();
}

app.Run();

