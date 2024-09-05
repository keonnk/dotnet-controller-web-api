using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_controller_web_api.Models;

namespace dotnet_controller_web_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _todoContext;

    public TodoItemsController(TodoContext todoContext)
    {
        _todoContext = todoContext;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        return await _todoContext.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/TodoItems/:id
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _todoContext.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }

    //PUT: api/TodoItems/:id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if(id !=  todoDTO.Id)
        {
            return BadRequest();
        }
        
        var todoItem = await _todoContext.TodoItems.FindAsync(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _todoContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent(); //change to return updated todoItem
    }

    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name,
        };

        _todoContext.TodoItems.Add(todoItem);
        await _todoContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem)
        );
    }

    // DELETE: api/TodoItems/:id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _todoContext.TodoItems.FindAsync(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        _todoContext.TodoItems.Remove(todoItem);
        await _todoContext.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _todoContext.TodoItems.Any(e => e.Id == id);
    }

    public static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}