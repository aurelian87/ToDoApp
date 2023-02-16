using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ToDoApp.Server.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.RequestsUri;

namespace ToDoApp.Server.Controllers
{
    [ApiController]
    [Route(TodoRequestsUri.GetAll)]
    public class ToDosController : Controller
    {
        private readonly ToDosAPIDbContext _context;

        public ToDosController(ToDosAPIDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllToDos()
        {
            return Ok(await _context.ToDos.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetToDo([FromRoute] int id)
        {
            var todo = await _context.ToDos.FindAsync(id);

            if (todo != null)
            {
                return Ok(todo);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddToDo(ToDoModel addToDo)
        {
            var todo = new ToDoModel
            {
                Title = addToDo.Title,
                Description = addToDo.Description,
                DueDate = addToDo.DueDate
            };

            await _context.ToDos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateToDo([FromRoute] int id, ToDoModel updateTodo)
        {
            var todo = _context.ToDos.Find(id);

            if (todo != null)
            {
                todo.Title = updateTodo.Title;
                todo.Description = updateTodo.Description;
                todo.DueDate = updateTodo.DueDate;

                await _context.SaveChangesAsync();

                return Ok(todo);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] int id)
        {
            var todo = _context.ToDos.Find(id);

            if (todo != null)
            {
                _context.ToDos.Remove(todo);
                await _context.SaveChangesAsync();

                return Ok(todo);
            }

            return NotFound();
        }
    }
}