namespace ToDoList.Persistence.Repositories;

public interface IRepository<T>
where T : class
{
    public void Create(T entity);
    public T? Read(int id);
    public IEnumerable<T> ReadAll();
    public void Update(T entity);
    public void Delete(int id);
}
