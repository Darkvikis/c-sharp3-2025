using System;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories;

public class ToDoItemsRepository(ToDoItemsContext dbContext) : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext dbContext = dbContext;

    public void Create(ToDoItem entity)
    {
        dbContext.ToDoItems.Add(entity);
        dbContext.SaveChanges();
    }

    public ToDoItem? Read(int id) => dbContext.ToDoItems.AsNoTracking().SingleOrDefault(x => x.Id == id);

    public IEnumerable<ToDoItem> ReadAll() => dbContext.ToDoItems.AsNoTracking();

    public void Update(ToDoItem entity)
    {
        dbContext.ToDoItems.Update(entity);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var item = dbContext.ToDoItems.Find(id);
        if (item != null)
        {
            dbContext.ToDoItems.Remove(item);
            dbContext.SaveChanges();
        }
    }
}
