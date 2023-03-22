using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ToDoApp.Server.Data;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.Requests;
using ToDoApp.Shared.RequestsUri;
using ToDoApp.Shared.Response;

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
        [Route("paginatedResult")]
        public async Task<IActionResult> GetPaginatedResultToDos([FromQuery] PageRequest pageRequest)
        {
            var result = new List<ToDoModel>();
            if (!string.IsNullOrEmpty(pageRequest.OrderBy))
            {
                result = await _context.ToDos
                              .OrderBy(pageRequest.OrderBy)
                              .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
                              .Take(pageRequest.PageSize)
                              .ToListAsync();
            }
            else
            {
                result = await _context.ToDos
                              .Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
                              .Take(pageRequest.PageSize)
                              .ToListAsync();
            }

            var response = new PageResponse<ToDoModel>
            {
                Data = result,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
                TotalItems = _context.ToDos.Count()
            };

            return Ok(response);
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
                DueDate = new DateTime(addToDo.DueDate.Year, addToDo.DueDate.Month, addToDo.DueDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)
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
                todo.DueDate = new DateTime(updateTodo.DueDate.Year, updateTodo.DueDate.Month, updateTodo.DueDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

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