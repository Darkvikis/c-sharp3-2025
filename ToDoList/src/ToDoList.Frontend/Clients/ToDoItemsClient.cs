namespace ToDoList.Frontend.Clients;

using System.Threading.Tasks;
using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient(HttpClient httpClient) : IToDoItemsClient
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<List<ToDoItemView>> ReadItems()
    {
        List<ToDoItemView> toDoItemViews = [];

        var response = await httpClient.GetFromJsonAsync<List<ToDoItemListItemDto>>("api/todo-items") ?? [];

        response.ForEach(dto => toDoItemViews.Add(MapToView(dto)));

        return toDoItemViews;
    }

    private static ToDoItemView MapToView(ToDoItemListItemDto dto) => new(
            dto.Id,
            dto.Name,
            string.Empty,
            dto.IsCompleted
        );
}
